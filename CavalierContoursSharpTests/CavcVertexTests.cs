using CavalierContoursSharp;

namespace CavalierContoursSharpTests;

public class CavcVertexTests
{
    [Fact]
    public void CanCreateCavcVertex()
    {
        // Arrange & Act
        var vertex = new CavcVertex(10, 20, 0.5);

        // Assert
        Assert.Equal(10, vertex.X);
        Assert.Equal(20, vertex.Y);
        Assert.Equal(0.5, vertex.Bulge);
    }
}
