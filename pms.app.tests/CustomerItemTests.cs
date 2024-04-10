using pms.app.Data;
using pms.app.Models;

namespace pms.app.tests
{
    public class CustomerItemTests
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UnitOfWork.UnitOfWork _unitOfWork;
        public CustomerItemTests()
        {
            var dbOptions = DbHelper.GetDbOptions();
            _dbContext = new ApplicationDbContext(dbOptions);

            // Initialize UnitOfWork with the actual ApplicationDbContext
            _unitOfWork = new UnitOfWork.UnitOfWork(_dbContext);
        }
        [Fact]
        public async Task Add_CustomerItem_To_Should_Assign_CustomerItem_Test()
        {
            var items = await _unitOfWork.GetRepository<Item>().GetAllAsync();
            var customers = await _unitOfWork.GetRepository<Customer>().GetAllAsync();
            var customer = customers[2];
            var item = items[2];

            var customerItem = new CustomerItem { CustomerId = customer.Id, ItemId = item.Id, Item = item, Customer = customer, Quantity = 5 };

            await _unitOfWork.GetRepository<CustomerItem>().AddAsync(customerItem);

            var result = await _unitOfWork.GetRepository<CustomerItem>().GetAllAsync(i => i.CustomerId == customer.Id && i.ItemId == item.Id);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<List<CustomerItem>>(result);
            Assert.Equal(result.First().CustomerId, customer.Id);
            Assert.Equal(result.First().ItemId, item.Id);
        }
    }
}
