using CavalierContoursSharp;

namespace CavalierContoursSharpTests;

public class BooleanOpTests
{
    [Fact]
    public void EnumerationValuesAreCorrect()
    {
        // Assert
        Assert.Equal(0, (int)BooleanOp.Or);
        Assert.Equal(1, (int)BooleanOp.And);
        Assert.Equal(2, (int)BooleanOp.Not);
        Assert.Equal(3, (int)BooleanOp.Xor);
    }
}
