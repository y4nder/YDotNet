using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Repository;
using System.Linq.Expressions;

namespace YanderTests;

// Test entity class
public class TestEntity : IEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

// Test repository implementation
public class TestRepository : Repository<TestEntity, int>
{
    public TestRepository(DbContext context) : base(context)
    {
    }
}

public class RepositoryTest
{
    private readonly Mock<DbContext> _mockContext;
    private readonly Mock<DbSet<TestEntity>> _mockSet;
    private readonly TestRepository _repository;
    private readonly List<TestEntity> _data;

    #pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public RepositoryTest()
    {
        try
        {
            _data = new List<TestEntity>
            {
                new TestEntity { Id = 1, Name = "Test1" },
                new TestEntity { Id = 2, Name = "Test2" }
            };

            _mockSet = new Mock<DbSet<TestEntity>>();
            _mockContext = new Mock<DbContext>();

            var queryable = _data.AsQueryable();

            // Setup mock DbSet
            _mockSet.As<IQueryable<TestEntity>>()
                .Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<TestEntity>(queryable.Provider));
            _mockSet.As<IQueryable<TestEntity>>()
                .Setup(m => m.Expression)
                .Returns(queryable.Expression);
            _mockSet.As<IQueryable<TestEntity>>()
                .Setup(m => m.ElementType)
                .Returns(queryable.ElementType);
            _mockSet.As<IQueryable<TestEntity>>()
                .Setup(m => m.GetEnumerator())
                .Returns(queryable.GetEnumerator());

            // Setup async enumerable
            _mockSet.As<IAsyncEnumerable<TestEntity>>()
                .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(new TestAsyncEnumerator<TestEntity>(_data.GetEnumerator()));

            _mockContext.Setup(c => c.Set<TestEntity>()).Returns(_mockSet.Object);
            _repository = new TestRepository(_mockContext.Object);
        }
        catch (System.Exception ex)
        {
            Console.WriteLine($"Test setup failed: {ex.Message}");
        }
    }

    // Helper classes for async operations
    internal class TestAsyncQueryProvider<TEntity> : IAsyncQueryProvider
    {
        private readonly IQueryProvider _inner;

        internal TestAsyncQueryProvider(IQueryProvider inner)
        {
            _inner = inner;
        }

        public IQueryable CreateQuery(Expression expression)
        {
            return new TestAsyncEnumerable<TEntity>(expression);
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new TestAsyncEnumerable<TElement>(expression);
        }

        public object? Execute(Expression expression)
        {
            return _inner.Execute(expression);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            return _inner.Execute<TResult>(expression);
        }

        public IAsyncEnumerable<TResult> ExecuteAsync<TResult>(Expression expression)
        {
            return new TestAsyncEnumerable<TResult>(expression);
        }

        public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
        {
            return Execute<TResult>(expression);
        }
    }

    internal class TestAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
    {
        public TestAsyncEnumerable(IEnumerable<T> enumerable)
            : base(enumerable)
        { }

        public TestAsyncEnumerable(Expression expression)
            : base(expression)
        { }

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return new TestAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
        }
    }

    internal class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _inner;

        public TestAsyncEnumerator(IEnumerator<T> inner)
        {
            _inner = inner;
        }

        public T Current => _inner.Current;

        public ValueTask<bool> MoveNextAsync()
        {
            return new ValueTask<bool>(_inner.MoveNext());
        }

        public ValueTask DisposeAsync()
        {
            _inner.Dispose();
            return new ValueTask();
        }
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ReturnsEntity()
    {
        // Arrange
        var testEntity = _data[0];
        _mockSet.Setup(m => m.FindAsync(testEntity.Id))
            .ReturnsAsync(testEntity);

        // Act
        var result = await _repository.GetByIdAsync(testEntity.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(testEntity.Id, result.Id);
        Assert.Equal(testEntity.Name, result.Name);
    }

    [Fact]
    public async Task GetByIdAsync_NonExistingId_ReturnsNull()
    {
        // Arrange
        _mockSet.Setup(m => m.FindAsync(999))
            .ReturnsAsync(null as TestEntity);

        // Act
        var result = await _repository.GetByIdAsync(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void Add_ValidEntity_CallsAddOnDbSet()
    {
        // Arrange
        var entity = new TestEntity { Id = 3, Name = "Test3" };

        // Act
        _repository.Add(entity);

        // Assert
        _mockSet.Verify(m => m.Add(entity), Times.Once);
    }

    [Fact]
    public void Update_ValidEntity_CallsUpdateOnDbSet()
    {
        // Arrange
        var entity = new TestEntity { Id = 1, Name = "Updated Test1" };

        // Act
        _repository.Update(entity);

        // Assert
        _mockSet.Verify(m => m.Update(entity), Times.Once);
    }

    [Fact]
    public void Delete_ValidEntity_CallsRemoveOnDbSet()
    {
        // Arrange
        var entity = _data[0];

        // Act
        _repository.Delete(entity);

        // Assert
        _mockSet.Verify(m => m.Remove(entity), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllEntities()
    {
        // Act
        var resultList = await _repository.GetAllAsync();

        // Assert
        Assert.NotNull(resultList);
        Assert.Equal(_data.Count, resultList.Count);
        Assert.Collection(resultList,
            item => Assert.Equal(_data[0].Id, item.Id),
            item => Assert.Equal(_data[1].Id, item.Id));
    }
} 