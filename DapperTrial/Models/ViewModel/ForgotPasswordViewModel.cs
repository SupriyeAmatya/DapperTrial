using System.ComponentModel.DataAnnotations;

namespace DapperTrial.Models.ViewModel
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
       
    }

}
