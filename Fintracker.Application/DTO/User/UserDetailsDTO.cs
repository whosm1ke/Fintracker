namespace Fintracker.Application.DTO.User;

public class UserDetailsDTO
{
    public string? Sex { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public string? Avatar { get; set; }

    public LanguageTypeEnum? Language { get; set; }
    
}