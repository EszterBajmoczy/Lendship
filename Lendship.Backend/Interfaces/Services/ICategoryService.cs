using Lendship.Backend.DTO;
using Lendship.Backend.Models;
using System.Collections.Generic;

namespace Lendship.Backend.Interfaces.Services
{
    public interface ICategoryService
    {
        List<CategoryDto> GetCategories();

        Category AddCategory(string category);
    }
}
