using DotNetAssignment.DAL;
using DotNetAssignment.Entities;
using DotNetAssignment.Services.DTOs;
using DotNetAssignment.Services.Helpers;
using DotNetAssignment.Services.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetAssignment.Services
{
    public class UserService : IUserService
    {
        #region Fields
        private readonly DatabaseContext _context;
        private readonly Logger _logger;
        #endregion

        #region Constructors
        public UserService(DatabaseContext context, Logger logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<GeneralModel> CreatePin(UserPinRequestModel data)
        {
            try
            {
                _logger.Log($"CreatePin method called with UserId: {data.UserId}");

                if (data.Pin < 100000 || data.Pin > 999999)
                {
                    _logger.Log($"Invalid Pin: {data.Pin}. Pin must be 6 digits.");
                    return GeneralHelper.ResponseModel(false, messagesEnum.GENERAL_ERROR, "Pin must be 6 digits");
                }
                var user = await _context.Users.FindAsync(data.UserId);
                if (user == null)
                {
                    _logger.Log($"Account not found for UserId: {data.UserId}");
                    return GeneralHelper.ResponseModel(false, messagesEnum.GENERAL_ERROR, "Account not found");
                }
                _logger.Log($"User found with UserId: {data.UserId}. Updating Pin.");
                user.PIN = data.Pin;
                user.ModifiedOn = DateTime.Now;
                user.ModifiedBy = user.Id;
                await _context.SaveChangesAsync();
                _logger.Log($"Pin created successfully for UserId: {data.UserId}");
                return GeneralHelper.ResponseModel(true, messagesEnum.GENERAL_SUCCESS, "Pin created successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return GeneralHelper.ResponseModel(false, messagesEnum.GENERAL_ERROR, ex.InnerException?.ToString() ?? ex.Message);
            }
        }

        public async Task<GeneralModel> Dashboard(int userId)
        {
            try
            {
                _logger.Log($"Dashboard method called! with UserId: {userId}");
                var response = await(from u in _context.Users.Where(x => x.Id == userId)
                                     select new UserModel
                                     {
                                         CustomerName = u.CustomerName,
                                         EmailAddress = u.EmailAddress,
                                         ICNumber = u.ICNumber,
                                         Id = u.Id,
                                         MobileNumber = u.MobileNumber
                                     }).FirstOrDefaultAsync();
                if (response != null)
                {
                    _logger.Log($"Data for Dashboard found");
                    return GeneralHelper.ResponseModel(response, messagesEnum.GENERAL_SUCCESS, "Success");
                }
                else
                {
                    _logger.Log($"Data for Dashboard not found");
                    return GeneralHelper.ResponseModel(false, messagesEnum.NOT_FOUND, "Data not found");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return GeneralHelper.ResponseModel(false, messagesEnum.GENERAL_ERROR, ex.InnerException?.ToString() ?? ex.Message);
            }
        }

        public async Task<GeneralModel> EnableBiometricVerification(int userId)
        {
            try
            {
                _logger.Log($"EnableBiometricVerification method called with UserId: {userId}");
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    _logger.Log($"User with UserId: {userId} not found.");
                    return GeneralHelper.ResponseModel(false, messagesEnum.GENERAL_ERROR, "Account not found");
                }

                _logger.Log($"User with UserId: {userId} found. Enabling biometric verification.");

                user.IsBiometricLoginEnabled = true;
                user.ModifiedOn = DateTime.Now;
                user.ModifiedBy = user.Id;
                await _context.SaveChangesAsync();
                _logger.Log($"Biometric verification enabled for UserId: {userId} successfully.");
                return GeneralHelper.ResponseModel(true, messagesEnum.GENERAL_SUCCESS, "Biometric enabled successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return GeneralHelper.ResponseModel(false, messagesEnum.GENERAL_ERROR, ex.InnerException?.ToString() ?? ex.Message);
            }
        }

        public async Task<GeneralModel> GetPrivacyPolicy()
        {
            try
            {
                _logger.Log($"GetPrivacyPolicy method called!");
                var policy = await _context.GlobalSettings.Where(u => u.Key == GlobalSettingsKeys.PrivacyPolicyKey).Select(x => x.Value).FirstOrDefaultAsync();
                _logger.Log($"Privacy policy found!");
                return GeneralHelper.ResponseModel(policy, messagesEnum.GENERAL_SUCCESS, "Success");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return GeneralHelper.ResponseModel(false, messagesEnum.GENERAL_ERROR, ex.InnerException?.ToString() ?? ex.Message);
            }
        }

        public async Task<GeneralModel> Login(string icNumber)
        {
            try
            {
                _logger.Log($"Login method called with ICNumber: {icNumber}");
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.ICNumber == icNumber);
                if (existingUser != null)
                {
                    _logger.Log($"User found with ICNumber: {icNumber}. Generating OTPs.");
                    existingUser.EmailOTP = GeneralHelper.GenerateOtp();
                    existingUser.MobileOTP = GeneralHelper.GenerateOtp();
                    existingUser.OTPExpiry = DateTime.Now.AddMinutes(10);
                    existingUser.ModifiedOn = DateTime.Now;
                    existingUser.ModifiedBy = existingUser.Id;
                    _context.Users.Update(existingUser);
                    await _context.SaveChangesAsync();
                    _logger.WriteOtpToFile(existingUser.CustomerName, existingUser.EmailAddress, existingUser.EmailOTP, existingUser.MobileNumber, existingUser.MobileOTP);
                    _logger.Log($"Account found and OTPs updated successfully for ICNumber: {icNumber}");
                    return GeneralHelper.ResponseModel(true, messagesEnum.GENERAL_SUCCESS, "Account found successfully.");
                }
                else
                {
                    _logger.Log($"No account found with ICNumber: {icNumber}");
                    return GeneralHelper.ResponseModel(false, messagesEnum.NOT_FOUND, "Account not found.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return GeneralHelper.ResponseModel(false, messagesEnum.GENERAL_ERROR, ex.InnerException?.ToString() ?? ex.Message);
            }
        }

        public async Task<GeneralModel> PrivacyPolicyAgreement(int userId)
        {
            try
            {
                _logger.Log($"PrivacyPolicyAgreement method called with UserId: {userId}");

                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    _logger.Log($"User with UserId: {userId} not found.");
                    return GeneralHelper.ResponseModel(false, messagesEnum.GENERAL_ERROR, "Account not found");
                }
                _logger.Log($"User with UserId: {userId} found. Updating privacy policy acceptance.");
                user.IsPrivacyPolicyAccepted = true;
                user.ModifiedOn = DateTime.Now;
                user.ModifiedBy = user.Id;
                await _context.SaveChangesAsync();
                _logger.Log($"Privacy policy acceptance updated for UserId: {userId} successfully.");
                return GeneralHelper.ResponseModel(true, messagesEnum.GENERAL_SUCCESS, "Privacy policy accepted");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return GeneralHelper.ResponseModel(false, messagesEnum.GENERAL_ERROR, ex.InnerException?.ToString() ?? ex.Message);
            }
        }

        public async Task<GeneralModel> UserRegistration(UserRegistrationModel data)
        {
            try
            {
                _logger.Log($"UserRegistration method called with ICNumber: {data.ICNumber}");
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.ICNumber == data.ICNumber);
                if (existingUser != null)
                {
                    existingUser.EmailOTP = GeneralHelper.GenerateOtp();
                    existingUser.MobileOTP = GeneralHelper.GenerateOtp();
                    existingUser.OTPExpiry = DateTime.Now.AddMinutes(10);
                    existingUser.ModifiedOn = DateTime.Now;
                    existingUser.ModifiedBy = existingUser.Id;

                    _logger.Log($"Updating existing user with ICNumber: {data.ICNumber}");
                    _logger.Log($"New OTPs - Email: {existingUser.EmailOTP}, Mobile: {existingUser.MobileOTP}");

                    _context.Users.Update(existingUser);
                    await _context.SaveChangesAsync();
                    _logger.WriteOtpToFile(existingUser.CustomerName, existingUser.EmailAddress, existingUser.EmailOTP, existingUser.MobileNumber, existingUser.MobileOTP);
                    _logger.Log($"User updated successfully");
                    return GeneralHelper.ResponseModel(false, messagesEnum.ACCOUNT_ALREADY_EXISTS, "Account already exist | There is an account registered with the IC number. Please login to continue.");
                }
                else
                {
                    int emailOtp = GeneralHelper.GenerateOtp();
                    int mobileOtp = GeneralHelper.GenerateOtp();
                    User user = new User
                    {
                        CustomerName = data.CustomerName,
                        EmailAddress = data.EmailAddress,
                        MobileNumber = data.MobileNumber,
                        ICNumber = data.ICNumber,
                        EmailOTP = emailOtp,
                        MobileOTP = mobileOtp,
                        OTPExpiry = DateTime.Now.AddMinutes(10),
                        IsBiometricLoginEnabled = false,
                        IsPrivacyPolicyAccepted = false,
                        IsActive = true,
                        IsMobileVerified = false,
                        IsEmailVerified = false,
                        CreatedOn = DateTime.Now,
                    };

                    _logger.Log($"Creating new user with ICNumber: {data.ICNumber}");
                    _logger.Log($"Generated OTPs - Email: {emailOtp}, Mobile: {mobileOtp}");

                    await _context.Users.AddAsync(user);
                    await _context.SaveChangesAsync();
                    _logger.WriteOtpToFile(user.CustomerName, user.EmailAddress, user.EmailOTP, user.MobileNumber, user.MobileOTP);
                    var _data = new { Status = true, UserId = user.Id, };
                    _logger.Log($"User registered successfully");
                    return GeneralHelper.ResponseModel(_data, messagesEnum.GENERAL_SUCCESS, "Account registered successfully.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return GeneralHelper.ResponseModel(false, messagesEnum.GENERAL_ERROR, ex.InnerException?.ToString() ?? ex.Message);
            }
        }

        public async Task<GeneralModel> OTPVerification(UserOTPVerifyModel data)
        {
            try
            {
                _logger.Log($"OTPVerification method called with OtpType: {data.OtpType}, OtpTypeValue: {data.OtpTypeValue}");
                User? user = null;
                if (data.OtpType == OtpType.Mobile)
                {
                    user = await _context.Users.FirstOrDefaultAsync(u => u.MobileNumber == data.OtpTypeValue && u.MobileOTP == data.Otp && u.Id == data.UserId);
                    _logger.Log($"Attempting to verify mobile OTP for MobileNumber: {data.OtpTypeValue} & UserId : {data.UserId}");
                }
                else if (data.OtpType == OtpType.Email)
                {
                    user = await _context.Users.FirstOrDefaultAsync(u => u.EmailAddress == data.OtpTypeValue && u.EmailOTP == data.Otp && u.Id == data.UserId);
                    _logger.Log($"Attempting to verify email OTP for EmailAddress: {data.OtpTypeValue} & UserId : {data.UserId}");
                }

                if (user != null && user.OTPExpiry > DateTime.Now)
                {
                    _logger.Log($"User found with OtpTypeValue: {data.OtpTypeValue}");

                    user.IsEmailVerified = data.OtpType == OtpType.Email ? true : user.IsEmailVerified;
                    user.IsMobileVerified = data.OtpType == OtpType.Mobile ? true : user.IsMobileVerified;
                    user.ModifiedOn = DateTime.Now;
                    user.ModifiedBy = user.Id;
                    await _context.SaveChangesAsync();
                    _logger.Log($"User updated successfully");
                    return GeneralHelper.ResponseModel(true, messagesEnum.GENERAL_SUCCESS, "OTP verified");
                }
                _logger.Log($"OTP didn't matched");
                return GeneralHelper.ResponseModel(false, messagesEnum.INCORRECT_OTP, "Incorrect OTP | Please enter your OTP again");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return GeneralHelper.ResponseModel(false, messagesEnum.GENERAL_ERROR, ex.InnerException?.ToString() ?? ex.Message);
            }
        }

        public async Task<GeneralModel> PinVerification(UserPinRequestModel data)
        {
            try
            {
                _logger.Log($"PinVerification method called with UserId: {data.UserId} and Pin: {data.Pin}");
                var user = await _context.Users.FindAsync(data.UserId);
                if (user == null)
                {
                    _logger.Log($"Account not found for UserId: {data.UserId}");
                    return GeneralHelper.ResponseModel(false, messagesEnum.GENERAL_ERROR, "Account not found");
                }

                if (user.PIN == data.Pin)
                {
                    _logger.Log($"PIN verified successfully for UserId: {data.UserId}");
                    return GeneralHelper.ResponseModel(true, messagesEnum.GENERAL_SUCCESS, "PIN verified successfully");
                }
                else
                {
                    _logger.Log($"Unmatched PIN entered for UserId: {data.UserId}. Expected: {user.PIN}, Provided: {data.Pin}");
                    return GeneralHelper.ResponseModel(false, messagesEnum.UNMATCHED_PIN, "Unmatched PIN | Please enter your PIN again");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return GeneralHelper.ResponseModel(false, messagesEnum.GENERAL_ERROR, ex.InnerException?.ToString() ?? ex.Message);
            }
        }
        #endregion


    }
}
