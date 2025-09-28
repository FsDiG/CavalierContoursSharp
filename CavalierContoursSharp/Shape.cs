using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: InternalsVisibleTo("CavalierContoursSharpTests")]

namespace CavalierContoursSharp;

/// <summary>
/// Managed wrapper for cavc_shape FFI type, representing a collection of polylines forming a shape.
/// </summary>
public class Shape : IDisposable
{
    /// <summary>
    /// Indicates if this instance owns the handle and should dispose it
    /// </summary>
    private bool _ownerHandle = true;

    #region Ctor
    /// <summary>
    /// Create a shape from a handle (for internal use when wrapping FFI results).
    /// </summary>
    /// <param name="handle">
    /// Pointer to the existing shape handle.
    /// </param>
    /// <param name="owner">
    /// Indicates if this instance owns the handle and should dispose it.
    /// </param>
    internal Shape(IntPtr handle, bool owner = true)
    {
        Handle = handle;
        _ownerHandle = owner;
    }

    /// <summary>Create a shape from a polyline list.</summary>
    /// <param name="polylineList">PolylineList containing the polylines to form the shape.</param>
    public Shape(PolylineList polylineList)
    {
        if (polylineList == null)
            throw new ArgumentNullException(nameof(polylineList));
        
        if (polylineList.Handle == IntPtr.Zero)
            throw new ObjectDisposedException(nameof(polylineList));
        
        int result = Cavc.cavc_shape_create(polylineList.Handle, out var handle);
        ThrowIfError(result, "Failed to create shape from polyline list");
        Handle = handle;
    }
    #endregion

