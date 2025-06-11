using System.Collections;

namespace CavalierContoursSharp;

/// <summary>Managed wrapper for cavc_plinelist FFI type.</summary>
public class PolylineList : IDisposable, IEnumerable<Polyline>
{
    private readonly bool _owner = true;
    private bool _disposed = false;

    internal IntPtr Handle { get; private set; }

    internal PolylineList(IntPtr handle, bool owner = true)
    {
        Handle = handle;
        _owner = owner;
    }

    ~PolylineList() => Dispose(false);

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (_owner && Handle != IntPtr.Zero)
        {
            Cavc.cavc_plinelist_f(Handle);
            Handle = IntPtr.Zero;
        }
        _disposed = true;
    }

    /// <summary>Number of polylines in the list.</summary>
    public int Count
    {
        get
        {
            if (Handle == IntPtr.Zero)
                return 0;
            if (Cavc.cavc_plinelist_get_count(Handle, out uint count) != 0)
                return 0;
            return (int)count;
        }
    }

    public Polyline Pop()
    {
        if (Handle == IntPtr.Zero)
            throw new ObjectDisposedException(nameof(PolylineList));

        if (Cavc.cavc_plinelist_pop(Handle, out IntPtr plineHandle) != 0)
            throw new InvalidOperationException("Failed to pop polyline");

        if (plineHandle == IntPtr.Zero)
            throw new InvalidOperationException("No polylines left in the list");
        return new Polyline(plineHandle, owner: true);
    }

    /// <summary>Get polyline at specified index.</summary>
    public Polyline this[int index]
    {
        get
        {
            if (Handle == IntPtr.Zero)
                throw new ObjectDisposedException(nameof(PolylineList));
            if (index < 0 || index >= Count)
                throw new IndexOutOfRangeException();

            if (Cavc.cavc_plinelist_get_pline(Handle, (uint)index, out IntPtr plineHandle) != 0)
                throw new InvalidOperationException("Failed to get polyline");

            return new Polyline(plineHandle, false);
        }
    }

    #region IEnumerable Implementation
    /// <summary>Get an enumerator for the vertices in the polyline.</summary>
    /// <returns>Enumerator for the vertices.</returns>
    public IEnumerator<Polyline> GetEnumerator()
    {
        ThrowIfDisposed();
        int count = Count;
        for (int i = 0; i < count; i++)
            yield return this[i];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private void ThrowIfDisposed()
    {
        if (Handle == IntPtr.Zero)
            throw new ObjectDisposedException(nameof(PolylineList));
    }
    #endregion

    #region ToString Override
    /// <summary>Returns a string representation of the polyline list.</summary>
    /// <returns>A string containing list information including count and polyline summaries.</returns>
    public override string ToString()
    {
        if (Handle == IntPtr.Zero)
            return "PolylineList [Disposed]";

        try
        {
            int count = Count;
            var sb = new System.Text.StringBuilder();
            sb.Append($"PolylineList [Count={count}]");
            
            if (count == 0)
            {
                sb.Append(" []");
            }
            else if (count <= 3)
            {
                // Show all polylines if 3 or fewer
                sb.Append(" [");
                for (int i = 0; i < count; i++)
                {
                    if (i > 0) sb.Append(", ");
                    try
                    {
                        var polyline = this[i];
                        sb.Append($"Polyline({polyline.VertexCount}v,{(polyline.IsClosed ? "closed" : "open")})");
                    }
                    catch
                    {
                        sb.Append("Polyline(error)");
                    }
                }
                sb.Append("]");
            }
            else
            {
                // Show first 2 and last 1 polylines with ellipsis
                sb.Append(" [");
                for (int i = 0; i < 2; i++)
                {
                    if (i > 0) sb.Append(", ");
                    try
                    {
                        var polyline = this[i];
                        sb.Append($"Polyline({polyline.VertexCount}v,{(polyline.IsClosed ? "closed" : "open")})");
                    }
                    catch
                    {
                        sb.Append("Polyline(error)");
                    }
                }
                sb.Append(", ..., ");
                try
                {
                    var lastPolyline = this[count - 1];
                    sb.Append($"Polyline({lastPolyline.VertexCount}v,{(lastPolyline.IsClosed ? "closed" : "open")})");
                }
                catch
                {
                    sb.Append("Polyline(error)");
                }
                sb.Append("]");
            }
            
            return sb.ToString();
        }
        catch
        {
            return "PolylineList [Error retrieving information]";
        }
    }
    #endregion
}
