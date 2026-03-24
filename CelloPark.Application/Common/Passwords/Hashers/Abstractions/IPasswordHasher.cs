namespace CelloPark.Application.Common.Passwords.Hashers.Abstractions;

public interface IPasswordHasher
{
    string HashPassword(string password);
    bool VerifyHashedPassword(string providedPassword, string hashedPassword);
}
