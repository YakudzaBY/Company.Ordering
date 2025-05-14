using Company.Ordering.Domain.OrderAggregate;
using System.Runtime.Serialization;

namespace Company.Ordering.API.Model
{
    public class OrderWithProducts : Order
    {
        [IgnoreDataMember]
        public override Guid Guid { get => base.Guid; set => base.Guid = value; }
    }
}