    #region IDisposable
    ~Shape() => Dispose(false);

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
                Cavc.cavc_shape_f(Handle);
            }
            Handle = IntPtr.Zero;
        }
    }
    #endregion

    internal IntPtr Handle { get; private set; }

    #region Properties
    /// <summary>Get the number of counter-clockwise polylines in the shape.</summary>
    public int CcwCount
    {
        get
        {
            ThrowIfDisposed();
            if (Cavc.cavc_shape_get_ccw_count(Handle, out uint count) != 0)
                ThrowLastError("Failed to get CCW polyline count");
            return (int)count;
        }
    }

    /// <summary>Get the number of clockwise polylines in the shape.</summary>
    public int CwCount
    {
        get
        {
            ThrowIfDisposed();
            if (Cavc.cavc_shape_get_cw_count(Handle, out uint count) != 0)
                ThrowLastError("Failed to get CW polyline count");
            return (int)count;
        }
    }
    #endregion

    #region CCW Polyline Access
    /// <summary>Get the vertex count of a specific counter-clockwise polyline.</summary>
    /// <param name="polylineIndex">Index of the CCW polyline.</param>
    /// <returns>Number of vertices in the specified CCW polyline.</returns>
    public int GetCcwPolylineVertexCount(uint polylineIndex)
    {
        ThrowIfDisposed();
        int result = Cavc.cavc_shape_get_ccw_polyline_count(Handle, polylineIndex, out uint count);
        ThrowIfError(result, "Failed to get CCW polyline vertex count");
        return (int)count;
    }

    /// <summary>Check if a specific counter-clockwise polyline is closed.</summary>
    /// <param name="polylineIndex">Index of the CCW polyline.</param>
    /// <returns>True if the CCW polyline is closed, false otherwise.</returns>
    public bool IsCcwPolylineClosed(uint polylineIndex)
    {
        ThrowIfDisposed();
        int result = Cavc.cavc_shape_get_ccw_polyline_is_closed(Handle, polylineIndex, out byte isClosed);
        ThrowIfError(result, "Failed to get CCW polyline closed state");
        return isClosed != 0;
    }

    /// <summary>Get vertex data of a counter-clockwise polyline.</summary>
    /// <param name="polylineIndex">Index of the CCW polyline.</param>
    /// <returns>Array of vertices for the specified CCW polyline.</returns>
    public CavcVertex[] GetCcwPolylineVertices(uint polylineIndex)
    {
        ThrowIfDisposed();
        int vertexCount = GetCcwPolylineVertexCount(polylineIndex);
        CavcVertex[] vertices = new CavcVertex[vertexCount];
        
        int result = Cavc.cavc_shape_get_ccw_polyline_vertex_data(Handle, polylineIndex, vertices);
        ThrowIfError(result, "Failed to get CCW polyline vertex data");
        
        return vertices;
    }

    /// <summary>Set userdata values for a CCW polyline.</summary>
    /// <param name="polylineIndex">Index of the CCW polyline.</param>
    /// <param name="userdataValues">Array of userdata values to set.</param>
    public void SetCcwPolylineUserdata(uint polylineIndex, ulong[] userdataValues)
    {
        ThrowIfDisposed();
        int result = Cavc.cavc_shape_set_ccw_pline_userdata_values(
            Handle, 
            polylineIndex, 
            userdataValues, 
            (uint)userdataValues.Length
        );
        ThrowIfError(result, "Failed to set CCW polyline userdata");
    }

    /// <summary>Get userdata values for a CCW polyline.</summary>
    /// <param name="polylineIndex">Index of the CCW polyline.</param>
    /// <returns>Array of userdata values for the specified CCW polyline.</returns>
    public ulong[] GetCcwPolylineUserdata(uint polylineIndex)
    {
        ThrowIfDisposed();
        
        // First get the count of userdata values
        int result = Cavc.cavc_shape_get_ccw_pline_userdata_count(Handle, polylineIndex, out uint count);
        ThrowIfError(result, "Failed to get CCW polyline userdata count");
        
        if (count == 0)
            return new ulong[0];
        
        // Then get the actual values
        ulong[] userdataValues = new ulong[count];
        result = Cavc.cavc_shape_get_ccw_pline_userdata_values(Handle, polylineIndex, userdataValues);
        ThrowIfError(result, "Failed to get CCW polyline userdata values");
        
        return userdataValues;
    }
    #endregion

    #region CW Polyline Access
    /// <summary>Get the vertex count of a specific clockwise polyline.</summary>
    /// <param name="polylineIndex">Index of the CW polyline.</param>
    /// <returns>Number of vertices in the specified CW polyline.</returns>
    public int GetCwPolylineVertexCount(uint polylineIndex)
    {
        ThrowIfDisposed();
        int result = Cavc.cavc_shape_get_cw_polyline_count(Handle, polylineIndex, out uint count);
        ThrowIfError(result, "Failed to get CW polyline vertex count");
        return (int)count;
    }

    /// <summary>Check if a specific clockwise polyline is closed.</summary>
    /// <param name="polylineIndex">Index of the CW polyline.</param>
    /// <returns>True if the CW polyline is closed, false otherwise.</returns>
    public bool IsCwPolylineClosed(uint polylineIndex)
    {
        ThrowIfDisposed();
        int result = Cavc.cavc_shape_get_cw_polyline_is_closed(Handle, polylineIndex, out byte isClosed);
        ThrowIfError(result, "Failed to get CW polyline closed state");
        return isClosed != 0;
    }

    /// <summary>Get vertex data of a clockwise polyline.</summary>
    /// <param name="polylineIndex">Index of the CW polyline.</param>
    /// <returns>Array of vertices for the specified CW polyline.</returns>
    public CavcVertex[] GetCwPolylineVertices(uint polylineIndex)
    {
        ThrowIfDisposed();
        int vertexCount = GetCwPolylineVertexCount(polylineIndex);
        CavcVertex[] vertices = new CavcVertex[vertexCount];
        
        int result = Cavc.cavc_shape_get_cw_polyline_vertex_data(Handle, polylineIndex, vertices);
        ThrowIfError(result, "Failed to get CW polyline vertex data");
        
        return vertices;
    }

    /// <summary>Set userdata values for a CW polyline.</summary>
    /// <param name="polylineIndex">Index of the CW polyline.</param>
    /// <param name="userdataValues">Array of userdata values to set.</param>
    public void SetCwPolylineUserdata(uint polylineIndex, ulong[] userdataValues)
    {
        ThrowIfDisposed();
        int result = Cavc.cavc_shape_set_cw_pline_userdata_values(
            Handle, 
            polylineIndex, 
            userdataValues, 
            (uint)userdataValues.Length
        );
        ThrowIfError(result, "Failed to set CW polyline userdata");
    }

    /// <summary>Get userdata values for a CW polyline.</summary>
    /// <param name="polylineIndex">Index of the CW polyline.</param>
    /// <returns>Array of userdata values for the specified CW polyline.</returns>
    public ulong[] GetCwPolylineUserdata(uint polylineIndex)
    {
        ThrowIfDisposed();
        
        // First get the count of userdata values
        int result = Cavc.cavc_shape_get_cw_pline_userdata_count(Handle, polylineIndex, out uint count);
        ThrowIfError(result, "Failed to get CW polyline userdata count");
        
        if (count == 0)
            return new ulong[0];
        
        // Then get the actual values
        ulong[] userdataValues = new ulong[count];
        result = Cavc.cavc_shape_get_cw_pline_userdata_values(Handle, polylineIndex, userdataValues);
        ThrowIfError(result, "Failed to get CW polyline userdata values");
        
        return userdataValues;
    }
    #endregion

    #region Shape Operations
    /// <summary>Perform parallel offset operation on the shape.</summary>
    /// <param name="offset">Offset distance (positive for outward, negative for inward).</param>
    /// <param name="options">Optional offset options.</param>
    /// <returns>New shape representing the offset result.</returns>
    public Shape ParallelOffset(double offset, CavcShapeOffsetOptions? options = null)
    {
        ThrowIfDisposed();
        
        IntPtr optionsPtr = IntPtr.Zero;
        if (options.HasValue)
        {
            optionsPtr = Marshal.AllocHGlobal(Marshal.SizeOf(options.Value));
            Marshal.StructureToPtr(options.Value, optionsPtr, false);
            
            // Initialize the options structure
            Cavc.cavc_shape_offset_o_init(optionsPtr);
        }
        
        try
        {
            int result = Cavc.cavc_shape_parallel_offset(Handle, offset, optionsPtr, out IntPtr resultHandle);
            ThrowIfError(result, "Failed to perform parallel offset");
            
            return new Shape(resultHandle);
        }
        finally
        {
            if (optionsPtr != IntPtr.Zero)
                Marshal.FreeHGlobal(optionsPtr);
        }
    }
    #endregion

    #region Helper Methods
    private void ThrowIfDisposed()
    {
        if (Handle == IntPtr.Zero)
            throw new ObjectDisposedException(nameof(Shape));
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
    /// <summary>Returns a string representation of the shape.</summary>
    /// <returns>A string containing shape information including polyline counts.</returns>
    public override string ToString()
    {
        if (Handle == IntPtr.Zero)
            return "Shape [Disposed]";

        try
        {
            return $"Shape [CcwCount={CcwCount}, CwCount={CwCount}]";
        }
        catch
        {
            return "Shape [Error retrieving information]";
        }
    }
    #endregion
}