using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RhythmGameHelper.Shared;
using RhythmGameHelper.Shared.DataQuery;
using RhythmGameHelper.Shared.DataStructure;
using RhythmGameHelper.Shared.DataTransfer;
using System.Linq;

namespace RhythmGameHelper.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SongController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SongController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/song/search
        [HttpPost("search")]
        public async Task<ActionResult<PackedSong>> GetSongsByGame([FromBody] SongQuery songQuery)
        {
            if (songQuery == null) return NoContent();

            var query = _context.SongData.AsQueryable();

            //处理SongData来源
            if (songQuery.GameId.HasValue)
            {
                var gameExists = await _context.GameData.AnyAsync(g => g.Id == songQuery.GameId);
                if (!gameExists) return NotFound($"Game with ID {songQuery.GameId} not found.");

                var gameQuery = _context.GameData
                    .Where(g => g.Id == songQuery.GameId)
                    .SelectMany(g => g.Inclusions);

                //联动和删除标签只对指定游戏有效
                if (!songQuery.ShowDeleted)
                {
                    gameQuery = gameQuery.Where(i => !i.Delete);
                }

                if(!songQuery.ShowNotOriginal)
                {
                    gameQuery = gameQuery.Where(i => i.Original);
                }

                if(!string.IsNullOrWhiteSpace(songQuery.CategoryName))
                {
                    gameQuery = gameQuery.Where(i => i.Category != null && i.Category.Name == songQuery.CategoryName);
                }

                if (!string.IsNullOrWhiteSpace(songQuery.VersionName))
                {
                    gameQuery = gameQuery.Where(i => i.Version != null && i.Version.Name == songQuery.VersionName);
                }

                query = gameQuery.Select(si => si.Song).Distinct();
            }

            //按名称筛选
            if (!string.IsNullOrWhiteSpace(songQuery.SongName))
            {
                var searchTerm = songQuery.SongName.ToLower();
                query = query.Where(s => s.Name.ToLower().Contains(searchTerm));
            }

            //按曲师筛选
            if( !string.IsNullOrWhiteSpace(songQuery.Artist))
            {
                var searchTerm = songQuery.Artist.ToLower();
                query = query.Where(s => s.Artists != null && s.Artists.Any(s => s.ToLower().Contains(searchTerm)));
            }

            //按BPM筛选
            query = query
                .Where(s => s.OfficialBPM >= songQuery.MinBPM && s.OfficialBPM <= songQuery.MaxBPM);

            //先计数再筛选
            var count = await query.CountAsync();

            var packedData = await query
                .Skip((songQuery.PageNumber - 1) * songQuery.PageSize)
                .Take(songQuery.PageSize)
                .ToListAsync();

            //将Song转化为SongDto，减少传输量
            var songs = packedData.Select(song => new SongDto
            {
                Id = song.Id,
                SongName = song.Name,
                OfficialArtist = song.OfficialArtist,
                Artists = song.Artists,
                OfficialBPM = song.OfficialBPM,
                IncludedGames = song.Inclusions.Select(inclusion => inclusion.Game.Name).ToList()
            }).ToList();

            var packedSongs = new PackedSong()
            {
                Songs = songs,
                TotalCount = count,
                PageNumber = songQuery.PageNumber,
                PageSize = songQuery.PageSize
            };

            return Ok(packedSongs);
        }
    }
}
