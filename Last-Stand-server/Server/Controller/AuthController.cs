using Microsoft.AspNetCore.Mvc;
using Server.DefinedException;
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
        private readonly ISessionService _sessionService;

        public AuthController(IAuthService authService,  ISessionService sessionService)
        {
            _authService = authService;
            _sessionService = sessionService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<PlayerRegisterResponse>> Register([FromBody] PlayerRegisterRequest req)
        {
            try
            {
                var success = await _authService.RegisterAsync(req.PlayerId, req.Password, req.Email);
                if (!success)
                {
                    return Conflict(new PlayerRegisterResponse
                    {
                        PlayerId = req.PlayerId,
                        Message = "Register failed for unknown reason",
                        Email = req.Email
                    });
                }

                return Ok(new PlayerRegisterResponse
                {
                    PlayerId = req.PlayerId,
                    Message = "Register Success",
                    Email = req.Email
                });
            }
            catch (DuplicatePlayerIdException)
            {
                return Conflict(new PlayerRegisterResponse
                {
                    PlayerId = req.PlayerId,
                    Message = "PlayerId already exists",
                    Email = req.Email
                });
            }
            catch (DuplicateEmailException)
            {
                return Conflict(new PlayerRegisterResponse
                {
                    PlayerId = req.PlayerId,
                    Message = "Email already exists",
                    Email = req.Email
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new PlayerRegisterResponse
                {
                    PlayerId = req.PlayerId,
                    Message = "Internal server error: " + ex.Message,
                    Email = req.Email
                });
            }
        }


        [HttpPost("login")]
        public async Task<ActionResult<PlayerLoginResponse>> Login([FromBody] PlayerLoginRequest req)
        {
            var (isSuccess, isNewAccount) = await _authService.LoginAsync(req.PlayerId, req.Password);
            if (!isSuccess)
            {
                return Unauthorized(new PlayerLoginResponse
                {
                    PlayerId = req.PlayerId,
                    Message = "Login Failed"
                });
            }

            
            try
            {
                await _sessionService.DeleteSessionByPlayerIdAsync(req.PlayerId);
                var sessionId = await _sessionService.CreateSessionAsync(req.PlayerId);

                return Ok(new PlayerLoginResponse
                {
                    PlayerId = req.PlayerId,
                    IsNewAccount = isNewAccount,
                    SessionId = sessionId,
                    Message = "Login successful"
                });
            }
            catch (InvalidOperationException)
            {
                return Conflict(new PlayerLoginResponse
                {
                    PlayerId = req.PlayerId,
                    IsNewAccount = isNewAccount,
                    Message = "Already logged in"
                });
            }
        }

        [HttpDelete("logout")]
        public async Task<IActionResult> Logout([FromQuery] string sessionId)
        {
            if (string.IsNullOrEmpty(sessionId))
                return BadRequest("SessionId is empty");
            
            await _sessionService.DeleteSessionAsync(sessionId);

            return Ok(new {message = "Logout succeed"});
        }
    }
}
