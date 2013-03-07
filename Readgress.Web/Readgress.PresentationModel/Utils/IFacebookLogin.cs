using Readgress.Models;

namespace Readgress.PresentationModel.Utils
{
    public interface IFacebookLogin
    {
        Reader CreateFBReader(string accessToken);

        Reader FindFBReader(string accessToken);

        bool Login(string accessToken);
    }
}
