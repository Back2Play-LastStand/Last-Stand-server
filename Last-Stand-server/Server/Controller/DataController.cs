using Microsoft.AspNetCore.Mvc;
using Server.Model.Data.Player.Dto.Request;
using Server.Model.Data.Player.Dto.Response;
using Server.Model.Data.Player.Entity;
using Server.Service.Interface;

namespace Server.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly IDataService _dataService;

        public DataController(IDataService dataService)
        {
            _dataService = dataService;
        }

        [HttpGet("{playerId}/name")]
        public async Task<ActionResult<PlayerDataResponse?>> GetPlayerDataAsync(string  playerId)
        {
            var playerData = await _dataService.GetByPlayerIdAsync(playerId);
            if (playerData == null)
                return NotFound(new {Message = "Player Not Found"});
            
            return Ok(new PlayerDataResponse
            {
                PlayerName = playerData.PlayerName,
            });
        }

        [HttpPost("name")]
        public async Task<ActionResult<PlayerDataResponse?>> SavePlayerName([FromBody] PlayerDataRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.PlayerId) || string.IsNullOrWhiteSpace(req.PlayerName))
                return BadRequest(new { message = "PlayerId and PlayerName are required." });
            
            if (await _dataService.IsNameTakenAsync(req.PlayerName))
                return Conflict(new { message = "PlayerName is already taken." });
            
            var existingData = await _dataService.GetByPlayerIdAsync(req.PlayerId);
            if (existingData != null)
            {
                existingData.PlayerName = req.PlayerName;
                await _dataService.AddPlayerDataAsync(existingData);
            }
            else
            {
                var newData = new PlayerData
                {
                    PlayerId = req.PlayerId,
                    PlayerName = req.PlayerName,
                };
                await _dataService.AddPlayerDataAsync(newData);
            }
            
            return Ok(new PlayerDataResponse { PlayerId = req.PlayerId, PlayerName = req.PlayerName });
        }
    }
}
