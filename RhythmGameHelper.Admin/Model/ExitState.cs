using Microsoft.Extensions.DependencyInjection;
using RhythmGameHelper.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhythmGameHelper.Admin.Model
{
    public class ExitState : State
    {
        public ExitState(Formatter formatter, IServiceScopeFactory context) : base(formatter, context) { }

        public override async Task<State> ExecuteAsync()
        {
            Console.WriteLine("退出系统中...");
            await Task.Delay(1000);
            return null!;
        }
    }
}
