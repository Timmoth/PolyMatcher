namespace PolyMatcher
{
    public record Vertex(float X, float Y)
    {
        public static List<Vertex> FromPoints(params float[] points)
        {
            if (points.Length < 2)
            {
                throw new ArgumentException("Could not construct vertex array, must provide at least two points.");
            }

            if (points.Length % 2 != 0)
            {
                throw new ArgumentException("Could not construct vertex array, must provide an even number of points.");
            }

            var vertices = new List<Vertex>();
            for (var i = 0; i < points.Length; i += 2)
            {
                vertices.Add(new Vertex(points[i], points[i+1]));
            }

            return vertices;
        }
    }
}
