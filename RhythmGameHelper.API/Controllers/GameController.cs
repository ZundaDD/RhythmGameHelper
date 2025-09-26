using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RhythmGameHelper.Shared.DataQuery;
using RhythmGameHelper.Shared.DataStructure;
using RhythmGameHelper.Shared;

namespace RhythmGameHelper.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GameController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/game
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Game>>> GetGames()
        {
            var games = await _context.GameData.OrderBy(g => g.Id).ToListAsync();
            return Ok(games);
        }

        [HttpGet("{gameId}/categories")]
        public async Task<ActionResult<IEnumerable<string>>> GetGameCategories([FromRoute] int gameId)
        {
            var gameExists = await _context.GameData.AnyAsync(g => g.Id == gameId);
            if (!gameExists) return NotFound($"Game with ID {gameId} not found.");

            var game = await _context.GameCategories
                .Where(gc => gc.GameId == gameId)
                .Select(gc => gc.Name)
                .ToListAsync();
            return Ok(game);
        }

        [HttpGet("{gameId}/versions")]
        public async Task<ActionResult<IEnumerable<string>>> GetGameVersions([FromRoute] int gameId)
        {
            var gameExists = await _context.GameData.AnyAsync(g => g.Id == gameId);
            if (!gameExists) return NotFound($"Game with ID {gameId} not found.");

            var game = await _context.GameVersions
                .Where(gv => gv.GameId == gameId)
                .OrderBy(gv => gv.StartTime)
                .Select(gc => gc.Name)
                .ToListAsync();

            return Ok(game);
        }
    }
}
