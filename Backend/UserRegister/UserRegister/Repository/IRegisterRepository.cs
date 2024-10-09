using UserRegister.Models;

namespace UserRegister.Repository
{
    public interface IRegisterRepository
    {
        string RegisterUser(Register user);
        Register GetUserByEmail(string email);
        string ValidateUser(string email, string password, string confirmPassword);
        string UpdateUserDetails(Register user);
        string DeleteUser(string email);
        string UpdateUserPassword(string email, string password);
    }
}
