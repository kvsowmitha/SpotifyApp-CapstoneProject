using UserRegister.Models;

namespace UserRegister.Services
{
    public interface IUserService
    {
        string RegisterUser(Register user);
        Register GetUserByEmail(string email);
        string ValidateUser(string email, string password, string confirmPassword);
        string UpdateUserDetails(Register user);
        string DeleteUser(string email);
        
    }
}
