
namespace OrderApi.Controllers
{
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using OrderApi.Events.Commands;
    using OrderApi.Events.Queries;
    using System;
    using System.Threading.Tasks;

    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        ///  Action to get an order
        /// </summary>
        /// <param name="id">Order Id</param>
        /// <returns>Return an order by Id</returns>
        /// <response code="200">Returned an order</response>
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(Guid id)
        {
            var model = await _mediator.Send(new GetOrderById.Query { Id = id });
            return Ok(model);
        }
        /// <summary>
        ///  Action to get all orders of a customer
        /// </summary>
        /// <param name="id">Customer Id</param>
        /// <returns>Return the list of orders by a customer Id</returns>
        /// <response code="200">Returned list of orders</response>
        [HttpGet("{id}/customer")]
        public async Task<ActionResult> GetByCustomerId(Guid id)
        {
            var model = await _mediator.Send(new GetOrderByCustomerId.Query { CustomerId = id });
            return Ok(model);
        }
        /// <summary>
        ///  Action to get all paid orders of a customer
        /// </summary>
        /// <returns>Return the list of paid orders</returns>
        /// <response code="200">Returned list of paid orders</response>
        [HttpGet("paid")]
        public async Task<ActionResult> GetPaidOrder()
        {
            var model = await _mediator.Send(new GetPaidOrder.Query());
            return Ok(model);
        }
        /// <summary>
        /// Action to create new order in the database
        /// </summary>
        /// <param name="command">Model to create a new order</param>
        /// <returns>Return the created order ID</returns>
        /// <response code="200">Returned if the order was created</response>
        /// <response code="400">Returned if the model couldn't parsed or the order couldn't be saved or the validation failed</response>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CreateOrder.Command command)
        {
            var model = await _mediator.Send(command);
            return Ok(model);
        }
        /// <summary>
        ///     Action to pay an order.
        /// </summary>
        /// <param name="id">The id of the order which got paid</param>
        /// <returns>Returns the paid order</returns>
        /// <response code="200">Returned if the order was updated (paid)</response>
        /// <response code="400">Returned if the order could not be found with the provided id</response>
        [HttpPut("Pay/{id}")]
        public async Task<ActionResult> Pay(Guid id)
        {
            var model = await _mediator.Send(new PayOrder.Command { Id = id });
            return Ok(model);
        }

    }
}
