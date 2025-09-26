using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhythmGameHelper.Shared.DataQuery
{
    /// <summary>
    /// 标准的查询对象
    /// </summary>
    public class SongQuery
    {
        public string SongName { get; set; } = string.Empty;
        public string Artist { get; set; } = string.Empty;
        public string? VersionName { get; set; }
        public string? CategoryName { get; set; }
        public int? GameId { get; set; }
        public int MinBPM { get; set; } = 0;
        public int MaxBPM { get; set; } = int.MaxValue;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public bool ShowNotOriginal { get; set; } = true;
        public bool ShowDeleted { get; set; } = true;
    }
}
