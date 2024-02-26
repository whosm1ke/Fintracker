using Fintracker.Domain.Common;
using Fintracker.Domain.Enums;

namespace Fintracker.Domain.Entities;

public class Category : IEntity<Guid>
{
    public Guid Id { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public string CreatedBy { get; set; }
    
    public DateTime ModifiedAt { get; set; }
    
    public string ModifiedBy { get; set; }

    public string Name { get; set; }

    public CategoryType Type { get; set; }

    public string Image { get; set; }

    public string IconColour { get; set; }
}