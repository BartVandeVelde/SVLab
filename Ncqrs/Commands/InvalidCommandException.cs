using System;

namespace Ncqrs.Commands
{
    public class InvalidCommandException : Exception
    {
        public InvalidCommandException(string message) : base(message)
        {
            
        }
    }
}