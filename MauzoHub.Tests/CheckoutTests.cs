using MauzoHub.Application.CQRS.Checkouts;
using MauzoHub.Domain.Entities;
using MauzoHub.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Moq;

namespace MauzoHub.Tests
{
    public class CheckoutTests
    {
        [Fact]
        public async Task ProceedToCheckout_Success()
        {
            // arrange
            var mockCartRepository = new Mock<ICartRepository>();
            var mockCheckoutRepository = new Mock<ICheckoutRepository>();
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            
            var handler = new ProceedToCheckoutCommandHandler(
                mockCartRepository.Object,
                mockCheckoutRepository.Object,
                mockHttpContextAccessor.Object);

            var command = new ProceedToCheckoutCommand
            {
                UserId = Guid.NewGuid(),
                Payment = new Payment(),
                ShippingAddress = new ShippingAddress()
            };

            mockCartRepository.Setup(repo => repo.GetByUserIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new Cart { UserId = command.UserId, Items = new List<CartItem>() });

            mockCheckoutRepository.Setup(repo => repo.AddAsync(It.IsAny<Checkout>()))
                .ReturnsAsync((Checkout checkout) => checkout);

            // act
            await handler.Handle(command, CancellationToken.None);

            // assert
            mockCartRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Cart>()), Times.Once);
        }
    }
}