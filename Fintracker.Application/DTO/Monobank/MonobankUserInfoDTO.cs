namespace Fintracker.Application.DTO.Monobank;

public class MonobankUserInfoDTO
{
    public MonobankUserInfoDTO()
    {
        Accounts = new HashSet<AccountDTO>();
        Jars = new HashSet<JarDTO>();
    }
    
    public string Name { get; set; } = default!;

    public ICollection<AccountDTO> Accounts { get; set; }
    
    public ICollection<JarDTO> Jars { get; set; }
}