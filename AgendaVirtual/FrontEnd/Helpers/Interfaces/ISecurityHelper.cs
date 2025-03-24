using FrontEnd.APIModels;

namespace FrontEnd.Helpers.Interfaces
{
    public interface ISecurityHelper
    {
        LoginAPI Login(string username, string password);
    }
}
