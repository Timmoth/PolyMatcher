namespace PolyMatcher;

public static class PolyMatcherExtensions
{
    #region Identification

    public static float[] GetId(this IReadOnlyCollection<Vertex> vertices)
    {
        // Get the point at the center of all vertices
        var centroid = GetCentroid(vertices);

        // Order points by distance from the centroid
        var ordered = vertices.OrderBy(v => v.EuclideanDistance(centroid)).ToList();

        // Create the output array containing
        var angles = new float[(int)Math.Ceiling(ordered.Count / 2.0f)];

        var angleIndex = 0;
        // For each pair of vertices calculate the angle a-b-c where b is the centroid
        for (var i = 0; i < ordered.Count; i += 2)
        {
            var v1 = ordered[i];
            // If there are an odd number of vertices pair the last vertex with the first
            var v2 = (i + 1) < ordered.Count ? ordered[i + 1] : ordered[0];

            var a = new Vertex(v1.X - centroid.X, v1.Y - centroid.Y);
            var c = new Vertex(v2.X - centroid.X, v2.Y - centroid.Y);
            angles[angleIndex++] = a.AngleTo(c);
        }

        return angles;
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

    private static float EuclideanDistance(this Vertex vector1, Vertex vector2)
    {
        return (float)Math.Sqrt((vector1.X - vector2.X) * (vector1.X - vector2.X) +
                                (vector1.Y - vector2.Y) * (vector1.Y - vector2.Y));
    }

    private static float AngleTo(this Vertex vertex, Vertex other)
    {
        // Calculate dot product and magnitudes of the vectors
        var dotProduct = vertex.X * other.X + vertex.Y * other.Y;
        var magnitude1 = Math.Sqrt(vertex.X * vertex.X + vertex.Y * vertex.Y);
        var magnitude2 = Math.Sqrt(other.X * other.X + other.Y * other.Y);

        // Calculate the cosine of the angle between the vectors using dot product and magnitudes
        var cosTheta = dotProduct / (magnitude1 * magnitude2);

        // Avoid potential NaN issues due to precision errors
        cosTheta = Math.Max(-1.0, Math.Min(1.0, cosTheta));

        // Calculate the angle in radians
        return (float)Math.Acos(cosTheta);
    }

    #endregion

    #region Comparison
    public static double EuclideanDistance(float[] set1, float[] set2)
    {
        if (set1.Length != set2.Length || set1.Length == 0)
        {
            throw new ArgumentException("Sets must have the same non-zero length.");
        }

        // Calculate squared differences for each dimension
        var squaredDifferencesSum = set1.Select((t, i) => Math.Pow(t - set2[i], 2)).Sum();

        // Calculate square root of the sum of squared differences
        var distance = Math.Sqrt(squaredDifferencesSum);
        return distance;
    }

    #endregion
}