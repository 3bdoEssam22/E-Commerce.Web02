
using AutoMapper;
using DomainLayer.Contracts;
using DomainLayer.Exceptions;
using DomainLayer.Models.BasketModule;
using DomainLayer.Models.OrderModule;
using Microsoft.Extensions.Configuration;
using ServiceAbstraction;
using Shared.DataTtansferObjects.BasketModuleDto;
using Stripe;
using Product = DomainLayer.Models.ProductModule.Product;

namespace Service
{
    public class PaymentService(IConfiguration _configuration,
        IBasketRepository _basketRepository,
        IUnitOfWork _unitOfWork,
        IMapper _mapper) : IPaymentService
    {
        public async Task<BasketDto> CreateOrUpdatePaymentIntentAsync(string BasketId)
        {
            //Configure Stripe
            StripeConfiguration.ApiKey = _configuration["StripeSetting:SecretKey"];

            //Get Basket
            var basket = await _basketRepository.GetBasketByIdAsync(BasketId) ?? throw new BasketNotFoundException(BasketId);

            //Get Amount - Get Products + Delivery Method Cost
            var productRepo = _unitOfWork.GetRepository<Product, int>();
            foreach (var item in basket.Items)
            {
                var product = await productRepo.GetByIdAsync(item.Id) ?? throw new ProductNotFoundException(item.Id);
                item.Price = product.Price;
            }
            ArgumentNullException.ThrowIfNull(basket.DeliveryMethodId);
            var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetByIdAsync(basket.DeliveryMethodId.Value)
                ?? throw new DeliveryMethodNotFoundException(basket.DeliveryMethodId.Value);
            basket.ShippingPrice = deliveryMethod.Price;

            var basketAmount = (long)(basket.Items.Sum(item => item.Quantity * item.Price) + deliveryMethod.Price) * 100;

            //Create payment intent [Create - Update]
            var paymentService = new PaymentIntentService();

            if (basket.PaymentIntentId is null) //create
            {
                var options = new PaymentIntentCreateOptions()
                {
                    Amount = basketAmount,
                    Currency = "USD",
                    PaymentMethodTypes = ["card"]
                };
                var paymentIntent = await paymentService.CreateAsync(options);
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else //update
            {
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = basketAmount
                };
                await paymentService.UpdateAsync(basket.PaymentIntentId, options);
            }
            await _basketRepository.CreateOrUpdateBasket(basket);
            return _mapper.Map<CustomerBasket, BasketDto>(basket);

        }
    }
}
