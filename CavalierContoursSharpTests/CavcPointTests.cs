using CavalierContoursSharp;

namespace CavalierContoursSharpTests;

public class CavcPointTests
{
    [Fact]
    public void CanCreateCavcPoint()
    {
        // Arrange & Act
        var point = new CavcPoint(10.5, 20.5);

        // Assert
        Assert.Equal(10.5, point.X);
        Assert.Equal(20.5, point.Y);
    }

    [Fact]
    public void CanConvertToVector2()
    {
        // Arrange
        var point = new CavcPoint(10, 20);

        // Act
        var vector = point.ToVector2();

        // Assert
        Assert.Equal(10f, vector.X, 6);
        Assert.Equal(20f, vector.Y, 6);
    }
}
