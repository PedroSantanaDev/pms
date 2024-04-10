using pms.app.Data;
using pms.app.Enums;
using pms.app.Models;
using System.Linq.Expressions;

namespace pms.app.tests
{
    public class CustomerTests : IDisposable
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UnitOfWork.UnitOfWork _unitOfWork;

        public CustomerTests()
        {
            var dbOptions = DbHelper.GetDbOptions();
            _dbContext = new ApplicationDbContext(dbOptions);

            // Initialize UnitOfWork with the actual ApplicationDbContext
            _unitOfWork = new UnitOfWork.UnitOfWork(_dbContext);
        }

        [Fact]
        public void Customer_Created_With_Required_Values_Tests()
        {
            var name = "JJ Automotive";
            var status = "Active";

            // Act
            var customer = GetTestCustomers().First();

            // Assert
            Assert.Equal(name, customer.Name);
            Assert.Equal(status, customer.Status);
            Assert.NotEqual(default, customer.Created);
            Assert.Null(customer.Updated);
        }

        [Fact]
        public void Customer_Items_Navigation_Property_Should_Be_Empty_By_Default_Test()
        {
            var customer = GetTestCustomers().First();

            // Assert
            Assert.NotNull(customer.CustomerItems);
            Assert.Empty(customer.CustomerItems);
        }

        [Fact]
        public async Task Customer_Items_Navigation_Property_Should_Not_Be_Null_Test()
        {
            var items = await _unitOfWork.GetRepository<Item>().GetAllAsync();
            var item = items.First();

            var customers = await _unitOfWork.GetRepository<Customer>().GetAllAsync();
            var customer = customers.First();

            var customerItem = new CustomerItem { CustomerId = customer.Id, ItemId = item.Id, Item = item, Customer = customer, Quantity = 2 };

            customer.CustomerItems.Add(customerItem);

            await _unitOfWork.GetRepository<Customer>().UpdateAsync(customer);

            var updatedCustomer = await _unitOfWork.GetRepository<Customer>().GetByIdAsync(customer.Id);

            Assert.NotNull(updatedCustomer);
            Assert.NotNull(updatedCustomer.CustomerItems);
            Assert.True(updatedCustomer.CustomerItems.Count > 0);
        }

        [Fact]
        public async Task Get_All_Customers_Should_Return_List_Of_Customers_Test()
        {
            var result = await _unitOfWork.GetRepository<Customer>().GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.IsType<List<Customer>>(result);
            Assert.True(result.Count > 0);
        }


        [Fact]
        public async Task Get_All_Customers_With_Query_Should_Return_List_Of_Customers_For_Matching_Query_Test()
        {
            // Set up filter
            Expression<Func<Customer, bool>> filter = i => i.Status == "Active";

            Func<IQueryable<Customer>, IOrderedQueryable<Customer>> orderBy = q => q.OrderBy(i => i.Name);

            // Set up includeProperties
            string includeProperties = "CustomerItems";

            // Set up pagination
            int page = 1;
            int pageSize = 10;

            var result = await _unitOfWork.GetRepository<Customer>().GetAllAsync(
                filter,
                orderBy,
                includeProperties,
                page,
                pageSize
            );

            //Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.IsType<List<Customer>>(result);
            Assert.True(result.Count <= 10);

            foreach (var customer in result)
            {
                Assert.NotNull(customer);
                Assert.Equal("Active", customer.Status);
            }
        }

        [Fact]
        public async Task Get_Customer_By_Id_Should_Return_Customer_For_Specified_Id_Test()
        {
            var customers = await _unitOfWork.GetRepository<Customer>().GetAllAsync();
            int id = customers.First().Id;

            var result = await _unitOfWork.GetRepository<Customer>().GetByIdAsync(id);
            Assert.NotNull(result);
            Assert.IsType<Customer>(result);
        }

        [Fact]
        public async Task Update_Customer_Should_Update_Customer_If_Exist_Test()
        {
            var customers = await _unitOfWork.GetRepository<Customer>().GetAllAsync();
            int id = customers.First().Id;

            var customer = await _unitOfWork.GetRepository<Customer>().GetByIdAsync(id);

            // Assert
            Assert.NotNull(customer);
            Assert.IsType<Customer>(customer);
            Assert.Equal(id, customer.Id);

            customer.Name = "Rolland Bike Store";
            await _unitOfWork.GetRepository<Customer>().UpdateAsync(customer);

            var updateCustomer = await _unitOfWork.GetRepository<Customer>().GetByIdAsync(id);

            //Assert
            Assert.NotNull(updateCustomer);
            Assert.IsType<Customer>(updateCustomer);
            Assert.Equal(id, customer.Id);
            Assert.Equal("Rolland Bike Store", updateCustomer.Name);

        }

        [Fact]
        public async Task Delete_Customer_Should_Delete_Customer_If_Exists_Test()
        {
            // Set up filter
            Expression<Func<Customer, bool>> filter = i => i.CustomerItems != null && i.CustomerItems.Count > 0;

            Func<IQueryable<Customer>, IOrderedQueryable<Customer>> orderBy = q => q.OrderBy(i => i.Id);

            // Set up includeProperties
            string includeProperties = "CustomerItems";

            // Set up pagination
            int page = 1;
            int pageSize = 2;

            var customers = await _unitOfWork.GetRepository<Customer>().GetAllAsync(
                filter,
                orderBy,
                includeProperties,
                page,
                pageSize
            );

            int id = customers.First().Id;

            var customer = customers.First();

            //Assert
            Assert.NotNull(customer);
            Assert.IsType<Customer>(customer);

            if (customer.CustomerItems != null && customer.CustomerItems.Count > 0)
            {
                var customerItems = await _unitOfWork.GetRepository<CustomerItem>().GetAllAsync(i => i.CustomerId == customer.Id);
                // Update CustomerItem reference for each item associated with this customer
                foreach (var customerItem in customerItems)
                {
                    await _unitOfWork.GetRepository<CustomerItem>().DeleteAsync(customerItem.Id);

                    var deletedCustomerItem = await _unitOfWork.GetRepository<CustomerItem>().GetByIdAsync(customerItem.Id);
                    Assert.Null(deletedCustomerItem);
                }
            }

            await _unitOfWork.GetRepository<Customer>().DeleteAsync(id);

            var result = await _unitOfWork.GetRepository<Customer>().GetByIdAsync(id);

            //Assert
            Assert.Null(result);
        }

        private List<Customer> GetTestCustomers()
        {
            return new List<Customer> {
                new Customer { Name = "JJ Automotive", Status = Status.Statuses.Active.ToString() },
                new Customer { Name = "LL Auto Care", Status = Status.Statuses.Active.ToString()},
                new Customer { Name = "Peter's Book Store", Status = Status.Statuses.Innactive.ToString() },
                new Customer { Name = "May Computer Repair", Status = Status.Statuses.Innactive.ToString() }
            };
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
