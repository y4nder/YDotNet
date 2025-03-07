using Microsoft.AspNetCore.Http.HttpResults;
using ResultType;
using ResultType.Errors;

namespace YanderTests;

public class ResultTypeTest
{
    [Fact]
    public void ResultSuccessShouldReturnTrue()
    {
        var result = Result.Success();
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void ResultFailureShouldReturnFalse()
    {
        var error = new Error("some.error", "some.error", 123);
        var result = Result.Failure(error);
        Assert.False(result.IsSuccess);
        Assert.Equal(error, result.Error);
    }

    [Fact]
    public void Success_Generic_ShouldReturnSuccessResultWithValue()
    {        
        var result = Result.Success(42);
        Assert.True(result.IsSuccess);
        Assert.Equal(42, result.Value);
    }
    
    [Fact]
    public void HttpResult_ShouldReturnOk_WhenResultIsSuccess()
    {
        // Arrange
        var result = Result.Success(42);

        // Act
        var materialized = result.ToHttpResult();

        // Assert
        var okResult = Assert.IsType<Ok<int>>(materialized);
        Assert.Equal(42, okResult.Value);
    }
    
    [Fact]
    public void HttpResult_ShouldReturnConflict_WhenResultHas409Error()
    {
        // Arrange
        var error = new Error(
            "Conflict", 
            "Data conflict occurred.", 
            409);
        var result = Result.Failure<int>(error);

        // Act
        var httpResult = result.ToHttpResult();
        
        // Assert
        Assert.IsType<Conflict<IError>>(httpResult);
    }
    
}