namespace Fintracker.Application.DTO.Category;

public class DeleteCategoryDTO
{

    public Guid CategoryToReplaceId { get; set; }

    public bool ShouldReplace { get; set; }
}