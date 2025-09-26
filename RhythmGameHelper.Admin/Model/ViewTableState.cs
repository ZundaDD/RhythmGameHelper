using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using RhythmGameHelper.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RhythmGameHelper.Admin.Model
{
    public class ViewTableState : State
    {
        private string _tableName;
        private int _totalRecords = -1;
        private int _totalPages;
        private bool _insertable = false;
        private int _currentPage = 1;
        private int _pageSize = 10;

        public ViewTableState(Formatter formatter, IServiceScopeFactory context,string tableName, bool insertable) : base(formatter, context) 
        {
            _tableName = tableName;
            _insertable = insertable;

            choices.Add(new Choice("切换到下一页", "n"));
            choices.Add(new Choice("切换到上一页", "p"));
            choices.Add(new Choice("退出表预览", "q"));
            if(insertable) choices.Add(new Choice("进入插入模式", "i"));
            choices.Add(new Choice("切换到指定页", "页号"));
        }

        public override async Task<State> ExecuteAsync()
        {
            if (_totalRecords == -1)
            {
                _totalRecords = await GetRecordCountAsync();
                _totalPages = (int) MathF.Ceiling((float)_totalRecords / _pageSize);
                if (_totalPages == 0) _totalRecords = 1; 
            }

            var (columns, rows) = await GetPaginatedDataAsync();
           
            _formatter.FormatHeader();
            _formatter.FormatTableHeader(_tableName, _totalRecords,_currentPage, _totalPages);
            _formatter.FormatTable(columns, rows);
            _formatter.FormatChoices(choices);

            var input = await GetInput();


            if (input == "n")
            {
                if (_currentPage < _totalPages) _currentPage++;
                return this;
            }
            else if (int.TryParse(input, out int choice) && choice > 0 && choice <= _totalPages)
            {
                _currentPage = choice;
                return this;
            }
            else if (input == "p")
            {
                if (_currentPage > 1) _currentPage--;
                return this;
            }
            else if (input == "q")
            {
                return new MainMenuState(_formatter, _context);
            }
            else if(_insertable && input == "i")
            {
                return new InsertTupleState(_formatter, _context, _tableName);
            }
            else return this;
        }

        private async Task<int> GetRecordCountAsync()
        {
            await using var scope = _context.CreateAsyncScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            
            await using var connection = dbContext.Database.GetDbConnection();
            await connection.OpenAsync();
            await using var command = connection.CreateCommand();
            command.CommandText = $"SELECT COUNT(*) FROM \"{_tableName}\"";

            var result = await command.ExecuteScalarAsync();
            return result is null ? 0 : Convert.ToInt32(result);
        }

        private async Task<(List<string> Columns, List<Dictionary<string, object>> Rows)> GetPaginatedDataAsync()
        {
            await using var scope = _context.CreateAsyncScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await using var connection = dbContext.Database.GetDbConnection();
            await connection.OpenAsync();
            await using var command = connection.CreateCommand();

            var offset = (_currentPage - 1) * _pageSize;
            var limit = _pageSize;

            if (await ColumnExistsAsync(_tableName, "Id"))
                command.CommandText = $"SELECT * FROM \"{_tableName}\" ORDER BY \"Id\" ASC OFFSET @offset LIMIT @limit";
            else
                command.CommandText = $"SELECT * FROM \"{_tableName}\" ORDER BY (SELECT NULL) OFFSET @offset LIMIT @limit";

            var offsetParam = command.CreateParameter();
            offsetParam.ParameterName = "@offset";
            offsetParam.Value = offset;
            command.Parameters.Add(offsetParam);

            var limitParam = command.CreateParameter();
            limitParam.ParameterName = "@limit";
            limitParam.Value = limit;
            command.Parameters.Add(limitParam);

            await using var reader = await command.ExecuteReaderAsync();

            var columns = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToList();
            var rows = new List<Dictionary<string, object>>();

            while (await reader.ReadAsync())
            {
                var row = new Dictionary<string, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    row[columns[i]] = reader.GetValue(i);
                }
                rows.Add(row);
            }

            return (columns, rows);
        }
    }
}
