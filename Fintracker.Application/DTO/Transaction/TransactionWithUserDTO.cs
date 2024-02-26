using Fintracker.Application.DTO.User;

namespace Fintracker.Application.DTO.Transaction;

public class TransactionWithUserDTO : TransactionBaseDTO
{
    public UserBaseDTO User { get; set; }
}