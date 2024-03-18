using Fintracker.Domain.Common;
using Fintracker.Domain.Enums;

namespace Fintracker.Domain.Entities;

public class Category : IEntity<Guid>
{
    public Guid Id { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public string CreatedBy { get; set; } = default!;
    
    public DateTime ModifiedAt { get; set; }
    
    public string ModifiedBy { get; set; } = default!;

    public string Name { get; set; } = default!;

    public CategoryType Type { get; set; }

    public string Image { get; set; } = default!;

    public string IconColour { get; set; } = default!;

    public Guid UserId { get; set; }

    public bool IsSystemCategory { get; set; }
}