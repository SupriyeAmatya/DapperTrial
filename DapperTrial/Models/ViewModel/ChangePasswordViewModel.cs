using System.ComponentModel.DataAnnotations;

namespace DapperTrial.Models.ViewModel
{
    public class ChangePasswordViewModel
    {
        
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Minimum 6 characters required")]
        public string NewPassword { get; set; }

        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Confirm password and password do not match")]
        public string ConfirmPassword { get; set; }
    }
}
