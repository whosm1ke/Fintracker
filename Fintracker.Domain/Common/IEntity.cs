namespace Fintracker.Domain.Common;

public interface IEntity<Tid>
{
    public Tid Id { get; set; }

    public DateTime CreatedAt { get; set; }
    
    public string CreatedBy { get; set; }
    
    public DateTime ModifiedAt { get; set; }

    public string ModifiedBy { get; set; }
}