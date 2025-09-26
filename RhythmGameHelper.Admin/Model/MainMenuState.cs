using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RhythmGameHelper.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhythmGameHelper.Admin.Model
{
    public class MainMenuState : State
    {

        private List<string> tables = null!;

        public MainMenuState(Formatter formatter, IServiceScopeFactory context) : base(formatter, context)
        {
            choices.Add(new Choice("浏览对应表数据", "表序号"));
            choices.Add(new Choice("退出程序", "q"));
        }

        public async override Task<State> ExecuteAsync()
        {
            if(tables == null)
            {
                tables = await GetTableNamesAsync();
            }

            _formatter.FormatHeader();
            _formatter.FormatTables(tables);
            _formatter.FormatChoices(choices);

            var input = await GetInput();

            if (input?.ToLower() == "q")
            {
                return new ExitState(_formatter, _context);
            }
            else if (int.TryParse(input, out int choice) && choice > 0 && choice <= tables.Count)
            {
                return new ViewTableState(_formatter, _context, tables[choice - 1], await ColumnExistsAsync(tables[choice - 1], "Id"));
            }
            else
            {
                return this;
            }
        }

        private async Task<List<string>> GetTableNamesAsync()
        {
            var tableNames = new List<string>();

            await using var scope = _context.CreateAsyncScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await using var connection = dbContext.Database.GetDbConnection();
            await connection.OpenAsync();

            await using var command = connection.CreateCommand();

            command.CommandText = @"
            SELECT table_name 
            FROM information_schema.tables 
            WHERE table_schema = 'public' 
              AND table_type = 'BASE TABLE'
              AND table_name != '__EFMigrationsHistory'
            ORDER BY table_name;";

            try
            {
                await using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    tableNames.Add(reader.GetString(0));
                }
            }
            catch (Exception) { }

            return tableNames;
        }
    }
}
