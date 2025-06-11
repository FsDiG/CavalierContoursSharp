using System.Numerics;
using System.Runtime.InteropServices;

namespace CavalierContoursSharp;

/// <summary>
/// Opaque type that wraps a StaticAABB2DIndex.
/// Note the internal member is only public for composing in other Rust libraries wanting to use the
/// FFI opaque type as part of their FFI API.
/// </summary>
/// <remarks>Create a new 2D point with specified coordinates.</remarks>
[StructLayout(LayoutKind.Sequential)]
public struct CavcPoint(double x, double y)
{
    public double X = x;
    public double Y = y;

    /// <summary>Convert from internal Vector2 type.</summary>
    public Vector2 ToVector2() => new Vector2((float)X, (float)Y);

    /// <summary>
    /// Create a CavcPoint from a Vector2.
    /// </summary>
    /// <param name="v">
    /// Vector2 to convert to CavcPoint.
    /// </param>
    /// <returns></returns>
    public static CavcPoint FromVector2(Vector2 v) => new CavcPoint(v.X, v.Y);
}
