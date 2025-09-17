using DotNetAssignment.Services;
using DotNetAssignment.Services.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace DotNetAssignment.API.Controllers
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : Controller
    {
        #region Fields
        private readonly IUserService _userService;
        #endregion

        #region Constructors
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        #endregion

        #region Methods
        [HttpPost]
        public async Task<IActionResult> UserRegistration(UserRegistrationModel model)
        {
            return Ok(await _userService.UserRegistration(model));
        }

        [HttpPost]
        public async Task<IActionResult> Login(string icNumber)
        {
            return Ok(await _userService.Login(icNumber));
        }

        [HttpPost]
        public async Task<IActionResult> OTPVerification(UserOTPVerifyModel model)
        {
            return Ok(await _userService.OTPVerification(model));
        }

        [HttpGet]
        public async Task<IActionResult> GetPrivacyPolicy()
        {
            return Ok(await _userService.GetPrivacyPolicy());
        }

        [HttpPost]
        public async Task<IActionResult> PrivacyPolicyAgreement(int userId)
        {
            return Ok(await _userService.PrivacyPolicyAgreement(userId));
        }

        [HttpPost]
        public async Task<IActionResult> CreatePin(UserPinRequestModel model)
        {
            return Ok(await _userService.CreatePin(model));
        }

        [HttpPost]
        public async Task<IActionResult> PinVerification(UserPinRequestModel model)
        {
            return Ok(await _userService.PinVerification(model));
        }

        [HttpPost]
        public async Task<IActionResult> EnableBiometricVerification(int userId)
        {
            return Ok(await _userService.EnableBiometricVerification(userId));
        }

        [HttpGet]
        public async Task<IActionResult> Dashboard(int userId)
        {
            return Ok(await _userService.Dashboard(userId));
        }
        #endregion

    }
}
