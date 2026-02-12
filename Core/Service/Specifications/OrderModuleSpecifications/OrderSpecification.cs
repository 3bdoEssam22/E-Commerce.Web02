using DomainLayer.Models.OrderModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications.OrderModuleSpecifications
{
    internal class OrderSpecification : BaseSpecifications<Order, Guid>
    {
        public OrderSpecification(string email) : base(S => S.UserEmail == email)
        {
            AddInclude(O => O.Items);
            AddInclude(O => O.DeliveryMethod);
            AddOrderByDesc(O => O.OrderDate);
        }
        public OrderSpecification(Guid id) : base(S => S.Id == id)
        {
            AddInclude(O => O.Items);
            AddInclude(O => O.DeliveryMethod);
        }
    }
}
