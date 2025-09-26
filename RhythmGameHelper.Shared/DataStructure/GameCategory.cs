using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhythmGameHelper.Shared.DataStructure
{
    public class GameCategory
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int GameId { get; set; }

        public Game Game { get; set; } = null!;

        public ICollection<SongInclusion> Inclusions { get; set; } = new List<SongInclusion>();
    }
}
