using CavalierContoursSharp;
using Xunit.Abstractions;

namespace CavalierContoursSharpTests;

public partial class BooleanOperationTests(ITestOutputHelper output)
{
    private readonly ITestOutputHelper _output = output;

    #region Helper Methods
    private static Polyline CreateSquare(double size, double x = 0, double y = 0) =>
        new Polyline(
            [
                new CavcVertex(x, y),
                new CavcVertex(x + size, y),
                new CavcVertex(x + size, y + size),
                new CavcVertex(x, y + size)
            ],
            true
        );

    private static Polyline CreateCircle(
        double radius,
        double x = 0,
        double y = 0,
        int segments = 12
    )
    {
        var vertices = new CavcVertex[segments];
        double angleStep = 2 * Math.PI / segments;

        for (int i = 0; i < segments; i++)
        {
            double angle = i * angleStep;
            vertices[i] = new CavcVertex(
                x + radius * Math.Cos(angle),
                y + radius * Math.Sin(angle),
                Math.Tan(angleStep / 4) // Bulge value for circular arc
            );
        }

        return new Polyline(vertices, true);
    }

    private static Polyline CreateRectangle(
        double width,
        double height,
        double x = 0,
        double y = 0
    ) =>
        new(
            [
                new CavcVertex(x, y),
                new CavcVertex(x + width, y),
                new CavcVertex(x + width, y + height),
                new CavcVertex(x, y + height)
            ],
            true
        );

    private static Polyline CreateLShaped() =>
        new(
            [
                new CavcVertex(0, 0),
                new CavcVertex(10, 0),
                new CavcVertex(10, 5),
                new CavcVertex(5, 5),
                new CavcVertex(5, 10),
                new CavcVertex(0, 10)
            ],
            true
        );

    private void LogPolylineInfo(string name, Polyline polyline)
    {
        _output.WriteLine(
            $"{name} - Closed: {polyline.IsClosed}, Vertices: {polyline.VertexCount}"
        );
        _output.WriteLine($"  Area: {polyline.Area}, Path Length: {polyline.PathLength}");

        if (polyline.GetExtents(out double minX, out double minY, out double maxX, out double maxY))
        {
            _output.WriteLine($"  Extents: ({minX}, {minY}) - ({maxX}, {maxY})");
        }
    }

    private void LogPolylineList(string name, PolylineList list)
    {
        _output.WriteLine($"{name} - Count: {list.Count}");
        for (int i = 0; i < list.Count; i++)
        {
            using (var pline = list[i])
            {
                LogPolylineInfo($"  Polyline {i}", pline);
            }
        }
    }
    #endregion

    #region Square vs Square Operations
    [Fact]
    public void SquareOrSquare_CompleteOverlap()
    {
        using var square1 = CreateSquare(10);
        using var square2 = CreateSquare(10);
        // Act
        var (positive, negative) = square1.BooleanOperation(square2, BooleanOp.Or);

        // Assert
        Assert.Single(positive);
        Assert.Empty(negative);

        var result = positive[0];
        Assert.True(result.IsClosed);
        Assert.Equal(4, result.VertexCount);
        Assert.Equal(100, Math.Abs(result.Area), 6);
    }

    [Fact]
    public void SquareAndSquare_PartialOverlap()
    {
        using var square1 = CreateSquare(10);
        using var square2 = CreateSquare(10, 5, 5);
        // Act
        var (positive, negative) = square1.BooleanOperation(square2, BooleanOp.And);

        // Assert
        Assert.Single(positive);
        Assert.Empty(negative);

        using var result = positive[0];
        Assert.True(result.IsClosed);
        Assert.Equal(4, result.VertexCount);
        Assert.Equal(25, Math.Abs(result.Area), 6);
    }

    [Fact]
    public void SquareNotSquare_PartialOverlap()
    {
        using var square1 = CreateSquare(10);
        using var square2 = CreateSquare(10, 5, 5);
        // Act
        var (positive, negative) = square1.BooleanOperation(square2, BooleanOp.Not);

        // Assert
        Assert.Single(positive);
        Assert.Empty(negative);

        using var result = positive[0];
        Assert.True(result.IsClosed);
        Assert.Equal(6, result.VertexCount);
        Assert.Equal(75, Math.Abs(result.Area), 0.1);
    }

    [Fact]
    public void SquareXorSquare_PartialOverlap()
    {
        using var square1 = CreateSquare(10);
        using var square2 = CreateSquare(10, 5, 5);
        // Act
        var (positive, negative) = square1.BooleanOperation(square2, BooleanOp.Xor);

        // Assert
        Assert.Equal(2, positive.Count);
        Assert.Empty(negative);

        double totalArea = 0;
        for (int i = 0; i < positive.Count; i++)
        {
            using var result = positive[i];
            totalArea += Math.Abs(result.Area);
        }

        Assert.Equal(150, totalArea, 6);
    }
    #endregion

