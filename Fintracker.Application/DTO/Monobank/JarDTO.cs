namespace Fintracker.Application.DTO.Monobank;

public class JarDTO : IAccountBaseDto
{
    public string Title { get; set; } = default!;

    public string Description { get; set; } = default!;

    public string Id { get; set; } = default!;
    
    public long Balance { get; set; }
    
    public long Goal { get; set; }
}