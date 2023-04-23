using System;

namespace Base.Domain
{
    public class DomainException : Exception
    {
        public DomainException()
        { }

        public DomainException(string message)
            : base(message)
        { }

        public string Code => GetType().Name;
    }
}
