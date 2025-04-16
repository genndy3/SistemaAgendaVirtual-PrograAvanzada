using FrontEnd.APIModels;

namespace FrontEnd.Helpers.Interfaces
{
    public interface ISecurityHelper
    {
        LoginAPI Login(string username, string password);
        RegisterAPI Register(string username, string email, string password);
    }
}
