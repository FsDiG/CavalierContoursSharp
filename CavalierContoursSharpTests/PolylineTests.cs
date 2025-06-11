using CavalierContoursSharp;
using Xunit.Abstractions;

namespace CavalierContoursSharpTests;

public class PolylineTests
{
    private readonly ITestOutputHelper _output;

    public PolylineTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void CanCreateEmptyPolyline()
    {
        // Act
        using var polyline = new Polyline();
        // Assert
        Assert.False(polyline.IsClosed);
        Assert.Equal(0, polyline.VertexCount);
    }

    [Fact]
    public void CanCreatePolylineWithVertices()
    {
        // Arrange
        var vertices = new[]
        {
            new CavcVertex(0, 0, 0),
            new CavcVertex(10, 0, 0),
            new CavcVertex(10, 10, 0)
        };

        // Act
        using var polyline = new Polyline(vertices, true);
        // Assert
        Assert.True(polyline.IsClosed);
        Assert.Equal(3, polyline.VertexCount);

        var v0 = polyline.GetVertex(0);
        Assert.Equal(0, v0.X);
        Assert.Equal(0, v0.Y);
        Assert.Equal(0, v0.Bulge);
    }

    [Fact]
    public void CanAddVertex()
    {
        // Arrange
        using var polyline = new Polyline();
        // Act
        polyline.AddVertex(10, 20, 0.5);

        // Assert
        Assert.Equal(1, polyline.VertexCount);

        var vertex = polyline.GetVertex(0);
        Assert.Equal(10, vertex.X);
        Assert.Equal(20, vertex.Y);
        Assert.Equal(0.5, vertex.Bulge);
    }

    [Fact]
    public void CanGetAndSetVertex()
    {
        // Arrange
        using var polyline = new Polyline();
        polyline.AddVertex(10, 20);

        // Act
        var vertex = polyline.GetVertex(0);
        vertex.X = 30;
        vertex.Y = 40;
        polyline.SetVertex(0, vertex);

        // Assert
        var updatedVertex = polyline.GetVertex(0);
        Assert.Equal(30, updatedVertex.X);
        Assert.Equal(40, updatedVertex.Y);
    }

    [Fact]
    public void CanRemoveVertex()
    {
        // Arrange
        using var polyline = new Polyline();
        polyline.AddVertex(10, 20);
        polyline.AddVertex(30, 40);

        // Act
        polyline.RemoveVertex(0);

        // Assert
        Assert.Equal(1, polyline.VertexCount);
        var vertex = polyline.GetVertex(0);
        Assert.Equal(30, vertex.X);
        Assert.Equal(40, vertex.Y);
    }

    [Fact]
    public void CanGetAndSetVertices()
    {
        // Arrange
        using var polyline = new Polyline();
        var vertices = new[] { new CavcVertex(0, 0), new CavcVertex(10, 0) };

        // Act
        polyline.SetVertices(vertices);

        // Assert
        Assert.Equal(2, polyline.VertexCount);
        var resultVertices = polyline.GetVertices();
        Assert.Equal(2, resultVertices.Length);
        Assert.Equal(0, resultVertices[0].X);
        Assert.Equal(0, resultVertices[0].Y);
        Assert.Equal(10, resultVertices[1].X);
        Assert.Equal(0, resultVertices[1].Y);
    }

    [Fact]
    public void CanClearPolyline()
    {
        // Arrange
        using var polyline = new Polyline();
        polyline.AddVertex(10, 20);

        // Act
        polyline.Clear();

        // Assert
        Assert.Equal(0, polyline.VertexCount);
    }

    [Fact]
    public void CanInvertDirection()
    {
        // Arrange
        using Polyline polyline = new();
        polyline.AddVertex(0, 0);
        polyline.AddVertex(10, 0);
        polyline.AddVertex(10, 10);

        // Act
        polyline.InvertDirection();

        // Assert
        var v0 = polyline.GetVertex(0);
        Assert.Equal(10, v0.X);
        Assert.Equal(10, v0.Y);

        var v2 = polyline.GetVertex(2);
        Assert.Equal(0, v2.X);
        Assert.Equal(0, v2.Y);
    }

