namespace Server.DefinedException;

public class DuplicatePlayerIdException : Exception
{
    public DuplicatePlayerIdException() : base("PlayerId already exists.")
    {
    }

    public DuplicatePlayerIdException(string message) : base(message)
    {
    }
}