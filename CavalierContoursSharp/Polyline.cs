using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: InternalsVisibleTo("CavalierContoursSharpTests")]

namespace CavalierContoursSharp;

/// <summary>
/// Managed wrapper for cavc_pline FFI type, representing a 2D polyline with vertices and bulge values.
/// </summary>
public class Polyline : IDisposable, IEnumerable<CavcVertex>
{
    /// <summary>
    /// Indicates if this instance owns the handle and should dispose it
    /// </summary>
    private bool _ownerHandle = true;

    #region Ctor
    /// <summary>
    /// Create a polyline from a handle (for internal use when wrapping FFI results).
    /// </summary>
    /// <param name="handle">
    /// Pointer to the existing polyline handle.
    /// </param>
    /// <param name="owner">
    /// Indicates if this instance owns the handle and should dispose it.
    /// </param>
    internal Polyline(IntPtr handle, bool owner = true)
    {
        Handle = handle;
        _ownerHandle = owner; // This instance does not own the handle
    }

    /// <summary>Create an empty polyline with optional closed state.</summary>
    /// <param name="isClosed">True to create a closed polyline, false for open.</param>
    public Polyline(bool isClosed = false)
    {
        int result = Cavc.cavc_pline_create(null, 0, (byte)(isClosed ? 1 : 0), out var handle);
        ThrowIfError(result, "Failed to create polyline");
        Handle = handle;
    }

    /// <summary>Create a polyline from an array of vertices with optional closed state.</summary>
    /// <param name="vertices">Array of vertices to initialize the polyline.</param>
    /// <param name="isClosed">True to create a closed polyline, false for open.</param>
    public Polyline(CavcVertex[] vertices, bool isClosed = false)
    {
        int result = Cavc.cavc_pline_create(
            vertices,
            (uint)vertices.Length,
            (byte)(isClosed ? 1 : 0),
            out var handle
        );
        ThrowIfError(result, "Failed to create polyline from vertices");
        Handle = handle;
    }
    #endregion