    [Fact]
    public void CanScalePolyline()
    {
        // Arrange
        using var polyline = new Polyline();
        polyline.AddVertex(0, 0);
        polyline.AddVertex(10, 0);

        // Act
        polyline.Scale(2);

        // Assert
        var v0 = polyline.GetVertex(0);
        Assert.Equal(0, v0.X);
        Assert.Equal(0, v0.Y);

        var v1 = polyline.GetVertex(1);
        Assert.Equal(20, v1.X);
        Assert.Equal(0, v1.Y);
    }

    [Fact]
    public void CanTranslatePolyline()
    {
        // Arrange
        using Polyline polyline = new();
        polyline.AddVertex(0, 0);
        polyline.AddVertex(10, 0);

        // Act
        polyline.Translate(5, 5);

        // Assert
        var v0 = polyline.GetVertex(0);
        Assert.Equal(5, v0.X);
        Assert.Equal(5, v0.Y);

        var v1 = polyline.GetVertex(1);
        Assert.Equal(15, v1.X);
        Assert.Equal(5, v1.Y);
    }

    [Fact]
    public void CanRemoveRepeatPositions()
    {
        // Arrange
        using var polyline = new Polyline();
        polyline.AddVertex(0, 0);
        polyline.AddVertex(0, 0); // Duplicate vertex
        polyline.AddVertex(10, 0);

        // Act
        polyline.RemoveRepeatPositions(0.001);

        // Assert
        Assert.Equal(2, polyline.VertexCount);
    }

    [Fact]
    public void CanCalculateWindingNumber()
    {
        // Arrange - Simple square
        using var polyline = new Polyline(
            [
                new CavcVertex(0, 0),
                new CavcVertex(10, 0),
                new CavcVertex(10, 10),
                new CavcVertex(0, 10)
            ],
            true
        );
        // Act - Inside the square
        int wn1 = polyline.GetWindingNumber(5, 5);

        // Act - Outside the square
        int wn2 = polyline.GetWindingNumber(20, 20);

        // Assert
        Assert.Equal(1, wn1);
        Assert.Equal(0, wn2);
    }

    [Fact]
    public void CanGetExtents()
    {
        // Arrange
        using var polyline = new Polyline(
            [
                new CavcVertex(0, 0),
                new CavcVertex(10, 0),
                new CavcVertex(10, 10),
                new CavcVertex(0, 10)
            ],
            true
        );
        // Act
        bool result = polyline.GetExtents(
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

    [Fact]
    public void CanPerformParallelOffset()
    {
        // Arrange - Simple square
        using var polyline = new Polyline(
            [
                new CavcVertex(0, 0),
                new CavcVertex(10, 0),
                new CavcVertex(10, 10),
                new CavcVertex(0, 10)
            ],
            true
        );
        // Act
        var offsetResult = polyline.ParallelOffset(1.0);

        // Assert
        Assert.True(offsetResult.Count > 0);
    }

    [Fact]
    public void CanCreateAabbIndex()
    {
        // Arrange
        using var polyline = new Polyline(
            [
                new CavcVertex(0, 0),
                new CavcVertex(10, 0),
                new CavcVertex(10, 10),
                new CavcVertex(0, 10)
            ],
            true
        );
        // Act
        var index1 = polyline.CreateApproximateAabbIndex();
        var index2 = polyline.CreateAabbIndex();

        // Assert
        Assert.NotNull(index1);
        Assert.NotNull(index2);
    }

    [Fact]
    public void CanEnumerateVertices()
    {
        // Arrange
        using Polyline polyline = new Polyline();
        polyline.AddVertex(0, 0);
        polyline.AddVertex(10, 0);
        polyline.AddVertex(10, 10);

        // Act & Assert
        int count = 0;
        foreach (var vertex in polyline)
        {
            count++;
            _output.WriteLine($"Vertex {count}: ({vertex.X}, {vertex.Y})");
        }

        Assert.Equal(3, count);
    }
}
