using System.Runtime.InteropServices;

namespace CavalierContoursSharp;

/// <summary>
/// Represents a polyline vertex holding x, y, and bulge.
/// </summary>
/// <remarks>Create a new vertex with position and bulge.</remarks>
[StructLayout(LayoutKind.Sequential)]
public struct CavcVertex : IEquatable<CavcVertex>
{
    public double X;
    public double Y;
    public double Bulge;

    /// <summary>
    /// Initializes a new instance of the <see cref="CavcVertex"/> struct with specified coordinates and bulge.
    /// </summary>
    /// <param name="x">The X coordinate.</param>
    /// <param name="y">The Y coordinate.</param>
    /// <param name="bulge">The bulge value (default is 0 for straight line).</param>
    public CavcVertex(double x, double y, double bulge = 0)
    {
        X = x;
        Y = y;
        Bulge = bulge;
    }

    /// <summary>
    /// Gets a value indicating whether this vertex represents a straight line segment (bulge = 0).
    /// </summary>
    public readonly bool IsLinear => Math.Abs(Bulge) < double.Epsilon;

    /// <summary>
    /// Gets a value indicating whether this vertex represents an arc segment (bulge != 0).
    /// </summary>
    public readonly bool IsArc => !IsLinear;

    /// <summary>
    /// Returns a string representation of the vertex.
    /// </summary>
    /// <returns>A string in the format "(X, Y, Bulge)".</returns>
    public override readonly string ToString()
    {
        return $"({X:F6}, {Y:F6}, {Bulge:F6})";
    }

    /// <summary>
    /// Returns a string representation of the vertex with custom formatting.
    /// </summary>
    /// <param name="format">The format string for numeric values.</param>
    /// <returns>A formatted string representation of the vertex.</returns>
    public readonly string ToString(string format)
    {
        return $"({X.ToString(format)}, {Y.ToString(format)}, {Bulge.ToString(format)})";
    }

    /// <summary>
    /// Determines whether the specified object is equal to this vertex.
    /// </summary>
    /// <param name="obj">The object to compare with this vertex.</param>
    /// <returns>true if the specified object is equal to this vertex; otherwise, false.</returns>
    public override readonly bool Equals(object obj)
    {
        return obj is CavcVertex other && Equals(other);
    }

    /// <summary>
    /// Determines whether the specified vertex is equal to this vertex.
    /// </summary>
    /// <param name="other">The vertex to compare with this vertex.</param>
    /// <returns>true if the specified vertex is equal to this vertex; otherwise, false.</returns>
    public readonly bool Equals(CavcVertex other)
    {
        return Math.Abs(X - other.X) < double.Epsilon &&
               Math.Abs(Y - other.Y) < double.Epsilon &&
               Math.Abs(Bulge - other.Bulge) < double.Epsilon;
    }

    /// <summary>
    /// Determines whether two vertices are approximately equal within the specified tolerance.
    /// </summary>
    /// <param name="other">The vertex to compare with this vertex.</param>
    /// <param name="tolerance">The tolerance for comparison.</param>
    /// <returns>true if the vertices are approximately equal; otherwise, false.</returns>
    public readonly bool Equals(CavcVertex other, double tolerance)
    {
        return Math.Abs(X - other.X) < tolerance &&
               Math.Abs(Y - other.Y) < tolerance &&
               Math.Abs(Bulge - other.Bulge) < tolerance;
    }

    /// <summary>
    /// Returns the hash code for this vertex.
    /// </summary>
    /// <returns>A hash code for this vertex.</returns>
    public override readonly int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 23 + X.GetHashCode();
            hash = hash * 23 + Y.GetHashCode();
            hash = hash * 23 + Bulge.GetHashCode();
            return hash;
        }
    }

    /// <summary>
    /// Determines whether two vertices are equal.
    /// </summary>
    /// <param name="left">The first vertex to compare.</param>
    /// <param name="right">The second vertex to compare.</param>
    /// <returns>true if the vertices are equal; otherwise, false.</returns>
    public static bool operator ==(CavcVertex left, CavcVertex right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Determines whether two vertices are not equal.
    /// </summary>
    /// <param name="left">The first vertex to compare.</param>
    /// <param name="right">The second vertex to compare.</param>
    /// <returns>true if the vertices are not equal; otherwise, false.</returns>
    public static bool operator !=(CavcVertex left, CavcVertex right)
    {
        return !left.Equals(right);
    }

    /// <summary>
    /// Calculates the distance between this vertex and another vertex.
    /// </summary>
    /// <param name="other">The other vertex.</param>
    /// <returns>The Euclidean distance between the two vertices.</returns>
    public readonly double DistanceTo(CavcVertex other)
    {
        double dx = X - other.X;
        double dy = Y - other.Y;
        return Math.Sqrt(dx * dx + dy * dy);
    }

    /// <summary>
    /// Creates a new vertex with the same position but different bulge.
    /// </summary>
    /// <param name="newBulge">The new bulge value.</param>
    /// <returns>A new vertex with the specified bulge.</returns>
    public readonly CavcVertex WithBulge(double newBulge)
    {
        return new CavcVertex(X, Y, newBulge);
    }

    /// <summary>
    /// Creates a new vertex with the same bulge but different position.
    /// </summary>
    /// <param name="newX">The new X coordinate.</param>
    /// <param name="newY">The new Y coordinate.</param>
    /// <returns>A new vertex with the specified position.</returns>
    public readonly CavcVertex WithPosition(double newX, double newY)
    {
        return new CavcVertex(newX, newY, Bulge);
    }
}
