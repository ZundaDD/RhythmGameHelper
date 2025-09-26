using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using RhythmGameHelper.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace RhythmGameHelper.Admin.Model
{
    public class FSM
    {
        public State State { get; private set; }

        public FSM(Formatter formatter, IServiceScopeFactory context)
        {
            State = new MainMenuState(formatter, context);
        }

        public async Task Loop()
        {
            while(State != null)
            {
                State = await State.ExecuteAsync();
            }
        }
    }
}
