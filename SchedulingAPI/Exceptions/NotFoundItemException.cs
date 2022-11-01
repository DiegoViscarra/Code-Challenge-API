using System;

namespace SchedulingAPI.Exceptions
{
    public class NotFoundItemException : Exception
    {
        public NotFoundItemException(string message) : base(message)
        {

        }
    }
}
