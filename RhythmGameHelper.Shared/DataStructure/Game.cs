using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhythmGameHelper.Shared.DataStructure
{
    /// <summary>
    /// 游戏数据
    /// </summary>
    public class Game
    {
        //唯一ID
        public int Id { get; set; }
        //名称
        public string Name { get; set; } = string.Empty;
        //统一英文
        public string EnglishName { get; set; } = string.Empty;

        public ICollection<SongInclusion> Inclusions { get; set; } = new List<SongInclusion>();
        public ICollection<GameCategory> Categories { get; set; } = new List<GameCategory>();
        public ICollection<GameVersion> Versions { get; set; } = new List<GameVersion>();
    }
}
