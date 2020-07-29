namespace OrderApi.Events.Commands
{
    using AutoMapper;
    using FluentValidation;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using OrderApi.Common;
    using OrderApi.Common.Contants;
    using OrderApi.Common.Enums;
    using OrderApi.Domains.Entities;
    using OrderApi.Repositories;
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class PayOrder
    {
        public class Command : IRequest<ApiResult<Result>>
        {
            public Guid Id { get; set; }
        }

        public class Result
        {
            public bool IsSuccess { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(p => p.Id).NotEmpty().NotNull();
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
                var order = _repository.GetAll().AsNoTracking().FirstOrDefault(x => x.Id == command.Id);

                if (order == null)
                    return ApiResult<Result>.Fail(MessageContants.NotFound);

                order.OrderState = (int)OrderStatus.Paid;
                var result = await _repository.UpdateAsync(order);

                return result != null ?
                    ApiResult<Result>.Success(new Result { IsSuccess = true })
                    : ApiResult<Result>.Fail(MessageContants.UpdateFailed);
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
