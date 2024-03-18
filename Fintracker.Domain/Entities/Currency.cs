using Fintracker.Domain.Common;

namespace Fintracker.Domain.Entities;

public class Currency : IEntity<Guid>
{
    public Guid Id { get; set; }
    
    public DateTime CreatedAt { get; set; }

    public string CreatedBy { get; set; } = default!;
    
    public DateTime ModifiedAt { get; set; }
    
    public string ModifiedBy { get; set; } = default!;

    public string Name { get; set; } = default!;
    
    public string Symbol { get; set; } = default!;

    public int Code { get; set; }
}