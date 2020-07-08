
namespace CustomerApi.Events.Commands
{
    using AutoMapper;
    using CustomerApi.Common;
    using CustomerApi.Domains.Entities;
    using CustomerApi.Repositories;
    using FluentValidation;
    using MediatR;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class CreateCustomer
    {
        public class Command : IRequest<ApiResult<Result>>
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public DateTime? Birthday { get; set; }
            public int? Age { get; set; }
        }

        public class Result
        {
            public Guid Id { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(p => p.FirstName).NotNull().NotEmpty().MaximumLength(50);
                RuleFor(p => p.LastName).NotNull().NotEmpty().MaximumLength(50);
                RuleFor(p => p.Age).GreaterThan(0);
            }
        }

        public class CommandHandler : IRequestHandler<Command, ApiResult<Result>>
        {
            private readonly IRepository<Customer> _repository;
            private readonly IMapper _mapper;

            public CommandHandler(IRepository<Customer> repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<ApiResult<Result>> Handle(Command command, CancellationToken cancellationToken)
            {
                Customer customer = _mapper.Map<Command, Customer>(command, opt => opt.AfterMap((src, dest) => dest.Id = new Guid()));
                await _repository.AddAsync(customer);

                return ApiResult<Result>.Success(new Result
                {
                    Id = customer.Id,
                });
            }
        }

        public class Profile : AutoMapper.Profile
        {
            public Profile()
            {
                CreateMap<Command, Customer>();
            }
        }
    }
}