    #region IDisposable
    ~Polyline() => Dispose(false);

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (Handle != IntPtr.Zero)
        {
            if (_ownerHandle)
            {
                Cavc.cavc_pline_f(Handle);
            }
            Handle = IntPtr.Zero;
        }
    }
    #endregion

    internal IntPtr Handle { get; private set; }

    #region Properties
    /// <summary>Get or set whether the polyline is closed.</summary>
    public bool IsClosed
    {
        get
        {
            ThrowIfDisposed();
            if (Cavc.cavc_pline_get_is_closed(Handle, out byte isClosed) != 0)
                ThrowLastError("Failed to get closed state");
            return isClosed != 0;
        }
        set
        {
            ThrowIfDisposed();
            int result = Cavc.cavc_pline_set_is_closed(Handle, (byte)(value ? 1 : 0));
            ThrowIfError(result, "Failed to set closed state");
        }
    }

    /// <summary>Get the number of vertices in the polyline.</summary>
    public int VertexCount
    {
        get
        {
            ThrowIfDisposed();
            if (Cavc.cavc_pline_get_vertex_count(Handle, out uint count) != 0)
                ThrowLastError("Failed to get vertex count");
            return (int)count;
        }
    }

    /// <summary>Get the total path length of the polyline.</summary>
    public double PathLength
    {
        get
        {
            ThrowIfDisposed();
            if (Cavc.cavc_pline_eval_path_length(Handle, out double length) != 0)
                ThrowLastError("Failed to evaluate path length");
            return length;
        }
    }

    /// <summary>Get the area enclosed by the polyline (requires closed polyline).</summary>
    /// <remarks>Maybe negtive.</remarks>
    public double Area
    {
        get
        {
            ThrowIfDisposed();
            if (Cavc.cavc_pline_eval_area(Handle, out double area) != 0)
                ThrowLastError("Failed to evaluate area");
            return area;
        }
    }
    #endregion

    #region Vertex Management
    /// <summary>Add a vertex to the polyline.</summary>
    /// <param name="x">X coordinate of the vertex.</param>
    /// <param name="y">Y coordinate of the vertex.</param>
    /// <param name="bulge">Bulge value for curved segments.</param>
    public void AddVertex(double x, double y, double bulge = 0)
    {
        ThrowIfDisposed();
        int result = Cavc.cavc_pline_add(Handle, x, y, bulge);
        ThrowIfError(result, "Failed to add vertex");
    }

    public void AddVertex(CavcVertex vertex)
    {
        ThrowIfDisposed();
        AddVertex(vertex.X, vertex.Y, vertex.Bulge);
    }

    /// <summary>Get a vertex at the specified index.</summary>
    /// <param name="index">Zero-based index of the vertex.</param>
    /// <returns>The vertex at the specified index.</returns>
    public CavcVertex GetVertex(uint index)
    {
        ThrowIfDisposed();
        if (Cavc.cavc_pline_get_vertex(Handle, index, out CavcVertex vertex) != 0)
            ThrowLastError("Failed to get vertex");
        return vertex;
    }

    /// <summary>Set a vertex at the specified index.</summary>
    /// <param name="index">Zero-based index of the vertex.</param>
    /// <param name="vertex">New vertex value.</param>
    public void SetVertex(uint index, CavcVertex vertex)
    {
        ThrowIfDisposed();
        int result = Cavc.cavc_pline_set_vertex(Handle, index, vertex);
        ThrowIfError(result, "Failed to set vertex");
    }

    /// <summary>Remove a vertex at the specified index.</summary>
    /// <param name="index">Zero-based index of the vertex to remove.</param>
    public void RemoveVertex(uint index)
    {
        ThrowIfDisposed();
        int result = Cavc.cavc_pline_remove(Handle, index);
        ThrowIfError(result, "Failed to remove vertex");
    }

    /// <summary>Get all vertices as an array.</summary>
    /// <returns>Array of vertices in the polyline.</returns>
    public CavcVertex[] GetVertices()
    {
        ThrowIfDisposed();
        int count = VertexCount;
        CavcVertex[] vertices = new CavcVertex[count];
        int result = Cavc.cavc_pline_get_vertex_data(Handle, vertices);
        ThrowIfError(result, "Failed to get vertex data");
        return vertices;
    }

    /// <summary>Set all vertices from an array.</summary>
    /// <param name="vertices">New vertices for the polyline.</param>
    public void SetVertices(CavcVertex[] vertices)
    {
        ThrowIfDisposed();
        int result = Cavc.cavc_pline_set_vertex_data(Handle, vertices, (uint)vertices.Length);
        ThrowIfError(result, "Failed to set vertex data");
    }

    /// <summary>Clear all vertices from the polyline.</summary>
    public void Clear()
    {
        ThrowIfDisposed();
        int result = Cavc.cavc_pline_clear(Handle);
        ThrowIfError(result, "Failed to clear polyline");
    }
    #endregion

    #region Transformations
    /// <summary>Invert the direction of the polyline.</summary>
    public void InvertDirection()
    {
        ThrowIfDisposed();
        int result = Cavc.cavc_pline_invert_direction(Handle);
        ThrowIfError(result, "Failed to invert direction");
    }

    /// <summary>Scale the polyline by a factor.</summary>
    /// <param name="scaleFactor">Factor to scale the polyline by.</param>
    public void Scale(double scaleFactor)
    {
        ThrowIfDisposed();
        int result = Cavc.cavc_pline_scale(Handle, scaleFactor);
        ThrowIfError(result, "Failed to scale polyline");
    }

    /// <summary>Translate (move) the polyline by offsets.</summary>
    /// <param name="xOffset">X offset to translate by.</param>
    /// <param name="yOffset">Y offset to translate by.</param>
    public void Translate(double xOffset, double yOffset)
    {
        ThrowIfDisposed();
        int result = Cavc.cavc_pline_translate(Handle, xOffset, yOffset);
        ThrowIfError(result, "Failed to translate polyline");
    }
    #endregion

    #region Cleaning Operations
    /// <summary>Remove repeated positions within a tolerance.</summary>
    /// <param name="epsilon">Tolerance for considering positions equal.</param>
    public void RemoveRepeatPositions(double epsilon)
    {
        ThrowIfDisposed();
        int result = Cavc.cavc_pline_remove_repeat_pos(Handle, epsilon);
        ThrowIfError(result, "Failed to remove repeat positions");
    }

    /// <summary>Remove redundant vertices within a tolerance.</summary>
    /// <param name="epsilon">Tolerance for considering positions redundant.</param>
    public void RemoveRedundantVertices(double epsilon)
    {
        ThrowIfDisposed();
        int result = Cavc.cavc_pline_remove_redundant(Handle, epsilon);
        ThrowIfError(result, "Failed to remove redundant vertices");
    }
    #endregion

    #region Evaluation
    /// <summary>Calculate the winding number at a point.</summary>
    /// <param name="x">X coordinate of the point.</param>
    /// <param name="y">Y coordinate of the point.</param>
    /// <returns>Winding number at the specified point.</returns>
    public int GetWindingNumber(double x, double y)
    {
        ThrowIfDisposed();
        if (Cavc.cavc_pline_eval_wn(Handle, x, y, out int wn) != 0)
            ThrowLastError("Failed to evaluate winding number");
        return wn;
    }

    /// <summary>Get the bounding box extents of the polyline.</summary>
    /// <param name="minX">Minimum X coordinate of the bounding box.</param>
    /// <param name="minY">Minimum Y coordinate of the bounding box.</param>
    /// <param name="maxX">Maximum X coordinate of the bounding box.</param>
    /// <param name="maxY">Maximum Y coordinate of the bounding box.</param>
    /// <returns>True if extents were successfully retrieved, false if polyline is empty.</returns>
    public bool GetExtents(out double minX, out double minY, out double maxX, out double maxY)
    {
        ThrowIfDisposed();
        int result = Cavc.cavc_pline_eval_extents(Handle, out minX, out minY, out maxX, out maxY);
        if (result == 0)
            return true;
        if (result == 2)
        {
            minX = minY = maxX = maxY = double.NaN;
            return false;
        }
        ThrowLastError("Failed to evaluate extents");
        minX = minY = maxX = maxY = 0; // Unreachable, but satisfy compiler
        return false;
    }
    #endregion

    #region Operations
    /// <summary>Perform a parallel offset on the polyline.</summary>
    /// <param name="offsetDistance">Distance to offset the polyline by.</param>
    /// <param name="options">Offset options, or null for defaults.</param>
    /// <returns>List of offset polylines.</returns>
    public PolylineList ParallelOffset(
        double offsetDistance,
        CavcPlineParallelOffsetOptions? options = null
    )
    {
        ThrowIfDisposed();
        IntPtr optionsPtr = IntPtr.Zero;
        if (options.HasValue)
        {
            optionsPtr = Marshal.AllocHGlobal(Marshal.SizeOf(options.Value));
            Marshal.StructureToPtr(options.Value, optionsPtr, false);
        }

        try
        {
            if (
                Cavc.cavc_pline_parallel_offset(
                    Handle,
                    offsetDistance,
                    optionsPtr,
                    out IntPtr result
                ) != 0
            )
                ThrowLastError("Failed to perform parallel offset");
            return new PolylineList(result);
        }
        finally
        {
            if (optionsPtr != IntPtr.Zero)
                Marshal.FreeHGlobal(optionsPtr);
        }
    }

    /// <summary>Perform a boolean operation with another polyline.</summary>
    /// <param name="other">The other polyline to operate with.</param>
    /// <param name="operation">Boolean operation to perform.</param>
    /// <param name="options">Boolean operation options, or null for defaults.</param>
    /// <returns>Tuple of positive and negative result polylines.</returns>
    public (PolylineList Positive, PolylineList Negative) BooleanOperation(
        Polyline other,
        BooleanOp operation,
        CavcPlineBooleanOptions? options = null
    )
    {
        ThrowIfDisposed();
        if (other == null)
            throw new ArgumentNullException(nameof(other));
        other.ThrowIfDisposed();

        IntPtr optionsPtr = IntPtr.Zero;
        if (options.HasValue)
        {
            optionsPtr = Marshal.AllocHGlobal(Marshal.SizeOf(options.Value));
            Marshal.StructureToPtr(options.Value, optionsPtr, false);
        }

        try
        {
            if (
                Cavc.cavc_pline_boolean(
                    Handle,
                    other.Handle,
                    (uint)operation,
                    optionsPtr,
                    out IntPtr posList,
                    out IntPtr negList
                ) != 0
            )
                ThrowLastError("Failed to perform boolean operation");

            return (new PolylineList(posList), new PolylineList(negList));
        }
        finally
        {
            if (optionsPtr != IntPtr.Zero)
                Marshal.FreeHGlobal(optionsPtr);
        }
    }
    #endregion

    #region AABB Index
    /// <summary>Create an approximate AABB index for the polyline.</summary>
    /// <returns>New AABB index for spatial queries.</returns>
    public CavcAabbIndex CreateApproximateAabbIndex()
    {
        ThrowIfDisposed();
        if (Cavc.cavc_pline_create_approx_aabbindex(Handle, out IntPtr index) != 0)
            ThrowLastError("Failed to create approximate AABB index");
        return new CavcAabbIndex(index);
    }

    /// <summary>Create an exact AABB index for the polyline.</summary>
    /// <returns>New AABB index for spatial queries.</returns>
    public CavcAabbIndex CreateAabbIndex()
    {
        ThrowIfDisposed();
        if (Cavc.cavc_pline_create_aabbindex(Handle, out IntPtr index) != 0)
            ThrowLastError("Failed to create AABB index");
        return new CavcAabbIndex(index);
    }
    #endregion

    #region IEnumerable Implementation
    /// <summary>Get an enumerator for the vertices in the polyline.</summary>
    /// <returns>Enumerator for the vertices.</returns>
    public IEnumerator<CavcVertex> GetEnumerator()
    {
        ThrowIfDisposed();
        int count = VertexCount;
        for (uint i = 0; i < count; i++)
            yield return GetVertex(i);
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    #endregion

    #region Helper Methods
    private void ThrowIfDisposed()
    {
        if (Handle == IntPtr.Zero)
            throw new ObjectDisposedException(nameof(Polyline));
    }

    private void ThrowIfError(int result, string message)
    {
        if (result != 0)
            throw new InvalidOperationException($"{message}: Error code {result}");
    }

    private void ThrowLastError(string message)
    {
        throw new InvalidOperationException($"{message}: Last error code");
    }
    #endregion

    #region ToString Override
    /// <summary>Returns a string representation of the polyline.</summary>
    /// <returns>A string containing polyline information including vertex count, closed state, and vertices.</returns>
    public override string ToString()
    {
        if (Handle == IntPtr.Zero)
            return "Polyline [Disposed]";

        try
        {
            var sb = new System.Text.StringBuilder();
            sb.Append($"Polyline [VertexCount={VertexCount}, IsClosed={IsClosed}");
            
            // Add path length if available
            try
            {
                sb.Append($", PathLength={PathLength:F3}");
            }
            catch
            {
                // Ignore if path length calculation fails
            }
            
            // Add area if closed and available
            if (IsClosed)
            {
                try
                {
                    sb.Append($", Area={Area:F3}");
                }
                catch
                {
                    // Ignore if area calculation fails
                }
            }
            
            sb.Append("] Vertices: ");
            
            int count = VertexCount;
            if (count == 0)
            {
                sb.Append("[]");
            }
            else if (count <= 5)
            {
                // Show all vertices if 5 or fewer
                sb.Append("[");
                for (uint i = 0; i < count; i++)
                {
                    if (i > 0) sb.Append(", ");
                    var vertex = GetVertex(i);
                    sb.Append($"({vertex.X:F3},{vertex.Y:F3}");
                    if (vertex.Bulge != 0)
                        sb.Append($",b={vertex.Bulge:F3}");
                    sb.Append(")");
                }
                sb.Append("]");
            }
            else
            {
                // Show first 3 and last 2 vertices with ellipsis
                sb.Append("[");
                for (uint i = 0; i < 3; i++)
                {
                    if (i > 0) sb.Append(", ");
                    var vertex = GetVertex(i);
                    sb.Append($"({vertex.X:F3},{vertex.Y:F3}");
                    if (vertex.Bulge != 0)
                        sb.Append($",b={vertex.Bulge:F3}");
                    sb.Append(")");
                }
                sb.Append(", ..., ");
                for (uint i = (uint)(count - 2); i < count; i++)
                {
                    if (i > count - 2) sb.Append(", ");
                    var vertex = GetVertex(i);
                    sb.Append($"({vertex.X:F3},{vertex.Y:F3}");
                    if (vertex.Bulge != 0)
                        sb.Append($",b={vertex.Bulge:F3}");
                    sb.Append(")");
                }
                sb.Append("]");
            }
            
            return sb.ToString();
        }
        catch
        {
            return "Polyline [Error retrieving information]";
        }
    }
    #endregion
}
