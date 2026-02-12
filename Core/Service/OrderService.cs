using AutoMapper;
using DomainLayer.Contracts;
using DomainLayer.Exceptions;
using DomainLayer.Models.OrderModule;
using DomainLayer.Models.ProductModule;
using Service.Specifications;
using Service.Specifications.OrderModuleSpecifications;
using ServiceAbstraction;
using Shared.DataTtansferObjects.IdentityDTOs;
using Shared.DataTtansferObjects.OrderDTOs;

namespace Service
{
    public class OrderService(IMapper _mapper, IBasketRepository _basketRepository, IUnitOfWork _unitOfWork) : IOrderService
    {
        public async Task<OrderToReturnDto> CreateOrderAsync(OrderDto orderDto, string Email)
        {
            //Map Address to Order Address
            var OrderAddress = _mapper.Map<AddressDto, OrderAddress>(orderDto.Address);

            //Get Basket
            var Basket = await _basketRepository.GetBasketByIdAsync(orderDto.BasketId) ?? throw new BasketNotFoundException(orderDto.BasketId);
            ArgumentNullException.ThrowIfNullOrEmpty(Basket.PaymentIntentId);

            var orderRepo = _unitOfWork.GetRepository<Order, Guid>();
            var orderSpec = new OrderWithPaymentIntentSpecifications(Basket.PaymentIntentId);
            var existingOrder = await orderRepo.GetByIdAsync(orderSpec);
            if (existingOrder is not null)
                orderRepo.Remove(existingOrder);
            //Create Order Item List
            List<OrderItem> orderItems = [];
            var ProductRepo = _unitOfWork.GetRepository<Product, int>();
            foreach (var item in Basket.Items)
            {
                var Product = await ProductRepo.GetByIdAsync(item.Id) ?? throw new ProductNotFoundException(item.Id);
                orderItems.Add(CreateOrderItem(item, Product));
            }


            //Get Delivery Method
            var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetByIdAsync(orderDto.DeliveryMethodId) ?? throw new DeliveryMethodNotFoundException(orderDto.DeliveryMethodId);

            //Calculate Sub total
            var subTotal = orderItems.Sum(I => I.Quantity * I.Price);

            var order = new Order(Email, OrderAddress, deliveryMethod, orderItems, subTotal, Basket.PaymentIntentId);

            await orderRepo.AddAsync(order);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<Order, OrderToReturnDto>(order);
        }

        private static OrderItem CreateOrderItem(DomainLayer.Models.BasketModule.BasketItem item, Product Product)
        {
            return new OrderItem()
            {
                Product = new ProductItemOrdered() { ProductName = Product.Name, PictureUrl = Product.PictureUrl, ProductId = Product.Id },
                Price = Product.Price,
                Quantity = item.Quantity

            };
        }

        public async Task<IEnumerable<DeliveryMethodDto>> GetAllDeliveryMethodsAsync()
        {
            var deliveryMethods = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetAllAsync();
            return _mapper.Map<IEnumerable<DeliveryMethod>, IEnumerable<DeliveryMethodDto>>(deliveryMethods);
        }

        public async Task<IEnumerable<OrderToReturnDto>> GetAllOrdersAsync(string email)
        {
            var orders = await _unitOfWork.GetRepository<Order, Guid>().GetAllAsync(new OrderSpecification(email));
            return _mapper.Map<IEnumerable<Order>, IEnumerable<OrderToReturnDto>>(orders);
        }

        public async Task<OrderToReturnDto> GetOrderByIdAsync(Guid id)
        {
            var order = await _unitOfWork.GetRepository<Order, Guid>().GetByIdAsync(new OrderSpecification(id)) ?? throw new OrderNotFoundException(id);
            return _mapper.Map<Order, OrderToReturnDto>(order);
        }
    }
}
