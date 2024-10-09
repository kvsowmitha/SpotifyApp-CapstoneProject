using AuthenticationService.Model;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;
using AuthenticationService.Exceptions;

namespace AuthenticationService.Repository
{
    public class AuthRepo : IAuthRepo
    {
        private readonly AuthDbContext _context;
        private static readonly Dictionary<string, (string otp, DateTime expiry)> _otpStore = new();

        public AuthRepo(AuthDbContext context)
        {
            _context = context;
        }

        public bool AddUserData(LoginData data)
        {
            try
            {
                _context.LoginData.Add(data);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new DatabaseOperationException("Error saving user data to the database: " + ex.Message);
            }
        }
        public async Task<string?> GenerateOtpForPasswordReset(string email)
        {
            var user = await _context.LoginData.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                return null;

            string otp = new Random().Next(100000, 999999).ToString();
            _otpStore[email] = (otp, DateTime.UtcNow.AddMinutes(5)); // Set OTP expiry to 5 minutes
            Console.WriteLine($"Generated OTP for {email}: {otp}");
            
            SendEmail(email, "Your OTP for password reset", $"Your OTP is: {otp}");
            return otp;
        }

        public async Task<bool> ValidateOtp(string email, string otp)
        {

            if (_otpStore.TryGetValue(email, out var otpData))
            {
                Console.WriteLine($"Validating OTP for {email}: Expected {otpData.otp}, Received {otp}");
                if (otpData.otp == otp && otpData.expiry > DateTime.UtcNow)
                {
                    _otpStore.Remove(email); // Remove OTP after validation
                    return true;
                }
            }
            return false;
        }
        public async Task<bool> UpdatePassword(string email, string newPassword)
        {
            var user = await _context.LoginData.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                return false;

            user.Password = newPassword; 
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<LoginData?> GetUserByEmail(string email)
        {
            var user = await _context.LoginData.FirstOrDefaultAsync(x => x.Email == email);
            if (user == null)
            {
                throw new UserNotFoundException($"User with email {email} not found.");
            }
            return user;
        }
        private void SendEmail(string to, string subject, string body)
        {
            using (var client = new SmtpClient("smtp.gmail.com"))
            {
                client.Port = 587; 
                client.Credentials = new NetworkCredential("kvsowmitha@gmail.com", "bwiyescxjujaadox");
                client.EnableSsl = true;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("kvsowmitha@gmail.com"),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                };

                mailMessage.To.Add(to);
                client.Send(mailMessage);
            }
        }
    }
}
