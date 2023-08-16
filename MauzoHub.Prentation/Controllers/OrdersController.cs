using MauzoHub.Application.CQRS.Reports;
using MauzoHub.Application.DTOs;
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

        [HttpGet("userid/{id}")]
        public async Task<IActionResult> GetOrdersByUserId(Guid id)
        {
            var query = new GetOrdersByUserIdQuery { UserId = id };
            var orderDtos = await _mediator.Send(query);
            return Ok(orderDtos);
        }

        [HttpGet("between-dates")]
        public async Task<IActionResult> GetOrdersBetweenDates([FromQuery] DateTime dateFrom, [FromQuery] DateTime dateTo)
        {
            var query = new GetOrdersBetweenDatesQuery
            {
                DateFrom = dateFrom,
                DateTo = dateTo
            };

            var orders = await _mediator.Send(query);

            return Ok(orders);
        }

        [HttpGet("date")]
        public async Task<IActionResult> GetOrdersByDate([FromQuery] DateTime date)
        {
            var query = new GetOrdersForADateQuery
            {
                OrderDate = date
            };

            var orders = await _mediator.Send(query);

            return Ok(orders);
        }

        [HttpGet("productid")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersByProductId(Guid productId)
        {
            var query = new GetOrdersByProductIdQuery { ProductId = productId };
            var orderDtos = await _mediator.Send(query);

            return Ok(orderDtos);
        }
    }
}
