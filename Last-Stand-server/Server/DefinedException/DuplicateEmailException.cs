namespace Server.DefinedException;

public class DuplicateEmailException : Exception
{
    public DuplicateEmailException() : base("Email already exists.")
    {
    }

    public DuplicateEmailException(string message) : base(message)
    {
    }
}