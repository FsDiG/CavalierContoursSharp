using System.Runtime.InteropServices;

namespace CavalierContoursSharp;

/// <summary>FFI representation of PlineBooleanOptions.</summary>
[StructLayout(LayoutKind.Sequential)]
public struct CavcPlineBooleanOptions
{
    /// <summary>
    /// Spatial index of all the polyline segment bounding boxes (or boxes no smaller, e.g. )
    /// </summary>
    public IntPtr Pline1AabbIndex;

    /// <summary>
    /// Spatial index of all the polyline segment bounding boxes (or boxes no smaller, e.g. )
    /// </summary>
    public double PosEqualEps;

    /// <summary>Returns a string representation of the boolean options.</summary>
    /// <returns>A string containing the option values.</returns>
    public override string ToString()
    {
        return $"CavcPlineBooleanOptions [Pline1AabbIndex={Pline1AabbIndex}, PosEqualEps={PosEqualEps:F6}]";
    }
}
