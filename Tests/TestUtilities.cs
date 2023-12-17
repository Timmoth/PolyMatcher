using PolyMatcher;

namespace Tests
{
    internal static class TestUtilities
    {
        public static List<Vertex> RotateVertices(this IReadOnlyCollection<Vertex> vertices, float angleRadians)
        {
            // Calculate centroid of the vertices
            var (x, y) = GetCentroid(vertices);

            // Translate vertices so that the centroid becomes the origin (0, 0)
            var translatedVertices = vertices.Select(v =>
                new Vertex(v.X - x, v.Y - y)).ToList();

            // Perform the rotation around the origin
            var rotatedVertices = translatedVertices.Select(v =>
                new Vertex(
                    v.X *(float) Math.Cos(angleRadians) - v.Y * (float)Math.Sin(angleRadians),
                    v.X * (float)Math.Sin(angleRadians) + v.Y * (float)Math.Cos(angleRadians)
                )).ToList();

            // Translate vertices back to their original position
            rotatedVertices = rotatedVertices.Select(v =>
                new Vertex(v.X + x, v.Y + y)).ToList();

            return rotatedVertices;
        }

        private static Vertex GetCentroid(IReadOnlyCollection<Vertex> vertices)
        {
            var totalX = 0.0f;
            var totalY = 0.0f;

            foreach (var vertex in vertices)
            {
                totalX += vertex.X;
                totalY += vertex.Y;
            }

            var centroidX = totalX / vertices.Count;
            var centroidY = totalY / vertices.Count;

            return new Vertex(centroidX, centroidY);
        }

        public static List<Vertex> DisplaceVertices(this IReadOnlyCollection<Vertex> input, float maxDisplacement)
        {
            var vertices = new List<Vertex>();
            var random = new Random();

            // Apply a displacement to each vertex
            foreach (var vertex in input)
            {
                var x = (float)((random.NextDouble() * 2 * maxDisplacement) - maxDisplacement);
                var y = (float)((random.NextDouble() * 2 * maxDisplacement) - maxDisplacement);
                vertices.Add(new Vertex(vertex.X + x, vertex.Y + y));
            }

            return vertices;
        }
    }
}
