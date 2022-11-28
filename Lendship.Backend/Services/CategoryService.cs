using Lendship.Backend.DTO;
using Lendship.Backend.Interfaces.Converters;
using Lendship.Backend.Interfaces.Repositories;
using Lendship.Backend.Interfaces.Services;
using Lendship.Backend.Models;
using System.Collections.Generic;
using System.Linq;

namespace Lendship.Backend.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICategoryConverter _categoryConverter;

        public CategoryService(ICategoryRepository categoryRepository, ICategoryConverter categoryConverter)
        {
            _categoryRepository = categoryRepository;
            _categoryConverter = categoryConverter;
        }

        public Category AddCategory(string category)
        {
            var newCategory = new Category() { Name = category };
            _categoryRepository.Create(newCategory);
            return newCategory;
        }

        public List<CategoryDto> GetCategories()
        {
            var categories = _categoryRepository.GetAll();

            return categories.Select(c => _categoryConverter.ConvertToDto(c)).ToList();
        }

        public Category GetOrCreateCategoryByName(string name)
        {
            var category = _categoryRepository.GetByName(name);

            if (category == null)
            {
                category = AddCategory(name.ToLower());
            }

            return category;
        }
    }
}
