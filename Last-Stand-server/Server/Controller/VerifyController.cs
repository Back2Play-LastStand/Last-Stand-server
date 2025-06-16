using Microsoft.AspNetCore.Mvc;
using Server.Model.Verify.Dto.Request;
using Server.Service.Interface;
using StackExchange.Redis;

namespace Server.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class VerifyController : ControllerBase
    {
        private readonly IVerificationService _verificationService;
        private readonly IEmailService _emailService;

        public VerifyController(
            IVerificationService verificationService,
            IEmailService emailService)
        {
            _verificationService = verificationService;
            _emailService = emailService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendVerificationCode([FromQuery] string email)
        {
            var code = new Random().Next(100000, 999999).ToString();

            await _verificationService.StoreVerificationCodeAsync(email, code);

            var subject = "[Last Stand] 이메일 인증코드";
            var body = code;
            await _emailService.SendAsync(email, subject, body);

            return Ok(new { Message = "Code sent to your email." });
        }

        [HttpPost]
        public async Task<IActionResult> VerifyCode([FromBody] EmailVerifyRequest req)
        {
            var isValid = await _verificationService.VerifyCodeAsync(req.Email, req.Code);
            if (!isValid)
                return BadRequest(new { Message = "Invalid or expired verification code." });

            await _verificationService.MarkEmailVerifiedAsync(req.Email);

            return Ok(new { Message = "Email verification successful." });
        }
    }
}
