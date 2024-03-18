
namespace Fintracker.Application.DTO.Category;

public class CreateCategoryDTO : ICategoryDto
{
    public CategoryTypeEnum Type { get; set; }

    public string Name { get; set; } = default!;

    public string Image { get; set; } = default!;

    public string IconColour { get; set; } = default!;
    
}