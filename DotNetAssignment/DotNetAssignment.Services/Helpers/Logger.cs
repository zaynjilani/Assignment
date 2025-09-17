using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;


namespace DotNetAssignment.Services.Helpers
{
    public class Logger
    {
        #region Fields
        private readonly string _logFilePath;
        private readonly string _logDirectory;
        private readonly string _otpFilePath;
        #endregion

        #region Constructors
        public Logger(IConfiguration configuration)
        {
            _logFilePath = configuration.GetValue<string>("Logging:LogFilePath");
            _otpFilePath = configuration.GetValue<string>("Logging:OTPFilePath");
            _logDirectory = Path.GetDirectoryName(_logFilePath);
            if (!Directory.Exists(_logDirectory))
            {
                Directory.CreateDirectory(_logDirectory);
            }
        }
        #endregion

        #region Methods
        public void Log(string message)
        {
            var logMessage = $"{DateTime.Now}: {message}";
            File.AppendAllText(_logFilePath, logMessage + Environment.NewLine);
        }
        public void LogError(Exception ex)
        {
            var logMessage = $"{DateTime.Now}: ERROR - {ex.Message}\n{ex.StackTrace}";
            File.AppendAllText(_logFilePath, logMessage + Environment.NewLine);
        }
        public void WriteOtpToFile(string customerName, string emailAddress, int? emailOtp, string mobileNumber, int? mobileOtp)
        {
            string content = $"CustomerName: {customerName}, EmailAddress: {emailAddress}, EmailOTP: {emailOtp}, MobileNumber: {mobileNumber}, MobileOTP: {mobileOtp}\n";
            File.AppendAllText(_otpFilePath, content);
        }
        #endregion
    }
}
