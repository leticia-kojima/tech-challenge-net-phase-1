using FCG.Domain._Common.Abstract;
using FCG.Domain._Common.Exceptions;
using System.Text.RegularExpressions;

namespace FCG.Domain.ValueObjects;

public class Email : ValueObjectBase<Email>
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

    public static implicit operator string(Email email) => email._email;

    public static implicit operator Email(string email) => new (email);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return _email;
    }
}