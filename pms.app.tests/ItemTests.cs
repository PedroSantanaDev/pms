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
            var dbOptions = DbHelper.GetDbOptions();
            _dbContext = new ApplicationDbContext(dbOptions);

            // Initialize UnitOfWork with the actual ApplicationDbContext
            _unitOfWork = new UnitOfWork.UnitOfWork(_dbContext);
        }


        [Fact]
        public void Item_Properties_Should_Work_Correctly_Tests()
        {
            var item = new Item
            {
                Id = 1,
                Name = "TestItem",
                Description = "Test Description",
                Price = 99.99m,
                Status = "Active",
                Created = new DateTime(2024, 4, 1),
                CategoryId = 1
            };

            // Assert
            Assert.Equal(1, item.Id);
            Assert.Equal("TestItem", item.Name);
            Assert.Equal("Test Description", item.Description);
            Assert.Equal(99.99m, item.Price);
            Assert.Equal("Active", item.Status);
            Assert.Equal(new DateTime(2024, 4, 1), item.Created);
            Assert.Equal(1, item.CategoryId);
            Assert.Null(item.Category);
        }

        [Fact]
        public void Item_Category_Navigation_Property_Should_Be_Null_By_Default_Test()
        {
            var item = new Item
            {
                Id = 1,
                Name = "TestItem",
                Description = "Test Description",
                Price = 99.99m,
                Status = "Active",
                Created = new DateTime(2024, 4, 1),
                CategoryId = 1,
            };

            // Assert
            Assert.Null(item.Category);
        }

        [Fact]
        public void Item_Category_Navigation_Property_Should_Not_Be_Null_Test()
        {
            var category = new Category
            {
                Id = 1,
                Name = "Computer",
                Description = "Test Description",
                Created = DateTime.Now,
            };
            var item = new Item
            {
                Id = 1,
                Name = "TestItem",
                Description = "Test Description",
                Price = 99.99m,
                Status = "Active",
                Created = new DateTime(2024, 4, 1),
                CategoryId = 1,
                Category = category
            };


            // Assert
            Assert.NotNull(item.Category);
            Assert.Equal(category.Id, item.CategoryId);
            Assert.Equal(item.CategoryId, category.Id);
        }

        [Fact]
        public async Task Add_Range_Item_Should_Add_List_Of_Items_To_DB_Test()
        {
            var items = GetTestItems();

            await _unitOfWork.GetRepository<Item>().AddRangeAsync(items);
            await _unitOfWork.SaveChangesAsync();
            var result = await _unitOfWork.GetRepository<Item>().GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task Get_All_Items_Should_Return_List_Of_Items_Test()
        {
            var result = await _unitOfWork.GetRepository<Item>().GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.IsType<List<Item>>(result);
        }

        [Fact]
        public async Task Get_All_Items_With_Query_Should_Return_List_Of_Items_For_Matching_Query_Test()
        {
            // Set up filter
            Expression<Func<Item, bool>> filter = i => i.Status == "Active";

            Func<IQueryable<Item>, IOrderedQueryable<Item>> orderBy = q => q.OrderBy(i => i.Name);

            // Set up includeProperties
            string includeProperties = "Category";

            // Set up pagination
            int page = 1;
            int pageSize = 10;

            var result = await _unitOfWork.GetRepository<Item>().GetAllAsync(
                filter,
                orderBy,
                includeProperties,
                page,
                pageSize
            );

            //Assert
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
        public async Task Get_Item_By_Id_Should_Return_Item_For_Specified_Id_Test()
        {
            var items = await _unitOfWork.GetRepository<Item>().GetAllAsync();
            int id = items.First().Id;

            var result = await _unitOfWork.GetRepository<Item>().GetByIdAsync(id);
            Assert.NotNull(result);
            Assert.IsType<Item>(result);
        }

        [Fact]
        public async Task Update_Item_Should_Update_Item_If_Exist_Test()
        {
            var items = await _unitOfWork.GetRepository<Item>().GetAllAsync();
            int id = items.First().Id;

            var item = await _unitOfWork.GetRepository<Item>().GetByIdAsync(id);

            // Assert
            Assert.NotNull(item);
            Assert.IsType<Item>(item);
            Assert.Equal(id, item.Id);

            item.Name = "McBook";
            item.Description = "Apple McBook";
            item.Price = 2250;
            await _unitOfWork.GetRepository<Item>().UpdateAsync(item);

            item = await _unitOfWork.GetRepository<Item>().GetByIdAsync(id);

            //Assert
            Assert.NotNull(item);
            Assert.IsType<Item>(item);
            Assert.Equal(id, item.Id);
            Assert.Equal("McBook", item.Name);
            Assert.Equal("Apple McBook", item.Description);
            Assert.Equal(2250, item.Price);
        }
        [Fact]
        public async Task Delete_Item_Should_Delete_Item_If_Exists_Test()
        {
            var items = await _unitOfWork.GetRepository<Item>().GetAllAsync();

            int id = items.First().Id;

            var item = await _unitOfWork.GetRepository<Item>().GetByIdAsync(id);

            //Assert
            Assert.NotNull(item);
            Assert.IsType<Item>(item);

            var customerItems = await _unitOfWork.GetRepository<CustomerItem>().GetAllAsync(i => i.ItemId == item.Id);
            foreach (var customerItem in customerItems)
            {
                await _unitOfWork.GetRepository<CustomerItem>().DeleteAsync(customerItem.Id);
            }

            // Set CategoryId to null if it's not already null
            if (item.CategoryId != null)
            {
                item.CategoryId = null;
                item.Updated = DateTime.Now;
                await _unitOfWork.GetRepository<Item>().UpdateAsync(item);
            }

            await _unitOfWork.GetRepository<Item>().DeleteAsync(id);

            item = await _unitOfWork.GetRepository<Item>().GetByIdAsync(id);

            //Assert
            Assert.Null(item);
        }

        [Fact]
        public async Task Add_Category_To_Item_Should_Assign_Category_Test()
        {
            var items = await _unitOfWork.GetRepository<Item>().GetAllAsync();
            var categories = await _unitOfWork.GetRepository<Category>().GetAllAsync();

            //Assert
            Assert.NotNull(items);
            Assert.NotNull(categories);

            var category = categories.First();
            var item = items.First();
            var itemId = item.Id;
            var categoryId = category.Id;
            //Assert
            Assert.NotNull(item);
            Assert.NotNull(category);

            item.Category = category;
            item.CategoryId = category.Id;

            await _unitOfWork.GetRepository<Item>().UpdateAsync(item);

            item = await _unitOfWork.GetRepository<Item>().GetByIdAsync(itemId);

            //Assert
            Assert.NotNull(item);
            Assert.IsType<Item>(item);
            Assert.Equal(categoryId, item.CategoryId);
            Assert.NotNull(item.Category);
            Assert.IsType<Category>(item.Category);
        }

        // Helper method to return test items
        private List<Item> GetTestItems()
        {
            return new List<Item> {
                new Item { Name = "Monitor", Status = "Active" },
                new Item { Name = "Laptop", Status = "Inactive" },
                new Item { Name = "Mouse", Status = "Inactive" },
                new Item { Name = "Headphones", Description = "Working headphones with microphone", Price = 99.99m, Status = "Inactive" }
            };
        }

        // Dispose DbContext after each test
        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
