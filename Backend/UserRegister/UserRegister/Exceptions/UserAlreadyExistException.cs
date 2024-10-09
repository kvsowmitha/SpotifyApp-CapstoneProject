using System;

namespace Exceptions
{
    public class UserAlreadyExistException : ApplicationException
    {
        public UserAlreadyExistException() { }
        public UserAlreadyExistException(string message) : base(message) { }
    }
}
