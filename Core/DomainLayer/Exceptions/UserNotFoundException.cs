namespace DomainLayer.Exceptions
{
    public sealed class UserNotFoundException(string email) : NotFoundException($"Could Not Find {email}.")
    {
    }
}
