namespace TaskAPI
{
    public interface IJwtAuthenticationManager
    {
        string Authenticate(string AuthorID, string Name, string LastName, string UserName);

    }
}