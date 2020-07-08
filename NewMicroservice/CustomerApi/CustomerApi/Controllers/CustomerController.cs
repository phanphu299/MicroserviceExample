

namespace CustomerApi.Controllers
{
    using CustomerApi.Events.Commands;
    using CustomerApi.Events.Queries;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CustomerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CreateCustomer.Command command)
        {
            var model = await _mediator.Send(command);
            return Ok(model);
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var model = await _mediator.Send(new GetAllCustomer.Query());
            return Ok(model);
        }

        [HttpPut]
        public async Task<ActionResult> Put([FromBody] UpdateCustomer.Command command)
        {
            var model = await _mediator.Send(command);
            return Ok(model);
        }
    }
}
