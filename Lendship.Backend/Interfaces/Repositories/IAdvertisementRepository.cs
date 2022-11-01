using Lendship.Backend.Models;
using System.Collections.Generic;

namespace Lendship.Backend.Interfaces.Repositories
{
    public interface IAdvertisementRepository
    {
        IEnumerable<Advertisement> GetAll(string signedInUserId);

        Advertisement GetById(int? id, string signedInUserId);

        void Create(Advertisement ad);

        void Update(Advertisement ad);

        void Delete(int id);
    }
}