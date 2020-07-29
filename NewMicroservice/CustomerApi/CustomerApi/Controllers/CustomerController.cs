

namespace CustomerApi.Controllers
{
    using CustomerApi.Events.Commands;
    using CustomerApi.Events.Queries;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CustomerController(IMediator mediator)
        {
            _mediator = mediator;
        }
        /// <summary>
        /// Action to create new customer in the database
        /// </summary>
        /// <param name="command">Model to create a new customer</param>
        /// <returns>Return the created customer ID</returns>
        /// <response code="200">Returned if the customer was created</response>
        /// <response code="400">Returned if the model couldn't parsed or the customer couldn't be saved or the validation failed</response>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CreateCustomer.Command command)
        {
            var model = await _mediator.Send(command);
            return Ok(model);
        }
        /// <summary>
        /// Action to get all customers
        /// </summary>
        /// <returns>Return the list of customers</returns>
        /// <response code="200">Returned list of customers</response>
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var model = await _mediator.Send(new GetAllCustomer.Query());
            return Ok(model);
        }
        /// <summary>
        /// Action to update an existing customer
        /// </summary>
        /// <param name="command">Model to update an existing customer</param>
        /// <returns>Returns the updated customer</returns>
        /// <response code="200">Returned if the customer was updated</response>
        /// <response code="400">Returned if the model couldn't be parsed or the customer couldn't be found or the validation failed</response>
        [HttpPut]
        public async Task<ActionResult> Put([FromBody] UpdateCustomer.Command command)
        {
            var model = await _mediator.Send(command);
            return Ok(model);
        }
    }
}
