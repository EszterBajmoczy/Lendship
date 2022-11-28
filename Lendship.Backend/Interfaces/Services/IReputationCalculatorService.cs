using Lendship.Backend.Authentication;

namespace Lendship.Backend.Interfaces.Services
{
    public interface IReputationCalculatorService
    {
        void RecalculateAdvertiserReputationForUser(ApplicationUser user);

        void RecalculateLenderReputationForUser(ApplicationUser user);
    }
}
