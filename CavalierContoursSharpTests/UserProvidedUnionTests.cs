using System;
using CavalierContoursSharp;
using Xunit;

namespace CavalierContoursSharpTests;

public class UserProvidedUnionTests
{
    [Fact]
    public void Union_Of_UserProvided_Polygons_ProducesExpectedExtents()
    {
        using var polyline1 = new Polyline(
            [
                new CavcVertex(11064489.8857023, 90553.8647126771, 0.0),
                new CavcVertex(11145689.8861776, 90553.8647126771, 0.0),
                new CavcVertex(11145689.8861776, 89553.8647126771, 0.0),
                new CavcVertex(11064489.8857023, 89553.8647126771, 0.0)
            ],
            true
        );

        using var polyline2 = new Polyline(
            [
                new CavcVertex(11145689.8861776, 98253.8647126772, 0.0),
                new CavcVertex(11145589.8857023, 98253.8647126772, 0.0),
                new CavcVertex(11145589.8857023, 98853.8647126772, 0.0),
                new CavcVertex(11145689.8861776, 98853.8647126772, 0.0),
                new CavcVertex(11145689.8861776, 103353.864712677, 0.0),
                new CavcVertex(11136489.8857023, 103353.864712677, 0.0),
                new CavcVertex(11136489.8857023, 101553.864712677, 0.0),
                new CavcVertex(11064489.8857023, 101553.864712629, 0.0),
                new CavcVertex(11064489.8857023, 98853.8647126772, 0.0),
                new CavcVertex(11064589.8857023, 98853.8647126772, 0.0),
                new CavcVertex(11064589.8857023, 98253.8647126772, 0.0),
                new CavcVertex(11064489.8857023, 98253.8647126772, 0.0),
                new CavcVertex(11064489.8857023, 97553.8647126773, 0.0),
                new CavcVertex(11064589.8857023, 97553.8647126773, 0.0),
                new CavcVertex(11064589.8857023, 95553.8647126773, 0.0),
                new CavcVertex(11064489.8857023, 95553.8647126773, 0.0),
                new CavcVertex(11064489.8857023, 95053.8647126764, 0.0),
                new CavcVertex(11064589.8857023, 95053.8647126764, 0.0),
                new CavcVertex(11064589.8857023, 94553.8647126772, 0.0),
                new CavcVertex(11064489.8857023, 94553.8647126772, 0.0),
                new CavcVertex(11064489.8857023, 93553.864712677, 0.0),
                new CavcVertex(11064589.8857023, 93553.864712677, 0.0),
                new CavcVertex(11064589.8857023, 91553.8647126771, 0.0),
                new CavcVertex(11064489.8857023, 91553.8647126771, 0.0),
                new CavcVertex(11064489.8857023, 86553.8647126773, 0.0),
                new CavcVertex(11145689.8861776, 86553.8647126771, 0.0),
                new CavcVertex(11145689.8861776, 91553.864712677, 0.0),
                new CavcVertex(11145589.8861776, 91553.8647126771, 0.0),
                new CavcVertex(11145589.8857023, 93553.8647126769, 0.0),
                new CavcVertex(11145689.8861776, 93553.8647126768, 0.0),
                new CavcVertex(11145689.8861776, 94553.8647126771, 0.0),
                new CavcVertex(11145589.8857023, 94553.8647126762, 0.0),
                new CavcVertex(11145589.8857023, 95053.8647126772, 0.0),
                new CavcVertex(11145689.8861776, 95053.8647126764, 0.0),
                new CavcVertex(11145689.8861776, 95553.8647126772, 0.0),
                new CavcVertex(11145589.8857023, 95553.8643116972, 0.0),
                new CavcVertex(11145589.8857023, 97553.8647126773, 0.0),
                new CavcVertex(11145689.8861776, 97553.8647126773, 0.0)
            ],
            true
        );

        var (positive, negative) = polyline2.BooleanOperation(polyline1, BooleanOp.And);

        Assert.Equal(4, positive[0].VertexCount);
    }
}
