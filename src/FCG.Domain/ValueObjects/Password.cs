using System.Text.RegularExpressions;

namespace FCG.Domain.ValueObjects;

public class Password
{
    public string Hash { get; private set; }

	public Password(string password)
	{
        if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
            throw new FCGValidationException(
                nameof(password),
                $"Password must be at least 8 characters long."
             );

        if (!Regex.IsMatch(password, @"^(?=.*[a-zA-Z])(?=.*\d)(?=.*[@$!%*?&]).{8,}$"))
            throw new FCGValidationException(
                nameof(password),
                $"Password must contain letters, numbers and special characters."
             );

        Hash = BCrypt.Net.BCrypt.HashPassword(password);

    }
}
