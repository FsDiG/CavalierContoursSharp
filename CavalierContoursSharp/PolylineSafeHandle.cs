using System.Runtime.InteropServices;
using CavalierContoursSharp;

internal class PolylineSafeHandle : SafeHandle
{
    public PolylineSafeHandle(IntPtr handle)
        : base(handle, true) { }

    public IntPtr Handle => handle;

    public override bool IsInvalid => this.handle == IntPtr.Zero;

    protected override bool ReleaseHandle()
    {
        Cavc.cavc_pline_f(this.handle);
        return true;
    }
}
