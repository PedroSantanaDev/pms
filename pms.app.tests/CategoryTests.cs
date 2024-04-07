using pms.app.Models;

namespace pms.app.tests
{
    public class CategoryTests
    {
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
    }
}
