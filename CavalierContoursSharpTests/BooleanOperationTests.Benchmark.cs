using System.Diagnostics;

namespace CavalierContoursSharpTests;

public partial class BooleanOperationTests
{
    [Fact]
    public void SquareOrSquare_CompleteOverlap_100000_times()
    {
        for (int i = 0; i < 100000; i++)
        {
            _output.WriteLine(i.ToString());
            Debug.WriteLine(i.ToString());
            SquareOrSquare_CompleteOverlap();
        }
    }
}
