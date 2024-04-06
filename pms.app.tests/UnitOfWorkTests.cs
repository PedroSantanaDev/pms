using Moq;
using pms.app.Data;

namespace pms.app.tests
{

    public class UnitOfWorkTests
    {
        private readonly Mock<ApplicationDbContext> mockDbContext;
        private readonly UnitOfWork.UnitOfWork unitOfWork;

        public UnitOfWorkTests()
        {
            mockDbContext = new Mock<ApplicationDbContext>();
            unitOfWork = new UnitOfWork.UnitOfWork(mockDbContext.Object);
        }

        [Fact]
        public void SaveChangesTest()
        {
            unitOfWork.SaveChanges();

            // Assert
            mockDbContext.Verify(m => m.SaveChanges(), Times.Once);
        }
    }
}
