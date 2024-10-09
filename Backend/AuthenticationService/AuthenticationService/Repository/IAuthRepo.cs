using AuthenticationService.Model;

namespace AuthenticationService.Repository
{
    public interface IAuthRepo
    {
        bool AddUserData(LoginData data);
        Task<string?> GenerateOtpForPasswordReset(string email);
        Task<bool> ValidateOtp(string email, string otp);
        Task<bool> UpdatePassword(string email, string newPassword);
        Task<LoginData?> GetUserByEmail(string email);
    }
}
