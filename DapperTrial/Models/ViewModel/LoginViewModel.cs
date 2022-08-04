using System.ComponentModel.DataAnnotations;

namespace DapperTrial.Models.ViewModel
{
    public class LoginViewModel
    {
        public class LoginView
        {
            [Display(Name = "User Name")]
            [Required(ErrorMessage = "Please, provide the User Name.")]
            public String UserName { get; set; }


            [Required(ErrorMessage = "Please, provide the Password.")]
            public String Password { get; set; }
        }
    }
}
