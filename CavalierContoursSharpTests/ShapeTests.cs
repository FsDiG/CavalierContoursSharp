using CavalierContoursSharp;
using Xunit.Abstractions;

namespace CavalierContoursSharpTests;

public class ShapeTests
{
    private readonly ITestOutputHelper _output;

    public ShapeTests(ITestOutputHelper output)
    {
        _output = output;
    }

    private static PolylineList CreateTestPolylineList()
    {
        // Create a simple rectangle
        var vertices = new CavcVertex[]
        {
            new CavcVertex(0, 0),
            new CavcVertex(10, 0),
            new CavcVertex(10, 10),
            new CavcVertex(0, 10)
        };

        using var polyline = new Polyline(vertices, true);
        var (positive, _) = polyline.BooleanOperation(polyline, BooleanOp.Or);
        return positive;
    }

    [Fact]
    public void CanCreateShapeFromPolylineList()
    {
        // Arrange
        using var polylineList = CreateTestPolylineList();

        // Act
        using var shape = new Shape(polylineList);

        // Assert
        Assert.NotNull(shape);
        Assert.True(shape.CcwCount >= 0);
        Assert.True(shape.CwCount >= 0);
    }

    [Fact]
    public void ThrowsArgumentNullExceptionForNullPolylineList()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new Shape(null));
    }

    [Fact]
    public void ThrowsObjectDisposedExceptionForDisposedPolylineList()
    {
        // Arrange
        var polylineList = CreateTestPolylineList();
        polylineList.Dispose();

        // Act & Assert
        Assert.Throws<ObjectDisposedException>(() => new Shape(polylineList));
    }

    [Fact]
    public void CanGetCcwAndCwCounts()
    {
        // Arrange
        using var polylineList = CreateTestPolylineList();
        using var shape = new Shape(polylineList);

        // Act
        var ccwCount = shape.CcwCount;
        var cwCount = shape.CwCount;

        // Assert
        Assert.True(ccwCount >= 0);
        Assert.True(cwCount >= 0);
        _output.WriteLine($"CCW Count: {ccwCount}, CW Count: {cwCount}");
    }

    [Fact]
    public void CanGetCcwPolylineVertexCount()
    {
        // Arrange
        using var polylineList = CreateTestPolylineList();
        using var shape = new Shape(polylineList);

        // Act & Assert
        if (shape.CcwCount > 0)
        {
            var vertexCount = shape.GetCcwPolylineVertexCount(0);
            Assert.True(vertexCount > 0);
            _output.WriteLine($"CCW Polyline 0 vertex count: {vertexCount}");
        }
    }

    [Fact]
    public void CanCheckCcwPolylineClosedState()
    {
        // Arrange
        using var polylineList = CreateTestPolylineList();
        using var shape = new Shape(polylineList);

        // Act & Assert
        if (shape.CcwCount > 0)
        {
            var isClosed = shape.IsCcwPolylineClosed(0);
            _output.WriteLine($"CCW Polyline 0 is closed: {isClosed}");
        }
    }

    [Fact]
    public void CanGetCcwPolylineVertices()
    {
        // Arrange
        using var polylineList = CreateTestPolylineList();
        using var shape = new Shape(polylineList);

        // Act & Assert
        if (shape.CcwCount > 0)
        {
            var vertices = shape.GetCcwPolylineVertices(0);
            Assert.NotNull(vertices);
            Assert.True(vertices.Length > 0);
            _output.WriteLine($"CCW Polyline 0 has {vertices.Length} vertices");

            // Log first vertex for debugging
            if (vertices.Length > 0)
            {
                _output.WriteLine(
                    $"First vertex: ({vertices[0].X}, {vertices[0].Y}, bulge={vertices[0].Bulge})"
                );
            }
        }
    }

    [Fact]
    public void CanSetAndGetCcwPolylineUserdata()
    {
        // Arrange
        using var polylineList = CreateTestPolylineList();
        using var shape = new Shape(polylineList);

        if (shape.CcwCount > 0)
        {
            var testUserdata = new ulong[] { 123, 456, 789 };

            // Act
            shape.SetCcwPolylineUserdata(0, testUserdata);
            var retrievedUserdata = shape.GetCcwPolylineUserdata(0);

            // Assert
            Assert.Equal(testUserdata.Length, retrievedUserdata.Length);
            for (int i = 0; i < testUserdata.Length; i++)
            {
                Assert.Equal(testUserdata[i], retrievedUserdata[i]);
            }
        }
    }

    [Fact]
    public void CanGetCwPolylineVertexCount()
    {
        // Arrange
        using var polylineList = CreateTestPolylineList();
        using var shape = new Shape(polylineList);

        // Act & Assert
        if (shape.CwCount > 0)
        {
            var vertexCount = shape.GetCwPolylineVertexCount(0);
            Assert.True(vertexCount > 0);
            _output.WriteLine($"CW Polyline 0 vertex count: {vertexCount}");
        }
    }

    [Fact]
    public void CanCheckCwPolylineClosedState()
    {
        // Arrange
        using var polylineList = CreateTestPolylineList();
        using var shape = new Shape(polylineList);

        // Act & Assert
        if (shape.CwCount > 0)
        {
            var isClosed = shape.IsCwPolylineClosed(0);
            _output.WriteLine($"CW Polyline 0 is closed: {isClosed}");
        }
    }

    [Fact]
    public void CanGetCwPolylineVertices()
    {
        // Arrange
        using var polylineList = CreateTestPolylineList();
        using var shape = new Shape(polylineList);

        // Act & Assert
        if (shape.CwCount > 0)
        {
            var vertices = shape.GetCwPolylineVertices(0);
            Assert.NotNull(vertices);
            Assert.True(vertices.Length > 0);
            _output.WriteLine($"CW Polyline 0 has {vertices.Length} vertices");

            // Log first vertex for debugging
            if (vertices.Length > 0)
            {
                _output.WriteLine(
                    $"First vertex: ({vertices[0].X}, {vertices[0].Y}, bulge={vertices[0].Bulge})"
                );
            }
        }
    }

    [Fact]
    public void CanSetAndGetCwPolylineUserdata()
    {
        // Arrange
        using var polylineList = CreateTestPolylineList();
        using var shape = new Shape(polylineList);

        if (shape.CwCount > 0)
        {
            var testUserdata = new ulong[] { 987, 654, 321 };

            // Act
            shape.SetCwPolylineUserdata(0, testUserdata);
            var retrievedUserdata = shape.GetCwPolylineUserdata(0);

            // Assert
            Assert.Equal(testUserdata.Length, retrievedUserdata.Length);
            for (int i = 0; i < testUserdata.Length; i++)
            {
                Assert.Equal(testUserdata[i], retrievedUserdata[i]);
            }
        }
    }

    [Fact]
    public void CanPerformParallelOffset()
    {
        // Arrange
        using var polylineList = CreateTestPolylineList();
        using var shape = new Shape(polylineList);

        // Act
        using var offsetShape = shape.ParallelOffset(2.0);

        // Assert
        Assert.NotNull(offsetShape);
        _output.WriteLine($"Original shape - CCW: {shape.CcwCount}, CW: {shape.CwCount}");
        _output.WriteLine($"Offset shape - CCW: {offsetShape.CcwCount}, CW: {offsetShape.CwCount}");
    }

    [Fact]
    public void CanPerformParallelOffsetWithOptions()
    {
        // Arrange
        using var polylineList = CreateTestPolylineList();
        using var shape = new Shape(polylineList);

        var options = new CavcShapeOffsetOptions
        {
            pos_equal_eps = 1e-9,
            slice_join_eps = 1e-9,
            offset_dist_eps = 1e-9
        };

        // Act
        using var offsetShape = shape.ParallelOffset(-1.0, options);

        // Assert
        Assert.NotNull(offsetShape);
        _output.WriteLine($"Original shape - CCW: {shape.CcwCount}, CW: {shape.CwCount}");
        _output.WriteLine(
            $"Inward offset shape - CCW: {offsetShape.CcwCount}, CW: {offsetShape.CwCount}"
        );
    }

    [Fact]
    public void ThrowsWhenAccessingOutOfBoundsCcwPolyline()
    {
        // Arrange
        using var polylineList = CreateTestPolylineList();
        using var shape = new Shape(polylineList);

        uint invalidIndex = (uint)shape.CcwCount + 10;

        // Act & Assert
        Assert.Throws<InvalidOperationException>(
            () => shape.GetCcwPolylineVertexCount(invalidIndex)
        );
        Assert.Throws<InvalidOperationException>(() => shape.IsCcwPolylineClosed(invalidIndex));
        Assert.Throws<InvalidOperationException>(() => shape.GetCcwPolylineVertices(invalidIndex));
    }

    [Fact]
    public void ThrowsWhenAccessingOutOfBoundsCwPolyline()
    {
        // Arrange
        using var polylineList = CreateTestPolylineList();
        using var shape = new Shape(polylineList);

        uint invalidIndex = (uint)shape.CwCount + 10;

        // Act & Assert
        Assert.Throws<InvalidOperationException>(
            () => shape.GetCwPolylineVertexCount(invalidIndex)
        );
        Assert.Throws<InvalidOperationException>(() => shape.IsCwPolylineClosed(invalidIndex));
        Assert.Throws<InvalidOperationException>(() => shape.GetCwPolylineVertices(invalidIndex));
    }

    [Fact]
    public void ThrowsObjectDisposedExceptionWhenDisposed()
    {
        // Arrange
        using var polylineList = CreateTestPolylineList();
        var shape = new Shape(polylineList);
        shape.Dispose();

        // Act & Assert
        Assert.Throws<ObjectDisposedException>(() => shape.CcwCount);
        Assert.Throws<ObjectDisposedException>(() => shape.CwCount);
        Assert.Throws<ObjectDisposedException>(() => shape.GetCcwPolylineVertexCount(0));
        Assert.Throws<ObjectDisposedException>(() => shape.ParallelOffset(1.0));
    }

    [Fact]
    public void ToStringReturnsValidRepresentation()
    {
        // Arrange
        using var polylineList = CreateTestPolylineList();
        using var shape = new Shape(polylineList);

        // Act
        var stringRepresentation = shape.ToString();

        // Assert
        Assert.NotNull(stringRepresentation);
        Assert.Contains("Shape", stringRepresentation);
        Assert.Contains("CcwCount", stringRepresentation);
        Assert.Contains("CwCount", stringRepresentation);
        _output.WriteLine($"Shape string representation: {stringRepresentation}");
    }

    [Fact]
    public void DisposedShapeToStringReturnsDisposedMessage()
    {
        // Arrange
        using var polylineList = CreateTestPolylineList();
        var shape = new Shape(polylineList);
        shape.Dispose();

        // Act
        var stringRepresentation = shape.ToString();

        // Assert
        Assert.Equal("Shape [Disposed]", stringRepresentation);
    }
}
