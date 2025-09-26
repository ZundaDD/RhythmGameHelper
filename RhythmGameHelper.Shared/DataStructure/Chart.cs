using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhythmGameHelper.Shared.DataStructure
{
    /// <summary>
    /// 谱面数据
    /// </summary>
    public class Chart
    {
        //唯一ID
        public int Id { get; set; }

        //对应的收录
        public int SongId { get; set; }
        public int GameId { get; set; }
        public SongInclusion SongInclusion { get; set; } = null!;

        //谱师
        public string Composer { get; set; } = string.Empty;
        //谱面添加版本
        public string Version { get; set; } = string.Empty;
        //难度名称
        public string DifficultyName { get; set; } = string.Empty;
        //难度定数
        public float DifficultyLevel { get; set; }
        //是否删除
        public bool Delete { get; set; }
        //物量
        public int Combo { get; set; }
    }
}
