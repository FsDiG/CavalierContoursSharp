namespace CavalierContoursSharp;

/// <summary>
/// Managed wrapper for AABB (Axis-Aligned Bounding Box) spatial index.
/// 
/// AABB stands for Axis-Aligned Bounding Box, a commonly used spatial data structure in computational geometry.
/// It represents a rectangular region whose edges are parallel to the coordinate axes, enabling fast spatial
/// queries and collision detection.
/// 
/// Key Features:
/// - Axis-aligned: Bounding box edges are parallel to X and Y axes, simplifying calculations
/// - Spatial indexing: Organizes geometric objects by spatial location to improve query efficiency
/// - Fast queries: Supports range queries, intersection tests, and other spatial operations
/// 
/// Use Cases:
/// - Performance optimization for polygon boolean operations
/// - Acceleration of parallel offset operations
/// - Spatial collision detection
/// - Fast filtering of geometric objects
/// 
/// How It Works:
/// 1. Calculate AABB for each geometric segment (line or arc segment)
/// 2. Organize these AABBs into a spatial index structure (such as R-tree)
/// 3. During queries, first use AABB to quickly filter candidate objects
/// 4. Then perform precise geometric calculations on the candidates
/// 
/// This two-phase approach significantly reduces the number of objects requiring precise calculations,
/// dramatically improving performance.
/// </summary>
public class CavcAabbIndex : IDisposable
{
    internal IntPtr Handle { get; private set; }

    internal CavcAabbIndex(IntPtr handle) => Handle = handle;

    ~CavcAabbIndex() => Dispose(false);

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (Handle != IntPtr.Zero)
        {
            Cavc.cavc_aabbindex_f(Handle);
            Handle = IntPtr.Zero;
        }
    }

    /// <summary>Gets total extents of the aabb index. Writes NaNs if the index is empty.</summary>
    /// <returns>True if extents were retrieved, false if index is empty</returns>
    public bool GetExtents(out double minX, out double minY, out double maxX, out double maxY)
    {
        var result = Cavc.cavc_aabbindex_get_extents(
            Handle,
            out minX,
            out minY,
            out maxX,
            out maxY
        );
        return result == 0;
    }

    /// <summary>Returns a string representation of the AABB index.</summary>
    /// <returns>A string containing AABB index information including extents if available.</returns>
    public override string ToString()
    {
        if (Handle == IntPtr.Zero)
            return "CavcAabbIndex [Disposed]";

        try
        {
            if (GetExtents(out double minX, out double minY, out double maxX, out double maxY))
            {
                if (double.IsNaN(minX) || double.IsNaN(minY) || double.IsNaN(maxX) || double.IsNaN(maxY))
                {
                    return "CavcAabbIndex [Empty]";
                }
                else
                {
                    double width = maxX - minX;
                    double height = maxY - minY;
                    return $"CavcAabbIndex [Extents=({minX:F3},{minY:F3})-({maxX:F3},{maxY:F3}), Size={width:F3}x{height:F3}]";
                }
            }
            else
            {
                return "CavcAabbIndex [Error retrieving extents]";
            }
        }
        catch
        {
            return "CavcAabbIndex [Error retrieving information]";
        }
    }
}
