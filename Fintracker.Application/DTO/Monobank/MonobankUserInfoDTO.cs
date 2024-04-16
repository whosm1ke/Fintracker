namespace Fintracker.Application.DTO.Monobank;

public class MonobankUserInfoDTO
{
    public MonobankUserInfoDTO()
    {
        Accounts = new HashSet<AccountDTO>();
    }
    
    public string Name { get; set; } = default!;

    public ICollection<AccountDTO> Accounts { get; set; }
    
}