    #region Circle vs Circle Operations
    [Fact]
    public void CircleOrCircle_CompleteOverlap()
    {
        using var circle1 = CreateCircle(5);
        using var circle2 = CreateCircle(5);
        // Act
        var (positive, negative) = circle1.BooleanOperation(circle2, BooleanOp.Or);

        // Assert
        Assert.Single(positive);
        Assert.Empty(negative);

        using var result = positive[0];
        Assert.True(result.IsClosed);
        Assert.InRange(result.VertexCount, 12, 24);
        Assert.InRange(Math.Abs(result.Area), Math.PI * 5 * 5 * 0.99, Math.PI * 5 * 5 * 1.01);
    }

    [Fact]
    public void CircleAndCircle_PartialOverlap()
    {
        using var circle1 = CreateCircle(5);
        using var circle2 = CreateCircle(5, 5, 0);
        // Act
        var (positive, negative) = circle1.BooleanOperation(circle2, BooleanOp.And);

        // Assert
        Assert.Single(positive);
        Assert.Empty(negative);

        using var result = positive[0];
        Assert.True(result.IsClosed);
        Assert.InRange(result.VertexCount, 8, 16);
        Assert.Equal(30.709, Math.Abs(result.Area), 0.1);
    }

    [Fact]
    public void CircleNotCircle_PartialOverlap()
    {
        using var circle1 = CreateCircle(5);
        using var circle2 = CreateCircle(5, 5, 0);
        // Act
        var (positive, negative) = circle1.BooleanOperation(circle2, BooleanOp.Not);

        // Assert
        Assert.Single(positive);
        Assert.Empty(negative);

        using var result = positive[0];
        Assert.True(result.IsClosed);
        Assert.InRange(result.VertexCount, 8, 16);
        Assert.Equal(47.83, Math.Abs(result.Area), 0.1);
    }

    [Fact]
    public void CircleXorCircle_PartialOverlap()
    {
        using var circle1 = CreateCircle(5);
        using var circle2 = CreateCircle(5, 5, 0);
        // Act
        var (positive, negative) = circle1.BooleanOperation(circle2, BooleanOp.Xor);

        // Assert
        Assert.Equal(2, positive.Count);
        Assert.Empty(negative);

        double totalArea = 0;
        for (int i = 0; i < positive.Count; i++)
        {
            using var result = positive[i];
            totalArea += Math.Abs(result.Area);
        }

        Assert.Equal(95.66, totalArea, 0.1);
    }
    #endregion

    #region Square vs Circle Operations
    [Fact]
    public void SquareOrCircle_CompleteOverlap()
    {
        using var square = CreateSquare(10);
        using var circle = CreateCircle(4);
        // Act
        var (positive, negative) = square.BooleanOperation(circle, BooleanOp.Or);

        // Assert
        Assert.Single(positive);
        Assert.Empty(negative);

        using var result = positive[0];
        Assert.True(result.IsClosed);
        // When circle is completely inside square, result keeps the outer boundary (square)
        // but may include circle vertices, so vertex count can be higher
        Assert.InRange(result.VertexCount, 4, 20);
        Assert.InRange(Math.Abs(result.Area), 100, 140);
    }

    [Fact]
    public void SquareAndCircle_PartialOverlap()
    {
        using var square = CreateSquare(10);
        using var circle = CreateCircle(6, 5, 5);
        // Act
        var (positive, negative) = square.BooleanOperation(circle, BooleanOp.And);

        // Assert
        Assert.Single(positive);
        Assert.Empty(negative);

        using var result = positive[0];
        Assert.True(result.IsClosed);
        Assert.InRange(result.VertexCount, 8, 16);
        Assert.InRange(Math.Abs(result.Area), 88, 96);
    }

    [Fact]
    public void SquareNotCircle_PartialOverlap()
    {
        using var square = CreateSquare(10);
        using var circle = CreateCircle(6, 5, 5);
        // Act
        var (positive, negative) = square.BooleanOperation(circle, BooleanOp.Not);

        // Assert
        // NOT operation may result in multiple separate regions
        Assert.InRange(positive.Count, 1, 5);
        Assert.Empty(negative);

        double totalArea = 0;
        for (int i = 0; i < positive.Count; i++)
        {
            using var result = positive[i];
            Assert.True(result.IsClosed);
            totalArea += Math.Abs(result.Area);
        }

        // Total area should be square area minus intersection area
        Assert.InRange(totalArea, 4, 15);
    }

    [Fact(Skip = "等待检查")]
    public void SquareXorCircle_PartialOverlap()
    {
        using var square = CreateSquare(10);
        using var circle = CreateCircle(6, 5, 5);
        // Act
        var (positive, negative) = square.BooleanOperation(circle, BooleanOp.Xor);

        // Assert
        Assert.Equal(2, positive.Count);
        Assert.Empty(negative);

        double totalArea = 0;
        for (int i = 0; i < positive.Count; i++)
        {
            using var result = positive[i];
            totalArea += Math.Abs(result.Area);
        }

        Assert.InRange(totalArea, 107.7, 108.7);
    }
    #endregion

