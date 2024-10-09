using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AuthenticationService.Model
{
    public class Register
    {
        public string email { get; set; }
        public string name { get; set; }
        public string password { get; set; }      
        public string confirmPassword { get; set; }     
        public DateTime dateOfBirth { get; set; }     
        public string gender { get; set; }

    }
}
