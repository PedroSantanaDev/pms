using pms.app.Data;
using pms.app.Models;
using System.Linq.Expressions;

namespace pms.app.tests
{
    public class CategoryTests : IDisposable
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UnitOfWork.UnitOfWork _unitOfWork;
        public CategoryTests()
        {
            var dbOptions = DbHelper.GetDbOptions();
            _dbContext = new ApplicationDbContext(dbOptions);

            // Initialize UnitOfWork with the actual ApplicationDbContext
            _unitOfWork = new UnitOfWork.UnitOfWork(_dbContext);
        }
        [Fact]
        public void Category_Properties_Should_Work_Correctly()
        {

            var category = new Category
            {
                Id = 1,
                Name = "Computer",
                Description = "Test Description",
                Created = DateTime.Now,
            };

            //Assert
            Assert.Equal(1, category.Id);
            Assert.Equal("Computer", category.Name);
            Assert.Equal("Test Description", category.Description);
            Assert.Equal(DateTime.Now.Date, category.Created.Date);
        }

        [Fact]
        public void Category_Should_Not_Be_Null()
        {
            var category = new Category();

            Assert.NotNull(category);
        }

        [Fact]
        public async Task Add_Range_Category_Should_Add_List_Of_Categories_To_DB_Test()
        {
            var categories = GetTestCategories();

            await _unitOfWork.GetRepository<Category>().AddRangeAsync(categories);
            await _unitOfWork.SaveChangesAsync();
            var result = await _unitOfWork.GetRepository<Category>().GetAllAsync();

            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task Get_All_Categories_Should_Return_List_Of_Categories_Test()
        {
            var result = await _unitOfWork.GetRepository<Category>().GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.IsType<List<Category>>(result);
        }

        [Fact]
        public async Task Get_All_Categories_With_Query_Should_Return_List_Of_Categories_For_Matching_Query_Test()
        {
            // Set up filter
            Expression<Func<Category, bool>> filter = i => i.Name != null;

            Func<IQueryable<Category>, IOrderedQueryable<Category>> orderBy = q => q.OrderBy(i => i.Name);

            // Set up includeProperties
            string includeProperties = "";

            // Set up pagination
            int page = 1;
            int pageSize = 10;

            var result = await _unitOfWork.GetRepository<Category>().GetAllAsync(
                filter,
                orderBy,
                includeProperties,
                page,
                pageSize
            );

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.IsType<List<Category>>(result);
            Assert.True(result.Count <= 10);

            foreach (var category in result)
            {
                Assert.NotNull(category);
                Assert.NotNull(category.Name);
            }
        }


        [Fact]
        public async Task Get_Category_By_Id_Should_Return_Category_For_Specified_Id_Test()
        {
            var categories = await _unitOfWork.GetRepository<Category>().GetAllAsync();
            int id = categories.First().Id;

            var result = await _unitOfWork.GetRepository<Category>().GetByIdAsync(id);
            Assert.NotNull(result);
            Assert.IsType<Category>(result);
        }

        [Fact]
        public async Task Update_Category_Should_Update_Category_If_Exist_Test()
        {
            var categories = await _unitOfWork.GetRepository<Category>().GetAllAsync();
            int id = categories.First().Id;

            var category = await _unitOfWork.GetRepository<Category>().GetByIdAsync(id);

            // Assert
            Assert.NotNull(category);
            Assert.IsType<Category>(category);
            Assert.Equal(id, category.Id);

            category.Description = "Test Description";
            category.Updated = DateTime.UtcNow;

            await _unitOfWork.GetRepository<Category>().UpdateAsync(category);

            category = await _unitOfWork.GetRepository<Category>().GetByIdAsync(id);

            Assert.NotNull(category);
            Assert.IsType<Category>(category);
            Assert.Equal(id, category.Id);
            Assert.Equal("Test Description", category.Description);
        }

        [Fact]
        public async Task Delete_Category_Should_Delete_Category_If_Exists_Test()
        {
            var categories = await _unitOfWork.GetRepository<Category>().GetAllAsync();
            var category = categories.FirstOrDefault(); // Deleting the first category for testing

            // Assert
            Assert.NotNull(category);

            // Get items associated with the category
            var items = await _unitOfWork.GetRepository<Item>().GetAllAsync(i => i.CategoryId == category.Id);

            // Update category reference for each item associated with this category
            foreach (var item in items)
            {
                item.CategoryId = null;
                await _unitOfWork.GetRepository<Item>().UpdateAsync(item);
            }

            // Delete the category
            await _unitOfWork.GetRepository<Category>().DeleteAsync(category.Id);

            var deletedCategory = await _unitOfWork.GetRepository<Category>().GetByIdAsync(category.Id);

            // Assert that the category is deleted
            Assert.Null(deletedCategory);

            // Assert that associated items are updated (category reference set to null or to a different category)
            foreach (var item in items)
            {
                var updatedItem = await _unitOfWork.GetRepository<Item>().GetByIdAsync(item.Id);
                Assert.NotNull(updatedItem);
                Assert.Null(updatedItem.CategoryId);
            }
        }

        [Fact]
        public async Task Get_Categories_With_Items_Should_Return_Categories_With_Items_Assigned_Test()
        {
            // Set up filter
            Expression<Func<Category, bool>> filter = i => i.Items != null && i.Items.Count > 0;

            Func<IQueryable<Category>, IOrderedQueryable<Category>> orderBy = q => q.OrderBy(i => i.Name);

            // Set up includeProperties
            string includeProperties = "Items";

            // Set up pagination
            int page = 1;
            int pageSize = 10;

            var result = await _unitOfWork.GetRepository<Category>().GetAllAsync(
                filter,
                orderBy,
                includeProperties,
                page,
                pageSize
            );

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.IsType<List<Category>>(result);
            Assert.True(result.Count <= 10);

            foreach (var category in result)
            {
                Assert.NotNull(category);
                Assert.NotNull(category.Name);
                Assert.NotNull(category.Items);
            }
        }

        // Helper method to return test categories
        private List<Category> GetTestCategories()
        {
            return new List<Category> {
                new Category { Name = "Computers", Description="Different computers"},
                new Category { Name = "Camera & Security", Description = "Various cameras and security systems"},
                new Category { Name = "Data Storage", Description = "Data storage devices"},
                new Category { Name = "Monitors & Accessories", Description = "Monitors, Headphones and Headsets, Monitor Parts & Accessories"}
            };
        }

        // Dispose DbContext after each test
        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
