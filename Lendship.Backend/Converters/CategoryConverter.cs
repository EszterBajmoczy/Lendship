using Lendship.Backend.DTO;
using Lendship.Backend.Interfaces.Converters;
using Lendship.Backend.Models;

namespace Lendship.Backend.Converters
{
    public class CategoryConverter : ICategoryConverter
    {
        public CategoryDto ConvertToDto(Category category)
        {
            return new CategoryDto()
            {
                Id = category.Id,
                Name = category.Name
            };
        }
    }
}
