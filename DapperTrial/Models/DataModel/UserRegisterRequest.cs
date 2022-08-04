using System.ComponentModel.DataAnnotations;

namespace DapperTrial.Models.DataModel
{
    public class UserRegisterRequest
    {

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Username { get; set; }

        [Required, MinLength(8)]
        public string Password { get; set; }
        [Required, Compare("Password")]
        public string ConfirmPassword { get; set; }
        [Required]
        public string Address { get; set; }
        [Required, StringLength(10)]
        public string Phonenumber { get; set; }
    }
}
