
namespace CustomerApi.Events.Commands
{
    using AutoMapper;
    using CustomerApi.Common;
    using CustomerApi.Common.Contants;
    using CustomerApi.Domains.Entities;
    using CustomerApi.Message.Send.Message;
    using CustomerApi.Message.Send.Sender;
    using CustomerApi.Repositories;
    using FluentValidation;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class UpdateCustomer
    {
        public class Command : IRequest<ApiResult<Result>>
        {
            public Guid Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public DateTime? Birthday { get; set; }
            public int? Age { get; set; }
        }

        public class Result
        {
            public bool IsSuccess { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(p => p.Id).NotNull().NotEmpty();
                RuleFor(p => p.FirstName).NotNull().NotEmpty().MaximumLength(50);
                RuleFor(p => p.LastName).NotNull().NotEmpty().MaximumLength(50);
                RuleFor(p => p.Age).GreaterThan(0);
            }
        }

        public class CommandHandler : IRequestHandler<Command, ApiResult<Result>>
        {
            private readonly IRepository<Customer> _repository;
            private readonly IMapper _mapper;
            private readonly ICustomerUpdateSender _customerUpdateSender;

            public CommandHandler(IRepository<Customer> repository, IMapper mapper, ICustomerUpdateSender customerUpdateSender)
            {
                _repository = repository;
                _mapper = mapper;
                _customerUpdateSender = customerUpdateSender;
            }

            public async Task<ApiResult<Result>> Handle(Command command, CancellationToken cancellationToken)
            {
                var customer = _repository.GetAll().AsNoTracking().FirstOrDefault(x => x.Id == command.Id);

                if (customer == null)
                    return ApiResult<Result>.Fail(MessageContants.NotFound);

                var itemToUpdate = _mapper.Map<Command, Customer>(command);
                var result = await _repository.UpdateAsync(itemToUpdate);

                var updateCustomerMessage = _mapper.Map<Customer, UpdateCustomerMessage>(result);
                _customerUpdateSender.SendUpdateCustomer(updateCustomerMessage);

                return result != null ? 
                    ApiResult<Result>.Success(new Result { IsSuccess = true })
                    : ApiResult<Result>.Fail(MessageContants.UpdateFailed);
            }
        }

        public class Profile : AutoMapper.Profile
        {
            public Profile()
            {
                CreateMap<Command, Customer>();
                CreateMap<Customer, UpdateCustomerMessage>();
            }
        }
    }
}
