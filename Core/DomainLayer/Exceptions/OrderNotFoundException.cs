using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Exceptions
{
    public class OrderNotFoundException(Guid id) : NotFoundException($"There is no order with id:{id}")
    {
    }
}
