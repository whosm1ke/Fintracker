
namespace Fintracker.Application.DTO.Category;

public class CreateCategoryDTO : ICategoryDto
{
    public CategoryTypeEnum Type { get; set; }

    public string Name { get; set; }

    public string Image { get; set; }

    public string IconColour { get; set; }
}