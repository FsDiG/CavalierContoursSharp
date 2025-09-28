using System.Runtime.InteropServices;

namespace CavalierContoursSharp;

/// <summary>Static class containing P/Invoke bindings to cavc_contours C API.</summary>
internal static class Cavc
{
    private const string DllPath = "cavalier_contours_ffi";

    #region Polyline
    /// <summary>
    /// Create a new polyline object.
    /// vertexes is an array of cavc_vertex to create the polyline with (may be null if n_vertexes is 0).
    /// n_vertexes contains the number of vertexes in the array.
    /// is_closed sets the polyline to be closed if non-zero.
    /// pline is an out parameter to hold the created polyline.
    /// </summary>
    /// <returns>0 on success, error code on failure</returns>
    /// <remarks>
    /// Safety: vertexes may be null if n_vertexes is 0 or must point to a valid contiguous buffer of cavc_vertex.
    /// pline must point to a valid place in memory to be written.
    /// </remarks>
    [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl)]
    public static extern int cavc_pline_create(
        [In, Optional] CavcVertex[]? vertexes,
        uint n_vertexes,
        byte is_closed,
        out IntPtr pline
    );

    /// <summary>Free an existing cavc_pline object.</summary>
    /// <remarks>Nothing happens if pline is null.</remarks>
    [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl)]
    public static extern void cavc_pline_f(IntPtr pline);

    /// <summary>Reserve space for an additional number of vertexes in the cavc_pline.</summary>
    /// <returns>0 on success, 1 if pline is null</returns>
    [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl)]
    public static extern int cavc_pline_reserve(IntPtr pline, uint additional);

    /// <summary>Clones the polyline.</summary>
    /// <returns>0 on success, 1 if pline is null</returns>
    [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl)]
    public static extern int cavc_pline_clone(IntPtr pline, out IntPtr cloned);

    #region Properties
    /// <summary>Get whether the polyline is closed or not.</summary>
    /// <returns>0 on success, 1 if pline is null</returns>
    [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl)]
    public static extern int cavc_pline_get_is_closed(IntPtr pline, out byte is_closed);

    /// <summary>Set whether the polyline is closed or not.</summary>
    /// <returns>0 on success, 1 if pline is null</returns>
    [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl)]
    public static extern int cavc_pline_set_is_closed(IntPtr pline, byte is_closed);

    /// <summary>Get the vertex count of a polyline.</summary>
    /// <returns>0 on success, 1 if pline is null</returns>
    [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl)]
    public static extern int cavc_pline_get_vertex_count(IntPtr pline, out uint count);

    /// <summary>Fills the buffer with the vertex data of a polyline.</summary>
    /// <returns>0 on success, 1 if pline is null</returns>
    /// <remarks>
    /// You must use cavc_pline_get_vertex_count to ensure the buffer has adequate length.
    /// </remarks>
    [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl)]
    public static extern int cavc_pline_get_vertex_data(
        IntPtr pline,
        [Out] CavcVertex[] vertex_data
    );
    #endregion

    #region Modification
    /// <summary>Sets all of the vertexes of a polyline.</summary>
    /// <returns>0 on success, 1 if pline is null</returns>
    [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl)]
    public static extern int cavc_pline_set_vertex_data(
        IntPtr pline,
        [In] CavcVertex[] vertex_data,
        uint n_vertexes
    );

    /// <summary>Clears all of the vertexes of a polyline.</summary>
    /// <returns>0 on success, 1 if pline is null</returns>
    [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl)]
    public static extern int cavc_pline_clear(IntPtr pline);

    /// <summary>Add a vertex to a polyline with x, y, and bulge.</summary>
    /// <returns>0 on success, 1 if pline is null</returns>
    [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl)]
    public static extern int cavc_pline_add(IntPtr pline, double x, double y, double bulge);

    /// <summary>Get a polyline vertex at a given index position.</summary>
    /// <returns>0 on success, 1 if pline is null, 2 if position is out of bounds</returns>
    [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl)]
    public static extern int cavc_pline_get_vertex(
        IntPtr pline,
        uint position,
        out CavcVertex vertex
    );

    /// <summary>Set a polyline vertex at a given index position.</summary>
    /// <returns>0 on success, 1 if pline is null, 2 if position is out of bounds</returns>
    [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl)]
    public static extern int cavc_pline_set_vertex(IntPtr pline, uint position, CavcVertex vertex);

    /// <summary>Remove a vertex from a polyline at an index position.</summary>
    /// <returns>0 on success, 1 if pline is null, 2 if position is out of bounds</returns>
    [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl)]
    public static extern int cavc_pline_remove(IntPtr pline, uint position);
    #endregion

    #region Evaluation
    /// <summary>Wraps PlineSource::path_length.</summary>
    /// <returns>0 on success, 1 if pline is null</returns>
    [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl)]
    public static extern int cavc_pline_eval_path_length(IntPtr pline, out double path_length);

    /// <summary>Wraps PlineSource::area.</summary>
    /// <returns>0 on success, 1 if pline is null</returns>
    [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl)]
    public static extern int cavc_pline_eval_area(IntPtr pline, out double area);

    /// <summary>Wraps PlineSource::winding_number.</summary>
    /// <returns>0 on success, 1 if pline is null</returns>
    [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl)]
    public static extern int cavc_pline_eval_wn(
        IntPtr pline,
        double x,
        double y,
        out int winding_number
    );
    #endregion

    #region Transformations
    /// <summary>Wraps PlineSourceMut::invert_direction_mut.</summary>
    /// <returns>0 on success, 1 if pline is null</returns>
    [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl)]
    public static extern int cavc_pline_invert_direction(IntPtr pline);

    /// <summary>Wraps PlineSourceMut::scale_mut.</summary>
    /// <returns>0 on success, 1 if pline is null</returns>
    [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl)]
    public static extern int cavc_pline_scale(IntPtr pline, double scale_factor);

    /// <summary>Wraps PlineSourceMut::translate_mut.</summary>
    /// <returns>0 on success, 1 if pline is null</returns>
    [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl)]
    public static extern int cavc_pline_translate(IntPtr pline, double x_offset, double y_offset);
    #endregion

    #region Cleaning
    /// <summary>Wraps PlineSource::remove_repeat_pos.</summary>
    /// <returns>0 on success, 1 if pline is null</returns>
    [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl)]
    public static extern int cavc_pline_remove_repeat_pos(IntPtr pline, double pos_equal_eps);

    /// <summary>Wraps PlineSource::remove_redundant.</summary>
    /// <returns>0 on success, 1 if pline is null</returns>
    [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl)]
    public static extern int cavc_pline_remove_redundant(IntPtr pline, double pos_equal_eps);

    /// <summary>Get user data from a polyline.</summary>
    /// <returns>User data pointer</returns>
    [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr cavc_pline_get_userdata(IntPtr pline);

    /// <summary>Set user data for a polyline.</summary>
    [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl)]
    public static extern void cavc_pline_set_userdata(IntPtr pline, IntPtr userdata);

    /// <summary>Wraps PlineSource::extents.</summary>
    /// <returns>0 on success, 1 if pline is null, 2 if vertex count &lt; 2</returns>
    [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl)]
    public static extern int cavc_pline_eval_extents(
        IntPtr pline,
        out double minX,
        out double minY,
        out double maxX,
        out double maxY
    );
    #endregion

    #region Operations
    /// <summary>Wraps PlineSource::parallel_offset_opt.</summary>
    /// <returns>0 on success, 1 if pline is null</returns>
    [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl)]
    public static extern int cavc_pline_parallel_offset(
        IntPtr pline,
        double offset,
        IntPtr options,
        out IntPtr result
    );

    /// <summary>Wraps PlineSource::boolean_opt.</summary>
    /// <returns>0 on success, 1 if pline1/2 is null, 2 if operation is unrecognized</returns>
    [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl)]
    public static extern int cavc_pline_boolean(
        IntPtr pline1,
        IntPtr pline2,
        uint operation,
        IntPtr options,
        out IntPtr posPlines,
        out IntPtr negPlines
    );

    /// <summary>Initialize self-intersect options.</summary>
    /// <returns>0 on success, 1 if options is null</returns>
    [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl)]
    public static extern int cavc_pline_self_intersect_o_init(IntPtr options);

    /// <summary>Scan for self-intersections in a polyline.</summary>
    /// <returns>0 on success, 1 if pline is null</returns>
    [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl)]
    public static extern int cavc_pline_scan_for_self_intersect(
        IntPtr pline,
        IntPtr options,
        out IntPtr result
    );

    /// <summary>Initialize contains options.</summary>
    /// <returns>0 on success, 1 if options is null</returns>
    [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl)]
    public static extern int cavc_pline_contains_o_init(IntPtr options);
    #endregion

    #endregion

    #region AABB Index
    /// <summary>Wraps PlineSource::create_approx_aabb_index.</summary>
    /// <returns>0 on success, 1 if pline is null</returns>
    [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl)]
    public static extern int cavc_pline_create_approx_aabbindex(IntPtr pline, out IntPtr aabbindex);

    /// <summary>Wraps PlineSource::create_aabb_index.</summary>
    /// <returns>0 on success, 1 if pline is null</returns>
    [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl)]
    public static extern int cavc_pline_create_aabbindex(IntPtr pline, out IntPtr aabbindex);

    /// <summary>
    /// Free an existing [cavc_aabbindex] object.
    /// </summary>
    /// <remarks>
    /// Nothing happens if `aabbindex` is null.
    /// </remarks>
    /// <param name="aabbindex"> `aabbindex` must be null or a valid [cavc_aabbindex] object.</param>
    /// <returns></returns>
    [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl)]
    public static extern int cavc_aabbindex_f(IntPtr aabbindex);

    /// <summary>
    /// Wraps the [`StaticAABB2DIndex::bounds`] method (gets total extents of the aabb index).
    /// Writes NaNs if the index is empty.
    /// </summary>
    /// <param name="aabbindex">`aabbindex` must be null or a valid [cavc_aabbindex] object.</param>
    /// <param name="minX"></param>
    /// <param name="minY"></param>
    /// <param name="maxX"></param>
    /// <param name="maxY"></param>
    /// <returns></returns>
    [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl)]
    public static extern int cavc_aabbindex_get_extents(
        IntPtr aabbindex,
        out double minX,
        out double minY,
        out double maxX,
        out double maxY
    );

    #endregion

    #region Polyline List
    /// <summary>Free an existing cavc_plinelist object and all cavc_pline owned by it.</summary>
    [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl)]
    public static extern void cavc_plinelist_f(IntPtr plinelist);

    /// <summary>Get the number of polylines inside a cavc_plinelist.</summary>
    /// <returns>0 on success, 1 if plinelist is null</returns>
    [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl)]
    public static extern int cavc_plinelist_get_count(IntPtr plinelist, out uint count);

    /// <summary>Get a polyline at the given index position in the cavc_plinelist.</summary>
    /// <returns>0 on success, 1 if plinelist is null, 2 if position out of range</returns>
    [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl)]
    public static extern int cavc_plinelist_get_pline(
        IntPtr plinelist,
        uint position,
        out IntPtr pline
    );

    /// <summary>Efficiently release and return the last cavc_pline from a cavc_plinelist.</summary>
    /// <returns>0 on success, 1 if plinelist is null, 2 if plinelist is empty</returns>
    [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl)]
    public static extern int cavc_plinelist_pop(IntPtr plinelist, out IntPtr pline);

    /// <summary>Release and return a cavc_pline from a cavc_plinelist at a given index.</summary>
    /// <returns>0 on success, 1 if plinelist is null, 2 if position out of range</returns>
    [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl)]
    public static extern int cavc_plinelist_take(IntPtr plinelist, uint position, out IntPtr pline);
    #endregion

    #region Options Initialization
    /// <summary>Write default option values to a cavc_pline_parallel_offset_o.</summary>
    /// <returns>0 on success, 1 if options is null</returns>
    [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl)]
    public static extern int cavc_pline_parallel_offset_o_init(IntPtr options);

    /// <summary>Write default option values to a cavc_pline_boolean_o.</summary>
    /// <returns>0 on success, 1 if options is null</returns>
    [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl)]
    public static extern int cavc_pline_boolean_o_init(IntPtr options);

    /// <summary>Write default option values to a cavc_shape_offset_o.</summary>
    /// <returns>0 on success, 1 if options is null</returns>
    [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl)]
    public static extern int cavc_shape_offset_o_init(IntPtr options);
    #endregion

    #region Shape
    /// <summary>Create a new cavc_shape object from a polyline list.</summary>
    /// <returns>0 on success, 1 if plinelist is null</returns>
    [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl)]
    public static extern int cavc_shape_create(IntPtr plinelist, out IntPtr shape);

    /// <summary>Free an existing cavc_shape object.</summary>
    [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl)]
    public static extern void cavc_shape_f(IntPtr shape);

    /// <summary>Wraps Shape::parallel_offset.</summary>
    /// <returns>0 on success, 1 if shape is null</returns>
    [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl)]
    public static extern int cavc_shape_parallel_offset(
        IntPtr shape,
        double offset,
        IntPtr options,
        out IntPtr result
    );

    /// <summary>Get the count of counter-clockwise polylines in a shape.</summary>
    /// <returns>0 on success, 1 if shape is null</returns>
    [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl)]
    public static extern int cavc_shape_get_ccw_count(IntPtr shape, out uint count);

    /// <summary>Get the vertex count of a specific counter-clockwise polyline in a shape.</summary>
    /// <returns>0 on success, 1 if shape is null, 2 if polyline_index out of bounds</returns>
    [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl)]
    public static extern int cavc_shape_get_ccw_polyline_count(
        IntPtr shape,
        uint polyline_index,
        out uint count
    );

    /// <summary>Get whether a specific counter-clockwise polyline in a shape is closed.</summary>
    /// <returns>0 on success, 1 if shape is null, 2 if polyline_index out of bounds</returns>
    [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl)]
    public static extern int cavc_shape_get_ccw_polyline_is_closed(
        IntPtr shape,
        uint polyline_index,
        out byte is_closed
    );

    /// <summary>Get vertex data of a counter-clockwise polyline in a shape.</summary>
    /// <returns>0 on success, 1 if shape is null, 2 if polyline_index out of bounds</returns>
    [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl)]
    public static extern int cavc_shape_get_ccw_polyline_vertex_data(
        IntPtr shape,
        uint polyline_index,
        [Out] CavcVertex[] vertex_data
    );

    /// <summary>Set userdata values of a CCW polyline in a shape.</summary>
    /// <returns>0 on success, 1 if shape is null, 2 if polyline_index out of bounds</returns>
    [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl)]
    public static extern int cavc_shape_set_ccw_pline_userdata_values(
        IntPtr shape,
        uint polyline_index,
        [In] ulong[] userdata_values,
        uint count
    );

    /// <summary>Get userdata value count of a CCW polyline in a shape.</summary>
    /// <returns>0 on success, 1 if shape is null, 2 if polyline_index out of bounds</returns>
    [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl)]
    public static extern int cavc_shape_get_ccw_pline_userdata_count(
        IntPtr shape,
        uint polyline_index,
        out uint count
    );

    /// <summary>Get userdata values of a CCW polyline in a shape.</summary>
    /// <returns>0 on success, 1 if shape is null</returns>
    [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl)]
    public static extern int cavc_shape_get_ccw_pline_userdata_values(
        IntPtr shape,
        uint polyline_index,
        [Out] ulong[] userdata_values
    );

    /// <summary>Get the count of clockwise polylines in a shape.</summary>
    /// <returns>0 on success, 1 if shape is null</returns>
    [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl)]
    public static extern int cavc_shape_get_cw_count(IntPtr shape, out uint count);

    /// <summary>Get the vertex count of a specific clockwise polyline in a shape.</summary>
    /// <returns>0 on success, 1 if shape is null, 2 if polyline_index out of bounds</returns>
    [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl)]
    public static extern int cavc_shape_get_cw_polyline_count(
        IntPtr shape,
        uint polyline_index,
        out uint count
    );

    /// <summary>Get whether a specific clockwise polyline in a shape is closed.</summary>
    /// <returns>0 on success, 1 if shape is null, 2 if polyline_index out of bounds</returns>
    [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl)]
    public static extern int cavc_shape_get_cw_polyline_is_closed(
        IntPtr shape,
        uint polyline_index,
        out byte is_closed
    );

    /// <summary>Get vertex data of a clockwise polyline in a shape.</summary>
    /// <returns>0 on success, 1 if shape is null, 2 if polyline_index out of bounds</returns>
    [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl)]
    public static extern int cavc_shape_get_cw_polyline_vertex_data(
        IntPtr shape,
        uint polyline_index,
        [Out] CavcVertex[] vertex_data
    );

    /// <summary>Set userdata values of a CW polyline in a shape.</summary>
    /// <returns>0 on success, 1 if shape is null, 2 if polyline_index out of bounds</returns>
    [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl)]
    public static extern int cavc_shape_set_cw_pline_userdata_values(
        IntPtr shape,
        uint polyline_index,
        [In] ulong[] userdata_values,
        uint count
    );

    /// <summary>Get userdata value count of a CW polyline in a shape.</summary>
    /// <returns>0 on success, 1 if shape is null, 2 if polyline_index out of bounds</returns>
    [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl)]
    public static extern int cavc_shape_get_cw_pline_userdata_count(
        IntPtr shape,
        uint polyline_index,
        out uint count
    );

    /// <summary>Get userdata values of a CW polyline in a shape.</summary>
    /// <returns>0 on success, 1 if shape is null</returns>
    [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl)]
    public static extern int cavc_shape_get_cw_pline_userdata_values(
        IntPtr shape,
        uint polyline_index,
        [Out] ulong[] userdata_values
    );
    #endregion
}

