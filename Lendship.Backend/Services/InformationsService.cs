using Lendship.Backend.DTO;
using Lendship.Backend.Interfaces.Services;
using Microsoft.Extensions.Configuration;

namespace Lendship.Backend.Services
{
    public class InformationsService : IInformationsService
    {
        private readonly IConfiguration _configuration;

        public InformationsService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public InformationsDto GetInformations()
        {
            var section = _configuration.GetSection("Informations");

            return new InformationsDto()
            {
                CompanyName = section.GetValue("Company name", ""),
                Address = section.GetValue("Adress", ""),
                Phone = section.GetValue("Phone", ""),
                Email = section.GetValue("Email", ""),
                Description = section.GetValue("Description", ""),
            };
        }
    }
}
