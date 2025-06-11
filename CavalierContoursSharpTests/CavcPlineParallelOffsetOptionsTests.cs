using CavalierContoursSharp;

namespace CavalierContoursSharpTests;

public class CavcPlineParallelOffsetOptionsTests
{
    [Fact]
    public void CanInitializeOptions()
    {
        // Arrange & Act
        var options = new CavcPlineParallelOffsetOptions
        {
            AabbIndex = nint.Zero,
            PosEqualEps = 0.001,
            SliceJoinEps = 0.01,
            OffsetDistEps = 0.001,
            HandleSelfIntersects = 1
        };

        // Assert
        Assert.Equal(nint.Zero, options.AabbIndex);
        Assert.Equal(0.001, options.PosEqualEps);
        Assert.Equal(0.01, options.SliceJoinEps);
        Assert.Equal(0.001, options.OffsetDistEps);
        Assert.Equal(1, options.HandleSelfIntersects);
    }
}
