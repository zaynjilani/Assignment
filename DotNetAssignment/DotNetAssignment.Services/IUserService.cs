using DotNetAssignment.Services.DTOs;
using DotNetAssignment.Services.Models;

namespace DotNetAssignment.Services
{
    public interface IUserService
    {

        Task<GeneralModel> UserRegistration(UserRegistrationModel data);
        Task<GeneralModel> CreatePin(UserPinRequestModel data);
        Task<GeneralModel> PinVerification(UserPinRequestModel data);
        Task<GeneralModel> OTPVerification(UserOTPVerifyModel data);
        Task<GeneralModel> EnableBiometricVerification(int userId);
        Task<GeneralModel> PrivacyPolicyAgreement(int userId);
        Task<GeneralModel> Login(string icNumber);
        Task<GeneralModel> GetPrivacyPolicy();
        Task<GeneralModel> Dashboard(int userId);
    }
}