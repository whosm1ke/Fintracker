namespace Fintracker.Application.DTO.User;

public class UserDetailsDTO
{
    public string? FName { get; set; }
    
    public string? LName { get; set; }

    public string? Sex { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public string? Avatar { get; set; }

    public LanguageTypeEnum? Language { get; set; }
}