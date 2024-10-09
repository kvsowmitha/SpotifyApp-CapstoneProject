namespace AuthenticationService.Exceptions
{
    public class PasswordMismatchException : Exception
    {
        public PasswordMismatchException(string message) : base(message) { }
    }
}
