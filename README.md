# CavalierContoursSharp

[![NuGet](https://img.shields.io/nuget/v/CavalierContoursSharp.svg)](https://www.nuget.org/packages/CavalierContoursSharp/)
[![CI](https://github.com/weianweigan/cavalier_contours_sharp/actions/workflows/ci.yml/badge.svg)](https://github.com/weianweigan/cavalier_contours_sharp/actions/workflows/ci.yml)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

C# binding for [cavalier_contours](https://github.com/jbuckmccready/cavalier_contours), a high-performance 2D polyline/shape library for offsetting, combining, and more. <mcreference link="https://github.com/jbuckmccready/cavalier_contours" index="1">1</mcreference>

## Features

- **High Performance**: Built on the fast Rust cavalier_contours library <mcreference link="https://github.com/jbuckmccready/cavalier_contours" index="1">1</mcreference>
- **Polyline Operations**: Create, modify, and analyze 2D polylines with arc support
- **Boolean Operations**: Union (OR), Intersection (AND), Difference (NOT), and XOR operations
- **Parallel Offsetting**: Generate offset polylines with various join types
- **Geometric Queries**: Area calculation, path length, winding number, point-in-polygon tests
- **Spatial Indexing**: AABB (Axis-Aligned Bounding Box) indexing for fast spatial queries
- **Memory Safe**: Proper resource management with IDisposable pattern
- **Cross-Platform**: Supports Windows x64 (more platforms coming soon)

## Installation

```bash
dotnet add package CavalierContoursSharp
```

## Quick Start

### Creating a Simple Polyline

```csharp
using CavalierContoursSharp;

// Create an empty polyline
using var polyline = new Polyline();

// Add vertices
polyline.AddVertex(0, 0);
polyline.AddVertex(10, 0);
polyline.AddVertex(10, 10);
polyline.AddVertex(0, 10);

// Make it closed
polyline.IsClosed = true;

// Get properties
Console.WriteLine($"Area: {polyline.Area}");
Console.WriteLine($"Path Length: {polyline.PathLength}");
Console.WriteLine($"Vertex Count: {polyline.VertexCount}");
```

### Creating Polylines with Arcs

```csharp
// Create a polyline with arc segments using bulge values
var vertices = new[]
{
    new CavcVertex(0, 0, 0),      // Straight line to next vertex
    new CavcVertex(10, 0, 0.5),   // Arc to next vertex (bulge = 0.5)
    new CavcVertex(10, 10, 0),    // Straight line to next vertex
    new CavcVertex(0, 10, 0)      // Straight line back to start
};

using var polyline = new Polyline(vertices, true); // true = closed
```

### Boolean Operations

```csharp
// Create two overlapping squares
using var square1 = new Polyline(new[]
{
    new CavcVertex(0, 0),
    new CavcVertex(10, 0),
    new CavcVertex(10, 10),
    new CavcVertex(0, 10)
}, true);

using var square2 = new Polyline(new[]
{
    new CavcVertex(5, 5),
    new CavcVertex(15, 5),
    new CavcVertex(15, 15),
    new CavcVertex(5, 15)
}, true);

// Perform boolean operations
var (union, _) = square1.BooleanOperation(square2, BooleanOp.Or);
var (intersection, _) = square1.BooleanOperation(square2, BooleanOp.And);
var (difference, _) = square1.BooleanOperation(square2, BooleanOp.Not);
var (xor, _) = square1.BooleanOperation(square2, BooleanOp.Xor);

// Remember to dispose results
union.Dispose();
intersection.Dispose();
difference.Dispose();
xor.Dispose();
```

### Parallel Offsetting

```csharp
using var polyline = new Polyline(new[]
{
    new CavcVertex(0, 0),
    new CavcVertex(10, 0),
    new CavcVertex(10, 10),
    new CavcVertex(0, 10)
}, true);

// Create offset polylines
using var offsetResults = polyline.ParallelOffset(2.0); // Offset by 2 units

foreach (var offsetPolyline in offsetResults)
{
    Console.WriteLine($"Offset polyline area: {offsetPolyline.Area}");
    offsetPolyline.Dispose();
}
```

### Geometric Queries

```csharp
using var polyline = new Polyline(new[]
{
    new CavcVertex(0, 0),
    new CavcVertex(10, 0),
    new CavcVertex(10, 10),
    new CavcVertex(0, 10)
}, true);

// Point-in-polygon test
int windingNumber = polyline.GetWindingNumber(5, 5); // Inside: winding number = 1
int windingNumber2 = polyline.GetWindingNumber(15, 15); // Outside: winding number = 0

// Get bounding box
if (polyline.GetExtents(out double minX, out double minY, out double maxX, out double maxY))
{
    Console.WriteLine($"Bounds: ({minX}, {minY}) to ({maxX}, {maxY})");
}

// Transform operations
polyline.Scale(2.0);        // Scale by factor of 2
polyline.Translate(5, 5);   // Move by (5, 5)
polyline.InvertDirection(); // Reverse vertex order
```

### Working with Vertices

```csharp
using var polyline = new Polyline();

// Add vertices one by one
polyline.AddVertex(0, 0, 0);     // x, y, bulge
polyline.AddVertex(10, 0, 0.5);  // Arc segment
polyline.AddVertex(10, 10, 0);

// Get and modify vertices
var vertex = polyline.GetVertex(1);
vertex.Bulge = 0.2; // Change arc curvature
polyline.SetVertex(1, vertex);

// Get all vertices
var allVertices = polyline.GetVertices();

// Iterate through vertices
foreach (var v in polyline)
{
    Console.WriteLine($"Vertex: ({v.X}, {v.Y}), Bulge: {v.Bulge}");
}
```

## Understanding Bulge Values

Bulge values define arc segments between vertices:
- `bulge = 0`: Straight line segment
- `bulge > 0`: Arc curves to the left (counter-clockwise)
- `bulge < 0`: Arc curves to the right (clockwise)
- `bulge = 1`: Perfect semicircle
- `bulge = tan(θ/4)` where θ is the arc's central angle

## API Reference

### Core Classes

- **`Polyline`**: Main class representing a 2D polyline with vertices and arcs
- **`CavcVertex`**: Structure representing a vertex with X, Y coordinates and bulge value
- **`PolylineList`**: Collection of polylines returned by boolean operations
- **`BooleanOp`**: Enumeration of boolean operation types (Or, And, Not, Xor)
- **`CavcAabbIndex`**: Spatial index for fast geometric queries

### Key Methods

- **Vertex Management**: `AddVertex()`, `GetVertex()`, `SetVertex()`, `RemoveVertex()`
- **Boolean Operations**: `BooleanOperation()`
- **Offsetting**: `ParallelOffset()`
- **Geometric Queries**: `GetWindingNumber()`, `GetExtents()`, `Area`, `PathLength`
- **Transformations**: `Scale()`, `Translate()`, `InvertDirection()`
- **Spatial Indexing**: `CreateAabbIndex()`, `CreateApproximateAabbIndex()`

## Performance Notes

- Always dispose of `Polyline` and `PolylineList` objects to free native memory
- Use `using` statements for automatic disposal
- Boolean operations and offsetting can return multiple result polylines
- AABB indexing significantly speeds up spatial queries for complex polylines

## Platform Support

- **Windows x64**: ✅ Fully supported
- **Linux x64**: 🚧 Coming soon
- **macOS**: 🚧 Coming soon

## Contributing

Contributions are welcome! Please feel free to submit issues and pull requests.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Related Projects

- [cavalier_contours](https://github.com/jbuckmccready/cavalier_contours) - The original Rust library <mcreference link="https://github.com/jbuckmccready/cavalier_contours" index="1">1</mcreference>
- [Interactive Web Demo](https://cavaliercontours.dev/) - Try the library online <mcreference link="https://github.com/jbuckmccready/cavalier_contours_web_demo" index="5">5</mcreference>

