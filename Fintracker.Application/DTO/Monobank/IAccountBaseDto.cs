namespace Fintracker.Application.DTO.Monobank;

public interface IAccountBaseDto
{
    public string Id { get; set; }
    
    public long Balance { get; set; }
}