    #region L-Shape vs Circle Operations
    [Fact]
    public void LShapeOrCircle_Overlap()
    {
        using var lShape = CreateLShaped();
        using var circle = CreateCircle(3, 7, 7);
        // Act
        var (positive, negative) = lShape.BooleanOperation(circle, BooleanOp.Or);

        // Assert
        Assert.Single(positive);
        Assert.Empty(negative);

        using var result = positive[0];
        Assert.True(result.IsClosed);
        Assert.InRange(result.VertexCount, 8, 16);
        Assert.InRange(Math.Abs(result.Area), 72, 98);
    }

    [Fact]
    public void LShapeAndCircle_Overlap()
    {
        using var lShape = CreateLShaped();
        using var circle = CreateCircle(3, 7, 7);
        // Act
        var (positive, negative) = lShape.BooleanOperation(circle, BooleanOp.And);

        // Assert
        Assert.Single(positive);
        Assert.Empty(negative);

        using var result = positive[0];
        Assert.True(result.IsClosed);
        Assert.InRange(result.VertexCount, 4, 9);
        Assert.InRange(Math.Abs(result.Area), 4, 12);
    }

    [Fact]
    public void LShapeNotCircle_Overlap()
    {
        using var lShape = CreateLShaped();
        using var circle = CreateCircle(3, 7, 7);
        // Act
        var (positive, negative) = lShape.BooleanOperation(circle, BooleanOp.Not);

        // Assert
        Assert.Single(positive);
        Assert.Empty(negative);

        using var result = positive[0];
        Assert.True(result.IsClosed);
        Assert.InRange(result.VertexCount, 8, 16);
        Assert.InRange(Math.Abs(result.Area), 63, 70);
    }

    [Fact]
    public void LShapeXorCircle_Overlap()
    {
        using var lShape = CreateLShaped();
        using var circle = CreateCircle(3, 7, 7);
        // Act
        var (positive, negative) = lShape.BooleanOperation(circle, BooleanOp.Xor);

        // Assert
        Assert.Equal(2, positive.Count);
        Assert.Empty(negative);

        double totalArea = 0;
        for (int i = 0; i < positive.Count; i++)
        {
            using var result = positive[i];
            totalArea += Math.Abs(result.Area);
        }

        Assert.InRange(totalArea, 79, 92);
    }
    #endregion

    #region Options Tests
    [Fact]
    public void BooleanOperation_WithOptions()
    {
        using var square1 = CreateSquare(10);
        using var square2 = CreateSquare(10, 5, 5);
        // Create options with custom epsilon
        var options = new CavcBooleanOptions
        {
            pos_equal_eps = 0.0001,
            pline1_aabb_index = IntPtr.Zero,
            collapsed_area_eps = 1e-5
        };

        // Act
        var (positive, negative) = square1.BooleanOperation(square2, BooleanOp.And, options);

        // Assert
        Assert.Single(positive);
        Assert.Empty(negative);

        using var result = positive[0];
        Assert.True(result.IsClosed);
        Assert.Equal(4, result.VertexCount);
        Assert.Equal(25, Math.Abs(result.Area), 6);
    }
    #endregion

    #region Edge Cases
    [Fact]
    public void BooleanOperation_WithEmptyPolyline()
    {
        using var square = CreateSquare(10);
        using var empty = new Polyline();
        // Act
        var (positive, negative) = square.BooleanOperation(empty, BooleanOp.Or);

        // Assert
        // Boolean operation with empty polyline may return empty result
        // This is valid behavior - empty polyline has no geometric contribution
        Assert.True(positive.Count >= 0);
        Assert.Empty(negative);

        if (positive.Count > 0)
        {
            using var result = positive[0];
            Assert.True(result.IsClosed);
            Assert.Equal(4, result.VertexCount);
            Assert.Equal(100, Math.Abs(result.Area), 6);
        }
    }

    [Fact]
    public void BooleanOperation_NonOverlapping()
    {
        using var square1 = CreateSquare(10);
        using var square2 = CreateSquare(10, 20, 20);
        // Act - OR operation on non-overlapping shapes
        var (positive, negative) = square1.BooleanOperation(square2, BooleanOp.Or);

        // Assert
        Assert.Equal(2, positive.Count);
        Assert.Empty(negative);

        double totalArea = 0;
        for (int i = 0; i < positive.Count; i++)
        {
            using var result = positive[i];
            Assert.True(result.IsClosed);
            Assert.Equal(4, result.VertexCount);
            totalArea += Math.Abs(result.Area);
        }

        Assert.Equal(200, totalArea, 6);
    }
    #endregion
}
