
namespace OrderApi.Events.Queries
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using FluentValidation;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using OrderApi.Common;
    using OrderApi.Common.Enums;
    using OrderApi.Domains.Entities;
    using OrderApi.Repositories;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetPaidOrder
    {
        public class Query : IRequest<ApiResult<Result>>
        {
        }

        public class Result
        {
            public List<Order> Orders { get; set; }
            public class Order
            {
                public Guid Id { get; set; }
                public int OrderState { get; set; }
                public Guid CustomerGuid { get; set; }
                public string CustomerFullName { get; set; }
            }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
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
                var orders = await _repository.GetAll()
                    .AsNoTracking()
                    .Where(x => x.OrderState == (int)OrderStatus.Paid)
                    .ProjectTo<Result.Order>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);

                return ApiResult<Result>.Success(new Result
                {
                    Orders = orders
                });
            }
        }

        public class Profile : AutoMapper.Profile
        {
            public Profile()
            {
                CreateMap<Order, Result.Order>();

            }
        }
    }
}
