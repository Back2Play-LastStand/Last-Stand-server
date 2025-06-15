using Microsoft.AspNetCore.Mvc;
using Server.Model.Account.Dto.Request;
using Server.Model.Account.Dto.Response;
using Server.Service.Interface;

namespace Server.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("player-id")]
        public async Task<ActionResult<FindPlayerIdResponse>> FindPlayerId([FromQuery] string email)
        {
            var playerId = await _accountService.FindPlayerIdByEmailAsync(email);
            if (playerId == null)
                return NotFound(new FindPlayerIdResponse { PlayerId = null });
            
            return Ok(new  FindPlayerIdResponse { PlayerId = playerId });
        }

        [HttpPatch("password")]
        public async Task<ActionResult<ResetPasswordResponse>> ResetPassword([FromBody] ResetPasswordRequest req)
        {
            var success = await _accountService.ResetPasswordAsync(req.PlayerId, req.Email, req.NewPassword);
    
            if (!success)
                return BadRequest(new ResetPasswordResponse { Success = false, Message = "PlayerId and email do not match." });

            return Ok(new ResetPasswordResponse { Success = true, Message = "Password has been reset successfully." });
        }
    }
}
