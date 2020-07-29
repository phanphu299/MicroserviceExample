
namespace OrderApi.Events.Queries
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using FluentValidation;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using OrderApi.Common;
    using OrderApi.Domains.Entities;
    using OrderApi.Repositories;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetOrderById
    {
        public class Query : IRequest<ApiResult<Result>>
        {
            public Guid Id { get; set; }
        }

        public class Result
        {
            public Guid Id { get; set; }
            public int OrderState { get; set; }
            public Guid CustomerGuid { get; set; }
            public string CustomerFullName { get; set; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(p => p.Id).NotNull().NotEmpty();
            }
        }

        public class QueryHandler : IRequestHandler<Query, ApiResult<Result>>
        {
            private readonly IRepository<Order> _repository;
            private readonly IMapper _mapper;

            public QueryHandler(IRepository<Order> repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<ApiResult<Result>> Handle(Query request, CancellationToken cancellationToken)
            {
                var order = await _repository.GetAll()
                    .AsNoTracking()
                    .ProjectTo<Result>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(x => x.Id == request.Id);

                return ApiResult<Result>.Success(order);
            }
        }

        public class Profile : AutoMapper.Profile
        {
            public Profile()
            {
                CreateMap<Order, Result>();

            }
        }
    }
}
