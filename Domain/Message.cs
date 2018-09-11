using System;
using System.Threading;

namespace Domain
{
    public class Message
    {
        public Message()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return Id.Equals(((Message)obj).Id);
        }


        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}