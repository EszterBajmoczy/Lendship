using Lendship.Backend.DTO;
using Lendship.Backend.Models;
using System.Collections.Generic;

namespace Lendship.Backend.Interfaces.Repositories
{
    public interface ICategoryRepository
    {
        Category GetByName(string name);

        void Create(Category category);

        IEnumerable<Category> GetAll();
    }
}