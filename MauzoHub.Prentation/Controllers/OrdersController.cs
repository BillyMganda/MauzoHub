using MauzoHub.Application.CQRS.Reports;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MauzoHub.Prentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;
        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var query = new GetAllOrdersReport();
            var orderDtos = await _mediator.Send(query);
            return Ok(orderDtos);
        }

        [HttpGet("orderid/{id}")]
        public async Task<IActionResult> GetOrderById(Guid id)
        {
            var query = new GetOrderByIdQuery { OrderId = id };
            var orderDto = await _mediator.Send(query);
            return Ok(orderDto);
        }
    }
}
