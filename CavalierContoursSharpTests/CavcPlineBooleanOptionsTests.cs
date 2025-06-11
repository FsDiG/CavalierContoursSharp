using CavalierContoursSharp;

namespace CavalierContoursSharpTests;

public class CavcPlineBooleanOptionsTests
{
    [Fact]
    public void CanInitializeBooleanOptions()
    {
        // Arrange & Act
        var options = new CavcPlineBooleanOptions
        {
            Pline1AabbIndex = nint.Zero,
            PosEqualEps = 0.001
        };

        // Assert
        Assert.Equal(nint.Zero, options.Pline1AabbIndex);
        Assert.Equal(0.001, options.PosEqualEps);
    }
}
