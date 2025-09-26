using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using RhythmGameHelper.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Npgsql.EntityFrameworkCore.PostgreSQL.Query.Expressions.Internal.PgTableValuedFunctionExpression;

namespace RhythmGameHelper.Admin.Model
{
    public class InsertTupleState : State
    {
        private string _tableName;
        private int _maxId = -1;
        private List<string> _columns = null!;
        private List<int> _columnLens = null!;
        private List<string> _inputs = null!;
        private string _exception = string.Empty;
        private int _currentProperty = 0;

        public InsertTupleState(Formatter formatter, IServiceScopeFactory context, string tableName) : base(formatter, context)
        {
            _tableName = tableName;
            choices.Add(new Choice("插入元组", "i"));
            choices.Add(new Choice("切换到上一个字段", "p"));
            choices.Add(new Choice("切换到下一个字段", "n"));
            choices.Add(new Choice("清空当前字段", "c"));
            choices.Add(new Choice("退出表插入", "q"));
        }

        public override async Task<State> ExecuteAsync()
        {
            if(_maxId == -1)
            {
                _columns = await GetColumnsAsync(_tableName);
                _columnLens = new();
                _inputs = new();
                foreach (var column in _columns)
                {
                    _columnLens.Add(column.Length + 2);
                    _inputs.Add("");
                }
                _maxId = await GetMaxIdAsync();
            }

            _formatter.FormatHeader();
            _formatter.FormatException(_exception);
            _formatter.FormatTableMaxId(_tableName, _maxId);
            _formatter.FormatTableColumnName(_columnLens,_columns, _currentProperty);
            _formatter.FormatTableColumnName(_columnLens, _inputs, _currentProperty);
            _formatter.FormatChoices(choices);

            var input = await GetInput();
            _exception = string.Empty;

            if(input == "q")
            {
                //能进来，肯定能插入
                return new ViewTableState(_formatter, _context, _tableName, true);
            }
            else if(input == "p")
            {
                if(_currentProperty > 0) _currentProperty--;
                return this;
            }
            else if (input == "c")
            {
                _inputs[_currentProperty] = "";
                return this;
            }
            else if(input == "n")
            {
                if(_currentProperty < _inputs.Count - 1) _currentProperty++;
                return this;
            }
            else if(input == "i")
            {
                _maxId = await InsertTupleAsync();
                if (_currentProperty == _inputs.Count - 1)
                {
                    for (int i = 0; i < _inputs.Count; i++) _inputs[i] = "";
                    _currentProperty = 0;
                }
                return this;
            }
            else
            {
                _inputs[_currentProperty] = $"{input}";
                _columnLens[_currentProperty] = Math.Max(_columnLens[_currentProperty]
                    , _inputs[_currentProperty].DisplayLength());
                if (_currentProperty < _inputs.Count - 1) _currentProperty++;

                return this;
            }
        }

        private async Task<int> InsertTupleAsync()
        {
            await using var scope = _context.CreateAsyncScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await using var connection = dbContext.Database.GetDbConnection();
            await connection.OpenAsync();
            await using var command = connection.CreateCommand();

            try
            {
                var keys = new List<string>();
                keys.Add("Id");
                var values = new List<object>();
                values.Add(_maxId + 1);

                for (int i = 0; i < _inputs.Count; ++i)
                {
                    if (_inputs[i] != string.Empty)
                    {
                        keys.Add(_columns[i].Split(':')[0]);
                        values.Add(ParseValue(_columns[i].Split(':')[1].Trim(), _inputs[i]));
                    }
                }

                var columnNamesSql = string.Join(", ", keys.Select(name => $"\"{name}\""));
                var paramNamesSql = string.Join(", ", Enumerable.Range(0, keys.Count).Select(i => $"@p{i}"));

                command.CommandText = $"INSERT INTO public.\"{_tableName}\" ({columnNamesSql}) VALUES ({paramNamesSql});";

                for (int i = 0; i < values.Count; i++) command.AddParam($"@p{i}", values[i]);

                var result = await command.ExecuteScalarAsync();
            }
            catch (Exception ex)
            {

                if (ex is DbUpdateException dbUpdateEx && dbUpdateEx.InnerException != null)
                    _exception = $"插入失败: {dbUpdateEx.InnerException.Message}";
                else _exception = $"插入失败: {ex.Message}";

                _exception += $"\n{command.CommandText}";
                return _maxId;
            }

            return _maxId + 1;
        }

        private async Task<int> GetMaxIdAsync()
        {
            await using var scope = _context.CreateAsyncScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await using var connection = dbContext.Database.GetDbConnection();
            await connection.OpenAsync();
            await using var command = connection.CreateCommand();
            command.CommandText = $"SELECT MAX(\"Id\") FROM \"{_tableName}\"";

            var result = await command.ExecuteScalarAsync();
            return result is null ? 0 : Convert.ToInt32(result);

        }

        public async Task<List<string>> GetColumnsAsync(string tableName)
        {
            await using var scope = _context.CreateAsyncScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await using var connection = dbContext.Database.GetDbConnection();
            await connection.OpenAsync();
            await using var command = connection.CreateCommand();

            command.CommandText = @"
                SELECT
                    column_name,
                    udt_name,
                    ordinal_position
                FROM
                    information_schema.columns
                WHERE
                    table_schema = 'public'
                    AND table_name = @tableName
                ORDER BY
                    ordinal_position ASC;";

            var tableNameParam = command.CreateParameter();
            tableNameParam.ParameterName = "@tableName";
            tableNameParam.Value = tableName;
            command.Parameters.Add(tableNameParam);

            await using var reader = await command.ExecuteReaderAsync();
            var columns = new List<string>();

            if (!reader.HasRows) return columns;
            
            while (await reader.ReadAsync())
            {
                var name = reader.GetString(reader.GetOrdinal("column_name"));
                var dataType = reader.GetString(reader.GetOrdinal("udt_name"));

                if (name == "Id") continue;
                columns.Add($"{name}: {dataType}");
            }

            return columns;
        }

        private object ParseValue(string type, string value)
        {
            switch (type)
            {
                case "float4":
                    return float.Parse(value);
                case "int4":
                    return int.Parse(value);
                case "date":
                    return DateTime.Parse(value);
                case "_text":
                    return value.Split(",");
                default:
                    return value;
            }

        }
    }
}
