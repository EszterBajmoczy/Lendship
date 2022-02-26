using Lendship.Backend.DTO.Authentication;
using System.Threading.Tasks;

namespace Lendship.Backend.Interfaces.Services
{
    public interface IEmailService
    {
        void SendEmailAsync(string username, string email, string url);
    }
}
