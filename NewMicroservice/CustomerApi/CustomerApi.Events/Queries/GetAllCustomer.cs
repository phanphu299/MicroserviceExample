
namespace CustomerApi.Events.Queries
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using CustomerApi.Common;
    using CustomerApi.Domains.Entities;
    using CustomerApi.Repositories;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetAllCustomer
    {
        public class Query : IRequest<ApiResult<Result>>
        {
        }

        public class Result
        {
            public List<Customer> Customers { get; set; }
            public partial class Customer
            {
                public Guid Id { get; set; }
                public string FirstName { get; set; }
                public string LastName { get; set; }
                public DateTime? Birthday { get; set; }
                public int? Age { get; set; }
            }
        }

        public class QueryHandler : IRequestHandler<Query, ApiResult<Result>>
        {
            private readonly IRepository<Customer> _repository;
            private readonly IMapper _mapper;

            public QueryHandler(IRepository<Customer> repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<ApiResult<Result>> Handle(Query request, CancellationToken cancellationToken)
            {
                var customers = await _repository.GetAll().AsNoTracking().ProjectTo<Result.Customer>(_mapper.ConfigurationProvider).ToListAsync();
                
                return ApiResult<Result>.Success(new Result
                {
                    Customers = customers
                });
            }
        }
        public class Profile : AutoMapper.Profile
        {
            public Profile()
            {
                CreateMap<Customer, Result.Customer>();

            }
        }
    }
}
