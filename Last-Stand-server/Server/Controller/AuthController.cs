using Microsoft.AspNetCore.Mvc;
using Server.Model.Account.Dto.Request;
using Server.Model.Account.Dto.Response;
using Server.Service.Interface;

namespace Server.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<PlayerRegisterResponse>> Register([FromBody] PlayerRegisterRequest req)
        {
            var success = await _authService.RegisterAsync(req.PlayerId, req.Password, req.Email);
            if (!success)
                return Conflict(new PlayerRegisterResponse
                {
                    PlayerId = req.PlayerId,
                    Message = "Id already exists",
                    Email = req.Email
                });
            
            return Ok(new  PlayerRegisterResponse
            {
                PlayerId = req.PlayerId,
                Message = "Register Success",
                Email = req.Email
            });
        }

        [HttpPost("login")]
        public async Task<ActionResult<PlayerLoginResponse>> Login([FromBody] PlayerLoginRequest req)
        {
            var (token, isNewAccount) = await _authService.LoginAsync(req.PlayerId, req.Password);
            if (token == null)
            {
                return Unauthorized(new PlayerLoginResponse
                {
                    PlayerId = req.PlayerId,
                    Message = "Login Failed"
                });
            }
            
            return Ok(new PlayerLoginResponse
            {
                PlayerId = req.PlayerId,
                AccessToken = token.AccessToken,
                RefreshToken = token.RefreshToken,
                IsNewAccount = isNewAccount,
                Message = "Login successful"
            });
        }
    }
}
