using Microsoft.AspNetCore.Mvc;
using Server.Model.Account.Dto.Request;
using Server.Model.Account.Dto.Response;
using Server.Service.Interface;
using StackExchange.Redis;

namespace Server.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private const string VerificationKeyPrefix = "verify:email:";

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("player-id")]
        public async Task<ActionResult<FindPlayerIdResponse>> FindPlayerId(
            [FromQuery] string email,
            [FromServices] IConnectionMultiplexer redis)
        {
            var redisDb = redis.GetDatabase();
            var isVerified = await redisDb.StringGetAsync($"{VerificationKeyPrefix}{email}");
            if (isVerified != "true")
                return Unauthorized(new { Message = "Email not verified." });

            var playerId = await _accountService.FindPlayerIdByEmailAsync(email);
            if (playerId == null)
                return NotFound(new FindPlayerIdResponse { PlayerId = null });

            return Ok(new FindPlayerIdResponse { PlayerId = playerId });
        }

        [HttpPatch("password")]
        public async Task<ActionResult<ResetPasswordResponse>> ResetPassword(
            [FromBody] ResetPasswordRequest req,
            [FromServices] IConnectionMultiplexer redis)
        {
            var redisDb = redis.GetDatabase();
            var key = $"{VerificationKeyPrefix}{req.Email}";

            var isVerified = await redisDb.StringGetAsync(key);
            if (isVerified != "true")
                return Unauthorized(new { Message = "Email not verified." });

            var success = await _accountService.ResetPasswordAsync(req.PlayerId, req.Email, req.NewPassword);
            if (!success)
                return BadRequest(new ResetPasswordResponse { Success = false, Message = "PlayerId and email do not match." });

            await redisDb.KeyDeleteAsync(key); // 인증 후 키 삭제

            return Ok(new ResetPasswordResponse { Success = true, Message = "Password has been reset successfully." });
        }
    }

}
