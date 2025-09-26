using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhythmGameHelper.Shared.DataStructure
{
    /// <summary>
    /// 单曲数据，内容是仅依赖歌曲的
    /// </summary>
    public class Song
    {
        //唯一ID
        public int Id { get; set; }
        //官方BPM
        public float OfficialBPM { get; set; }
        //官方BPM备用字段，用于范围
        public float? MaxOfficialBPM { get; set; }
        //曲名
        public string Name { get; set; } = string.Empty;
        //作曲师
        public List<string>? Artists { get; set; } = new();
        //官方名义
        public string? OfficialArtist { get; set; } = string.Empty;
        //收录情况
        public ICollection<SongInclusion> Inclusions { get; set; } = new List<SongInclusion>();
    }
}
