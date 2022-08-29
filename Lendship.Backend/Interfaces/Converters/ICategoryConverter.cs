using Lendship.Backend.DTO;
using Lendship.Backend.Models;

namespace Lendship.Backend.Interfaces.Converters
{
    public interface ICategoryConverter
    {
        CategoryDto ConvertToDto(Category category);

    }
}
