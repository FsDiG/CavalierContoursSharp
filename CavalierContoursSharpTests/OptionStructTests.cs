using Xunit;
using CavalierContoursSharp;
using System;

namespace CavalierContoursSharpTests
{
    public class OptionStructTests
    {
        [Fact]
        public void CavcPlineSelfIntersectOptions_DefaultValues()
        {
            // Arrange & Act
            var options = new CavcPlineSelfIntersectOptions();

            // Assert
            Assert.Equal(0.0, options.pos_equal_eps);
        }

        [Fact]
        public void CavcPlineSelfIntersectOptions_CanSetValues()
        {
            // Arrange
            var options = new CavcPlineSelfIntersectOptions();
            double expectedEps = 1e-6;

            // Act
            options.pos_equal_eps = expectedEps;

            // Assert
            Assert.Equal(expectedEps, options.pos_equal_eps);
        }

        [Fact]
        public void CavcPlineContainsOptions_DefaultValues()
        {
            // Arrange & Act
            var options = new CavcPlineContainsOptions();

            // Assert
            Assert.Equal(0.0, options.pos_equal_eps);
        }

        [Fact]
        public void CavcPlineContainsOptions_CanSetValues()
        {
            // Arrange
            var options = new CavcPlineContainsOptions();
            double expectedEps = 1e-8;

            // Act
            options.pos_equal_eps = expectedEps;

            // Assert
            Assert.Equal(expectedEps, options.pos_equal_eps);
        }

        [Fact]
        public void CavcBooleanOptions_DefaultValues()
        {
            // Arrange & Act
            var options = new CavcBooleanOptions();

            // Assert
            Assert.Equal(IntPtr.Zero, options.pline1_aabb_index);
            Assert.Equal(0.0, options.pos_equal_eps);
            Assert.Equal(0.0, options.collapsed_area_eps);
        }

        [Fact]
        public void CavcBooleanOptions_CanSetValues()
        {
            // Arrange
            var options = new CavcBooleanOptions();
            IntPtr expectedIndex = new IntPtr(123);
            double expectedPosEps = 1e-6;
            double expectedAreaEps = 1e-4;

            // Act
            options.pline1_aabb_index = expectedIndex;
            options.pos_equal_eps = expectedPosEps;
            options.collapsed_area_eps = expectedAreaEps;

            // Assert
            Assert.Equal(expectedIndex, options.pline1_aabb_index);
            Assert.Equal(expectedPosEps, options.pos_equal_eps);
            Assert.Equal(expectedAreaEps, options.collapsed_area_eps);
        }

        [Fact]
        public void CavcPlineParallelOffsetOptions_CanCreateDefault()
        {
            // Act
            var options = CavcPlineParallelOffsetOptions.Default();

            // Assert
            Assert.Equal(IntPtr.Zero, options.AabbIndex);
            Assert.Equal(1e-5, options.PosEqualEps);
            Assert.Equal(1e-5, options.SliceJoinEps);
            Assert.Equal(1e-5, options.OffsetDistEps);
            Assert.Equal(1, options.HandleSelfIntersects);
        }

        [Fact]
        public void CavcPlineParallelOffsetOptions_CanSetValues()
        {
            // Arrange
            var options = new CavcPlineParallelOffsetOptions();
            IntPtr expectedIndex = new IntPtr(456);
            double expectedPosEps = 1e-7;
            double expectedSliceEps = 1e-8;
            double expectedOffsetEps = 1e-9;
            byte expectedHandle = 0;

            // Act
            options.AabbIndex = expectedIndex;
            options.PosEqualEps = expectedPosEps;
            options.SliceJoinEps = expectedSliceEps;
            options.OffsetDistEps = expectedOffsetEps;
            options.HandleSelfIntersects = expectedHandle;

            // Assert
            Assert.Equal(expectedIndex, options.AabbIndex);
            Assert.Equal(expectedPosEps, options.PosEqualEps);
            Assert.Equal(expectedSliceEps, options.SliceJoinEps);
            Assert.Equal(expectedOffsetEps, options.OffsetDistEps);
            Assert.Equal(expectedHandle, options.HandleSelfIntersects);
        }

        [Fact]
        public void CavcShapeOffsetOptions_DefaultValues()
        {
            // Arrange & Act
            var options = new CavcShapeOffsetOptions();

            // Assert
            Assert.Equal(0.0, options.pos_equal_eps);
            Assert.Equal(0.0, options.slice_join_eps);
            Assert.Equal(0.0, options.offset_dist_eps);
        }

        [Fact]
        public void CavcShapeOffsetOptions_CanSetValues()
        {
            // Arrange
            var options = new CavcShapeOffsetOptions();
            double expectedPosEps = 1e-6;
            double expectedSliceEps = 1e-7;
            double expectedOffsetEps = 1e-8;

            // Act
            options.pos_equal_eps = expectedPosEps;
            options.slice_join_eps = expectedSliceEps;
            options.offset_dist_eps = expectedOffsetEps;

            // Assert
            Assert.Equal(expectedPosEps, options.pos_equal_eps);
            Assert.Equal(expectedSliceEps, options.slice_join_eps);
            Assert.Equal(expectedOffsetEps, options.offset_dist_eps);
        }
    }
}