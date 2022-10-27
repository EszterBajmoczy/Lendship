using Lendship.Backend.Models;
using System.Collections.Generic;

namespace Lendship.Backend.Interfaces.Repositories
{
    public interface IAdvertisementRepository
    {
        IEnumerable<Advertisement> GetAll();

        Advertisement GetById(int? id);

        void Create(Advertisement ad);

        void Update(Advertisement ad);

        void Delete(int id);
    }
}