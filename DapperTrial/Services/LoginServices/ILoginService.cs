using DapperTrial.Models.DataModel;
using DapperTrial.Models.ViewModel;

namespace DapperTrial.Services.LoginServices
{
    public interface ILoginService
    {

        Users Login(string email, string password);
        //Task<String> ForgotPassword(ForgotPasswordViewModel model);
    }
}
