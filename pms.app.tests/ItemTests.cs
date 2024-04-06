using Microsoft.EntityFrameworkCore;
using pms.app.Data;
using pms.app.Models;
using System.Linq.Expressions;

namespace pms.app.tests
{
    public class ItemTests : IDisposable
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UnitOfWork.UnitOfWork _unitOfWork;

        public ItemTests()
        {
            // Set up the test database connection string
            var connectionString = "Data Source=pms.db";

            // Set up the DbContext with the test database connection string
            var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connectionString)
                .Options;
            _dbContext = new ApplicationDbContext(dbContextOptions);

            // Initialize UnitOfWork with the actual ApplicationDbContext
            _unitOfWork = new UnitOfWork.UnitOfWork(_dbContext);
        }

        [Fact]
        public async Task AddRangeItemTest()
        {
            var items = GetTestItems();

            await _unitOfWork.GetRepository<Item>().AddRangeAsync(items);
            await _unitOfWork.SaveChangesAsync();
            var result = await _unitOfWork.GetRepository<Item>().GetAllAsync();

            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task GetAllItemsTest()
        {
            // Add items to database
            //_unitOfWork_unitOfWork.GetRepository<Item>().AddRange(items);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _unitOfWork.GetRepository<Item>().GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.IsType<List<Item>>(result);
        }

        [Fact]
        public async Task GetAllItemsWithQueryTest()
        {
            // Set up filter
            Expression<Func<Item, bool>> filter = i => i.Status == "Active";

            Func<IQueryable<Item>, IOrderedQueryable<Item>> orderBy = q => q.OrderBy(i => i.Name);

            // Set up includeProperties
            string includeProperties = "Category";

            // Set up pagination
            int page = 1;
            int pageSize = 10;

            // Act
            var result = await _unitOfWork.GetRepository<Item>().GetAllAsync(
                filter,
                orderBy,
                includeProperties,
                page,
                pageSize
            );
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.IsType<List<Item>>(result);
            Assert.True(result.Count <= 10);

            foreach (var item in result)
            {
                Assert.NotNull(item);
                Assert.Equal("Active", item.Status);
            }
        }

        [Fact]
        public async Task GetItemByIdTest()
        {
            var items = await _unitOfWork.GetRepository<Item>().GetAllAsync();
            int id = items.First().Id;

            var result = await _unitOfWork.GetRepository<Item>().GetByIdAsync(id);
            Assert.NotNull(result);
            Assert.IsType<Item>(result);
        }

        [Fact]
        public async Task UpdateItemTest()
        {
            var items = await _unitOfWork.GetRepository<Item>().GetAllAsync();
            int id = items.First().Id;

            var item = await _unitOfWork.GetRepository<Item>().GetByIdAsync(id);

            Assert.NotNull(item);
            Assert.IsType<Item>(item);
            Assert.Equal(id, item.Id);

            item.Name = "McBook";
            item.Description = "Apple McBook";
            item.Price = 2250;
            await _unitOfWork.GetRepository<Item>().UpdateAsync(item);

            item = await _unitOfWork.GetRepository<Item>().GetByIdAsync(id);

            Assert.NotNull(item);
            Assert.IsType<Item>(item);
            Assert.Equal(id, item.Id);
            Assert.Equal("McBook", item.Name);
            Assert.Equal("Apple McBook", item.Description);
            Assert.Equal(2250, item.Price);
        }
        [Fact]
        public async Task DeleteItemTest()
        {
            var items = await _unitOfWork.GetRepository<Item>().GetAllAsync();
            int id = items.First().Id;

            var item = await _unitOfWork.GetRepository<Item>().GetByIdAsync(id);
            Assert.NotNull(item);
            Assert.IsType<Item>(item);

            await _unitOfWork.GetRepository<Item>().DeleteAsync(id);

            item = await _unitOfWork.GetRepository<Item>().GetByIdAsync(id);
            Assert.Null(item);
        }

        // Helper method to return test items
        private List<Item> GetTestItems() =>
        [
            new() { Name = "PC" , Status = "Active"},
            new() { Name = "Monitor", Status = "Active"},
            new() { Name = "Laptop", Status = "Inactive" },
            new() { Name = "Mouse", Status = "Inactive" },
            new() { Name = "Headphones", Description= "Working headphones with microphone", Price= 99.99m, Status = "Inactive" }
        ];

        // Dispose DbContext after each test
        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
