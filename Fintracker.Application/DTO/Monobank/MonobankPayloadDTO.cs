namespace Fintracker.Application.DTO.Monobank;

public class MonobankPayloadDTO
{
    public ICollection<MonoTransactionDTO> Transactions { get; set; } = default!;

    public Guid OwnerId { get; set; }

    public string Email { get; set; } = default!;

    public string AccountId { get; set; } = default!;
}