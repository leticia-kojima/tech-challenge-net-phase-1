using System.Text.RegularExpressions;
using FCG.Domain._Common;

namespace FCG.Domain.ValueObjects;

public class Email
{
    private readonly string _email;

    public Email(string email)
    {
        if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            throw new FCGValidationException(
                nameof(email),
                $"{nameof(email)} is not valid."
            );

        _email = email;
    }

    public override string ToString() => _email;

}