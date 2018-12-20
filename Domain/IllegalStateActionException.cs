using System;

namespace Domain
{
    internal class IllegalStateActionException : ApplicationException
    {
        public IllegalStateActionException(Type type) : base($"Can't do this action when in state: '{type.Name}'")
        {
        }
    }
}