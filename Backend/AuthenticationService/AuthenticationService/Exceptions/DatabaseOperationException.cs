namespace AuthenticationService.Exceptions
{
    public class DatabaseOperationException : Exception
    {
        public DatabaseOperationException(string message) : base(message)
        {
        }
    }
}
