using CavalierContoursSharp;

namespace CavalierContoursSharpTests;

public class CavcAabbIndexTests
{
    [Fact]
    public void CanCreateAndDispose()
    {
        // Arrange
        using var polyline = new Polyline();
        // Act
        var index = polyline.CreateApproximateAabbIndex();
        // Assert
        Assert.NotNull(index);
    }

    [Fact]
    public void CanGetExtents()
    {
        // Arrange
        using var polyline = new Polyline();
        polyline.AddVertex(0, 0);
        polyline.AddVertex(10, 0);
        polyline.AddVertex(10, 10);
        polyline.AddVertex(0, 10);
        polyline.IsClosed = true;

        var index = polyline.CreateAabbIndex();

        // Act
        var result = index.GetExtents(
            out double minX,
            out double minY,
            out double maxX,
            out double maxY
        );

        // Assert
        Assert.True(result);
        Assert.Equal(0, minX, 6);
        Assert.Equal(0, minY, 6);
        Assert.Equal(10, maxX, 6);
        Assert.Equal(10, maxY, 6);
    }
}
