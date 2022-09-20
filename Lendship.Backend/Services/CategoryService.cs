using Lendship.Backend.Converters;
using Lendship.Backend.DTO;
using Lendship.Backend.Interfaces.Converters;
using Lendship.Backend.Interfaces.Services;
using Lendship.Backend.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;

namespace Lendship.Backend.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly LendshipDbContext _dbContext;

        private readonly ICategoryConverter _categoryConverter;

        public CategoryService(IHttpContextAccessor httpContextAccessor, LendshipDbContext dbContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;

            _categoryConverter = new CategoryConverter();
        }

        public Category AddCategory(string category)
        {
            var newCategory = new Category() { Name = category };
            _dbContext.Categories.Add(newCategory);
            _dbContext.SaveChanges();
            return newCategory;
        }

        public List<CategoryDto> GetCategories()
        {
            var categories = _dbContext.Categories.ToList();

            return categories.Select(c => _categoryConverter.ConvertToDto(c)).ToList();
        }

    }
}
