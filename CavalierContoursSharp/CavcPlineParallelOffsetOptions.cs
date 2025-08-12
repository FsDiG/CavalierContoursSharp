using System.ComponentModel.Design;
using System.Runtime.InteropServices;

namespace CavalierContoursSharp;

/// <summary>FFI representation of PlineOffsetOptions.</summary>
[StructLayout(LayoutKind.Sequential)]
public struct CavcPlineParallelOffsetOptions
{
    /// <summary>
    /// Spatial index of all the polyline segment bounding boxes (or boxes no smaller, e.g. )
    /// </summary>
    /// <remarks>
    /// <see cref="CavcAabbIndex"/> must be created with the same points as the polyline."/>
    /// </remarks>
    public IntPtr AabbIndex;

    /// <summary>
    /// Epsilon for comparing positions of vertices.
    /// </summary>
    public double PosEqualEps;

    /// <summary>
    /// Epsilon for comparing distances between vertices and offset segments.
    /// </summary>
    public double SliceJoinEps;

    /// <summary>
    /// Epsilon for comparing distances between offset segments and the original polyline.
    /// </summary>
    public double OffsetDistEps;

    /// <summary>
    /// Epsilon for comparing distances between offset segments and the original polyline.
    /// </summary>
    public byte HandleSelfIntersects;

    /// <summary>Returns a string representation of the parallel offset options.</summary>
    /// <returns>A string containing the option values.</returns>
    public override string ToString()
    {
        return $"CavcPlineParallelOffsetOptions [AabbIndex={AabbIndex}, PosEqualEps={PosEqualEps:F6}, SliceJoinEps={SliceJoinEps:F6}, OffsetDistEps={OffsetDistEps:F6}, HandleSelfIntersects={HandleSelfIntersects}]";
    }

    public static CavcPlineParallelOffsetOptions Default()
    {
        const double eps = 1e-6;
        return new CavcPlineParallelOffsetOptions
        {
            AabbIndex = IntPtr.Zero,
            PosEqualEps = eps,
            SliceJoinEps = eps,
            OffsetDistEps = eps,
            HandleSelfIntersects = 0
        };
    }
}
