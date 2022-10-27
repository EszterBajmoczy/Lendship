using Lendship.Backend.DTO;
using Lendship.Backend.Interfaces.Repositories;
using Lendship.Backend.Models;
using System.Collections.Generic;
using System.Linq;

namespace Lendship.Backend.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly LendshipDbContext _dbContext;

        public CategoryRepository(LendshipDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Category GetByName(string name)
        {
            return _dbContext.Categories
                .Where(x => x.Name.ToLower() == name.ToLower())
                .FirstOrDefault();
        }

        public void Create(Category category)
        {
            _dbContext.Categories.Add(category);
            _dbContext.SaveChanges();
        }

        public IEnumerable<Category> GetAll()
        {
            return _dbContext.Categories;
        }
    }
}
