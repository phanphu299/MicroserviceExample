
namespace CustomerApi.Test
{
    using CustomerApi.Domains.Entities;
    using CustomerApi.Repositories;
    using System;
    using Xunit;
    using System.Linq;
    using Moq;
    using System.Collections.Generic;
    public class CustomerTest
    {
        private IRepository<Customer> _repository;

        public CustomerTest()
        {
            IList<Customer> customers = new List<Customer>
                {
                    new Customer
                {
                    Id = Guid.Parse("9f35b48d-cb87-4783-bfdb-21e36012930a"),
                    FirstName = "Wolfgang",
                    LastName = "Ofner",
                    Birthday = new DateTime(1989, 11, 23),
                    Age = 30
                },
                new Customer
                {
                    Id = Guid.Parse("654b7573-9501-436a-ad36-94c5696ac28f"),
                    FirstName = "Darth",
                    LastName = "Vader",
                    Birthday = new DateTime(1977, 05, 25),
                    Age = 42
                },
                new Customer
                {
                    Id = Guid.Parse("971316e1-4966-4426-b1ea-a36c9dde1066"),
                    FirstName = "Son",
                    LastName = "Goku",
                    Birthday = new DateTime(737, 04, 16),
                    Age = 1282
                }
                };

            Mock<IRepository<Customer>> mockCustomerRepository = new Mock<IRepository<Customer>>();

            // Return all the item
            mockCustomerRepository.Setup(mr => mr.GetAll()).Returns(customers.AsQueryable());

            // Allows us to test add new item
            mockCustomerRepository
                .Setup(i => i.AddAsync(It.IsAny<Customer>()))
                .Callback((Customer item) => customers.Add(item))
                .ReturnsAsync((Customer target) => { return target; });

            // Allows us to test update an item
            mockCustomerRepository.Setup(mr => mr.UpdateAsync(It.IsAny<Customer>())).ReturnsAsync(
                (Customer target) =>
                {

                    var original = customers.Where(
                        q => q.Id == target.Id).Single();

                    if (original == null)
                    {
                        return null;
                    }

                    original.FirstName = target.FirstName;
                    original.LastName = target.LastName;
                    original.Birthday = target.Birthday;
                    original.Age = target.Age;

                    return target;
                });

            _repository = mockCustomerRepository.Object;
        }

        [Fact]
        public void GetAll_should_return_all_customer()
        {
            // Act
            IList<Customer> customers = _repository.GetAll().ToList();

            // Assert
            Assert.NotNull(customers);
            Assert.Equal(3, customers.Count);
        }

        [Fact]
        public async void AddAsync_should_add_new_customer()
        {
            //Arrange
            Customer newCustomer = new Customer
            {
                Id = Guid.Parse("9f35b48d-cb87-4783-bfdb-21e99992930a"),
                FirstName = "phu",
                LastName = "phan",
                Birthday = new DateTime(1991, 11, 23),
                Age = 30
            };

            // Act
            Customer result = await _repository.AddAsync(newCustomer);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("phu", result.FirstName);
            Assert.Equal(4, _repository.GetAll().ToList().Count);
        }

        [Fact]
        public async void UpdateAsync_should_update_customer()
        {
            //Arrange
            Customer newCustomer = new Customer
            {
                Id = Guid.Parse("971316e1-4966-4426-b1ea-a36c9dde1066"),
                FirstName = "Son 2",
                LastName = "Goku 2",
                Birthday = new DateTime(737, 04, 16),
                Age = 12
            };

            // Act
            await _repository.UpdateAsync(newCustomer);

            // Assert
            Assert.Equal("Son 2", _repository.GetAll().FirstOrDefault(x => x.Id == newCustomer.Id).FirstName);
        }
    }
}
