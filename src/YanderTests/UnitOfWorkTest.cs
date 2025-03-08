using Microsoft.EntityFrameworkCore;
using Moq;

namespace YanderTests;

public class UnitOfWorkTest
{
    [Fact]
    public async Task SaveChangesAsync_ShouldCallContextSaveChanges()
    {
        // Arrange
        var mockContext = new Mock<DbContext>();
        mockContext.Setup(c => c.SaveChangesAsync(default))
            .ReturnsAsync(1)
            .Verifiable();

        var unitOfWork = new UnitOfWork.UnitOfWork(mockContext.Object);

        // Act
        var result = await unitOfWork.SaveChangesAsync();

        // Assert
        Assert.Equal(1, result);
        mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task SaveChangesAsync_WhenContextThrows_ShouldPropagateException()
    {
        // Arrange
        var mockContext = new Mock<DbContext>();
        var expectedException = new DbUpdateException("Test exception");
        mockContext.Setup(c => c.SaveChangesAsync(default))
            .ThrowsAsync(expectedException);

        var unitOfWork = new UnitOfWork.UnitOfWork(mockContext.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<DbUpdateException>(
            () => unitOfWork.SaveChangesAsync());
        
        Assert.Same(expectedException, exception);
    }
} 