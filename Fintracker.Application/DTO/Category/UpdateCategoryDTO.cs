using Fintracker.Application.DTO.Common;

namespace Fintracker.Application.DTO.Category;

public class UpdateCategoryDTO : IBaseDto
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public string Image { get; set; }

    public string IconColour { get; set; }
}