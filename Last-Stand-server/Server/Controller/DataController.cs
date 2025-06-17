using Microsoft.AspNetCore.Mvc;
using Server.Model.Account.Dto.Response;
using Server.Model.Account.Entity;
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
        private readonly IAccountService _accountService;

        public DataController(IDataService dataService,  IAccountService accountService)
        {
            _dataService = dataService;
            _accountService = accountService;
        }

        [HttpGet("name")]
        public async Task<ActionResult<PlayerNameResponse?>> GetPlayerDataAsync(
            [FromQuery] string  playerId,
            [FromServices] ISessionService sessionService,
            [FromServices] IAccountService accountService)
        {
            var (isSuccess, errorResult, playerData) = await ValidateSessionAndGetPlayerAsync(playerId, sessionService, accountService);
            if (!isSuccess)
                return errorResult!;
            
            var data = await _dataService.GetByPlayerIdAsync(playerId);
            if (data == null)
                return NotFound(new {Message = "Player Not Found"});
            
            return Ok(new PlayerNameResponse
            {
                PlayerName = data.PlayerName,
            });
        }

        [HttpPost("name")]
        public async Task<ActionResult<PlayerDataResponse>> AddPlayerName(
            [FromBody] PlayerDataRequest req,
            [FromServices] ISessionService sessionService,
            [FromServices] IAccountService accountService)
        {
            if (string.IsNullOrWhiteSpace(req.PlayerId) || string.IsNullOrWhiteSpace(req.PlayerName))
                return BadRequest(new { message = "PlayerId and PlayerName are required." });
            
            var (isSuccess, errorResult, playerData) = await ValidateSessionAndGetPlayerAsync(req.PlayerId, sessionService, accountService);
            if (!isSuccess)
                return errorResult!;

            if (await _dataService.IsNameTakenAsync(req.PlayerName))
                return Conflict(new { message = "PlayerName is already taken." });

            var isNewAccount = await _accountService.CheckIsNewAccountByPlayerIdAsync(req.PlayerId);
            if (isNewAccount == null)
                return NotFound(new { message = "Player Not Found" });

            if (isNewAccount == false)
                return Conflict(new { message = "This account is not New" });

            var loginData = await _accountService.GetPlayerLoginDataByPlayerIdAsync(req.PlayerId);
            if (loginData == null)
                return NotFound(new { message = "Player Not Found" });

            var newData = new PlayerData
            {
                Id = loginData.Id,
                PlayerId = req.PlayerId,
                PlayerName = req.PlayerName
            };

            await _dataService.AddPlayerDataAsync(newData, false);
            await _accountService.UpdateIsNewAccountAsync(req.PlayerId, false);

            return Ok(new PlayerDataResponse
            {
                PlayerId = req.PlayerId,
                PlayerName = req.PlayerName
            });
        }

        private async Task<(bool IsSuccess, ActionResult? ErrorResult, PlayerLoginData? PlayerData)>
            ValidateSessionAndGetPlayerAsync(string playerId,ISessionService sessionService, IAccountService accountService)
        {
            if (!Request.Headers.TryGetValue("Session-Id", out var sessionId))
                return (false, Unauthorized(new { Message = "Session Id Is Not Found" }), null);

            var accountId = await sessionService.GetAccountIdBySessionIdAsync(sessionId);
            if (accountId == null)
                return (false, Unauthorized(new { Message = "Invalid or expired session." }), null);

            var playerData = await accountService.GetPlayerLoginDataByIdAsync(accountId.Value);
            if (playerData == null || playerData.PlayerId != playerId)
                return (false, Unauthorized(new { Message = "Session does not match player." }), null);

            return (true, null, playerData);
        }
    }
}
