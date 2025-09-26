using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhythmGameHelper.Shared.DataStructure
{
    public class SongInclusion
    {
        public int GameId { get; set; }
        public Game Game { get; set; } = null!;

        public int SongId { get; set; }
        public Song Song { get; set; } = null!;

        public int? CategoryId { get; set; }
        public GameCategory? Category { get; set; } = null!;

        public int? VersionId { get; set; }
        public GameVersion? Version { get; set; } = null!;

        //是否删除
        public bool Delete { get; set; }
        //是否为原创曲
        public bool Original { get; set; }

        public ICollection<Chart> Charts { get; set; } = new List<Chart>();
    }
}
