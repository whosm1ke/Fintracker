namespace Fintracker.Application.DTO.Monobank;

public class AccountDTO : IAccountBaseDto
{
    public string Id { get; set; }  = default!;
    
    public long Balance { get; set; }

    public string[] MaskedPan { get; set; } = default!;

    public string Iban { get; set; } = default!;
}