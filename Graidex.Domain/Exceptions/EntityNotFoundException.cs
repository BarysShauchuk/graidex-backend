using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Domain.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException()
            : base("Requested entity wasn't not found.")
        {
        }

        public EntityNotFoundException(string message) 
            : base(message)
        {
        }

        public EntityNotFoundException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }

        public EntityNotFoundException(string message, Type entityType)
            : base(message)
        {
            EntityType = entityType;
        }

        public EntityNotFoundException(string message, Type entityType, Exception innerException)
            : base(message, innerException)
        {
            EntityType = entityType;
        }

        public Type? EntityType { get; }

        public override string Message
        {
            get
            {
                if (EntityType is null)
                {
                    return base.Message;
                }

                return $"{base.Message} Entity type: {EntityType.Name}.";
            }
        }
    }
}
