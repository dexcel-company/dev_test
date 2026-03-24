namespace CelloPark.Domain.Features.Users.Constants;

public static class UserSettings
{
    public const int FirstNameMinLength = 2;
    public const int FirstNameMaxLength = 20;

    public const int LastNameMinLength = 2;
    public const int LastNameMaxLength = 20;

    public const int EmailMinLength = 8;
    public const int EmailMaxLength = 40;

    public const int PhoneNumberMinLength = 4;
    public const int PhoneNumberMaxLength = 20;

    public const int JobTitleMinLength = 4;
    public const int JobTitleMaxLength = 24;

    public const int PasswordMinLength = 4;
    public const int PasswordMaxLength = 64;
}
