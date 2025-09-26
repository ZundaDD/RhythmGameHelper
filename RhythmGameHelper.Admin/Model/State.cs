using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RhythmGameHelper.Shared;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhythmGameHelper.Admin.Model
{
    public struct Choice
    {
        public string Explanation;
        public string Input;

        public Choice(string explanation, string input)
        {
            Explanation = explanation;
            Input = input;
        }
    }

    public abstract class State
    {
        protected readonly Formatter _formatter;
        protected readonly IServiceScopeFactory _context;

        protected List<Choice> choices = new();

        public State(Formatter formatter, IServiceScopeFactory context)
        {
            _formatter = formatter;
            _context = context;
        }

        public async Task<string?> GetInput()
        {
            await Task.Delay(1);
            var color = Console.ForegroundColor;
            
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("User> ");
            Console.ForegroundColor = color;

            return Console.ReadLine();
        }

        public abstract Task<State> ExecuteAsync();

        protected async Task<bool> ColumnExistsAsync(string tableName, string columnName)
        {
            await using var scope = _context.CreateAsyncScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await using var connection = dbContext.Database.GetDbConnection();
            await connection.OpenAsync();
            await using var command = connection.CreateCommand();

            command.CommandText = @"
                SELECT 1
                FROM information_schema.columns
                WHERE table_schema = 'public'
                AND table_name = @tableName
                AND column_name = @columnName;";

            var tableNameParam = command.CreateParameter();
            tableNameParam.ParameterName = "@tableName";
            tableNameParam.Value = tableName;
            command.Parameters.Add(tableNameParam);

            var columnParam = command.CreateParameter();
            columnParam.ParameterName = "@columnName";
            columnParam.Value = columnName;
            command.Parameters.Add(columnParam);


            var result = await command.ExecuteScalarAsync();
            return result != null;
        }
    }
}
