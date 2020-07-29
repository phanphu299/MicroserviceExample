
namespace OrderApi.Events.Commands
{
    using AutoMapper;
    using FluentValidation;
    using MediatR;
    using OrderApi.Common;
    using OrderApi.Domains.Entities;
    using OrderApi.Repositories;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class CreateOrder
    {
        public class Command : IRequest<ApiResult<Result>>
        {
            public Guid CustomerGuid { get; set; }
            public string CustomerFullName { get; set; }
            public int OrderState { get; set; }
        }

        public class Result
        {
            public Guid Id { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(p => p.OrderState).GreaterThan(0);
                RuleFor(p => p.CustomerFullName).NotNull().NotEmpty().MaximumLength(50);
                RuleFor(p => p.CustomerGuid).NotEmpty().NotNull();
            }
        }

        public class CommandHandler : IRequestHandler<Command, ApiResult<Result>>
        {
            private readonly IRepository<Order> _repository;
            private readonly IMapper _mapper;

            public CommandHandler(IRepository<Order> repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<ApiResult<Result>> Handle(Command command, CancellationToken cancellationToken)
            {
                Order order = _mapper.Map<Command, Order>(command, opt => opt.AfterMap((src, dest) => dest.Id = new Guid()));
                await _repository.AddAsync(order);

                return ApiResult<Result>.Success(new Result
                {
                    Id = order.Id,
                });
            }
        }

        public class Profile : AutoMapper.Profile
        {
            public Profile()
            {
                CreateMap<Command, Order>();
            }
        }
    }
}
