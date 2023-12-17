using PolyMatcher;
using Xunit.Abstractions;

namespace Tests
{
    public class Tests
    {
        private readonly ITestOutputHelper _outputHelper;

        public Tests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }


        [Theory]
        [InlineData(new[] { 0.0f, 0.0f, 1.0f, 1.0f, 1.0f, 0.0f })]
        [InlineData(new[] { 10.0f, 20.0f, 5.0f, 1.0f, 32.0f, 55.0f, 102.0f, 5.0f })]

        public void Rotated_Vertices_Should_Match(float[] points)
        {
            var original = Vertex.FromPoints(points);

            var numberOfRotations = 5; // Change this value as needed
            var maxRotation = 2 * Math.PI; // Maximum rotation in radians

            for (var i = 0; i < numberOfRotations; i++)
            {
                var rotationAngleObj1 = i * (maxRotation / numberOfRotations);
                var poly1 = original.RotateVertices((float)rotationAngleObj1);

                for (var j = 0; j < numberOfRotations; j++)
                {
                    var rotationAngleObj2 = j * (maxRotation / numberOfRotations);
                    var poly2 = original.RotateVertices((float)rotationAngleObj2);

                    var id1 = poly1.GetId();
                    var id2 = poly2.GetId();

                    // Act
                    var similarity = PolyMatcherExtensions.EuclideanDistance(id1, id2);

                    // Assert
                    _outputHelper.WriteLine($"{similarity} - {string.Join(",", id1)} - {string.Join(",", id2)}");
                    Assert.InRange(similarity, 0.0f, 0.1f);
                }
            }
        }

        [Theory]
        [InlineData(new[] { 0.0f, 0.0f, 10.0f, 10.0f, 10.0f, 20.0f })]

        public void Similar_Vertices_Should_Match(float[] points)
        {
            var original = Vertex.FromPoints(points);

            var numberOfRotations = 5; // Change this value as needed
            var maxRotation = 2 * Math.PI; // Maximum rotation in radians

            for (var i = 0; i < numberOfRotations; i++)
            {
                var rotationAngleObj1 = i * (maxRotation / numberOfRotations);
                var poly1 = original.DisplaceVertices(0.2f);
                poly1 = poly1.RotateVertices((float)rotationAngleObj1);

                for (var j = 0; j < numberOfRotations; j++)
                {
                    var rotationAngleObj2 = j * (maxRotation / numberOfRotations);
                    var poly2 = original.RotateVertices((float)rotationAngleObj2);

                    var id1 = poly1.GetId();
                    var id2 = poly2.GetId();
                    // Act
                    var similarity = PolyMatcherExtensions.EuclideanDistance(id1, id2);

                    _outputHelper.WriteLine($"{similarity} - {string.Join(",", id1)} - {string.Join(",", id2)}");

                    // Assert
                    Assert.InRange(similarity, 0.0f, 0.1f);
                }
            }
        }

        [Theory]
        [InlineData(new[] { 0.0f, 0.0f, 10.0f, 10.0f, 10.0f, 20.0f }, new[] { 40.0f, 2.0f, 2.0f, 1.0f, 32.0f, 11.0f })]

        public void Different_Vertices_Should_Not_Match(float[] pointsA, float[] pointsB)
        {
            // Arrange
            var poly1 = Vertex.FromPoints(pointsA);
            var poly2 = Vertex.FromPoints(pointsB);
            var id1 = poly1.GetId();
            var id2 = poly2.GetId();
            
            // Act
            var similarity = PolyMatcherExtensions.EuclideanDistance(id1, id2);

            // Assert
            _outputHelper.WriteLine($"{similarity} - {string.Join(",", id1)} - {string.Join(",", id2)}");
            Assert.InRange(similarity, 0.4f, 1.1f);
        }
    }
}