/// <summary>Options for cavc_pline_boolean.</summary>
[StructLayout(LayoutKind.Sequential)]
public struct CavcBooleanOptions
{
    /// <summary>Spatial index of the first polyline segment bounding boxes.</summary>
    public IntPtr pline1_aabb_index;

    /// <summary>Positive real number used to determine if two positions are equal.</summary>
    public double pos_equal_eps;

    /// <summary>Positive real number used to determine collapsed area threshold (set to NaN for None).</summary>
    public double collapsed_area_eps;
}

/// <summary>Options for cavc_shape_parallel_offset.</summary>
[StructLayout(LayoutKind.Sequential)]
public struct CavcShapeOffsetOptions
{
    /// <summary>Positive real number used to determine if two positions are equal.</summary>
    public double pos_equal_eps;

    /// <summary>Positive real number used to determine if two real numbers are equal.</summary>
    public double slice_join_eps;

    /// <summary>Positive real number used to determine if two real numbers are equal.</summary>
    public double offset_dist_eps;
}

/// <summary>Options for cavc_pline_self_intersect.</summary>
[StructLayout(LayoutKind.Sequential)]
public struct CavcPlineSelfIntersectOptions
{
    public IntPtr pline_aabb_index;

    /// <summary>Positive real number used to determine if two positions are equal.</summary>
    public double pos_equal_eps;

    public int include;
}

/// <summary>Options for cavc_pline_contains.</summary>
[StructLayout(LayoutKind.Sequential)]
public struct CavcPlineContainsOptions
{
    public IntPtr pline1_aabb_index;

    /// <summary>Positive real number used to determine if two positions are equal.</summary>
    public double pos_equal_eps;
}
