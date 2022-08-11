using DapperTrial.Models.DataModel;
using DapperTrial.Models.ViewModel;

namespace DapperTrial.Services.LoginServices
{
    public interface ILoginService
    {

        Users Login(string email, string password);
        public GenericResponseModel FGPASS(ForgotPasswordViewModel model, string url);

        public int changePassword(string newpassword, string oldpassword, string token);
        //Task<String> ForgotPassword(ForgotPasswordViewModel model);
    }
}
