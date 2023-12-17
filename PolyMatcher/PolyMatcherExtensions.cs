namespace PolyMatcher;

public static class PolyMatcherExtensions
{
    #region Identification

    public static float[] GetId(this IReadOnlyCollection<Vertex> vertices)
    {
        // Get the point at the center of all vertices
        var centroid = GetCentroid(vertices);

        // Order points by distance from the centroid
        List<(Vertex vertex, float euclideanDistance)> orderedVertices = vertices.Select(v => (v, v.EuclideanDistance(centroid))).OrderByDescending(v => v.Item2).ToList();

        var furthestDistance = orderedVertices[0].euclideanDistance;

        // Normalize all vertices about the centroid
        for (var i = 0; i < orderedVertices.Count; i++)
        {
            var unscaled = orderedVertices[i].vertex;
            var scaledVertex = new Vertex(centroid.X + (unscaled.X - centroid.X) / furthestDistance,
                centroid.Y + (unscaled.Y - centroid.Y) / furthestDistance);
            orderedVertices[i] = (scaledVertex, scaledVertex.EuclideanDistance(centroid));
        }

        // Create the output array
        var angles = new float[orderedVertices.Count];

        // For each pair of vertices calculate the angle a-b-c where b is the centroid
        for (var i = 0; i < orderedVertices.Count; i++)
        {
            var vertexA = orderedVertices[i];
            var vectorAB = new Vertex(vertexA.vertex.X - centroid.X, vertexA.vertex.Y - centroid.Y);

            // If there are an odd number of vertices pair the last vertex with the first
            var vertexC = (i + 1) < orderedVertices.Count ? orderedVertices[i + 1] : orderedVertices[0];
            var vectorCB = new Vertex(vertexC.vertex.X - centroid.X, vertexC.vertex.Y - centroid.Y);
            
            var minAngle = vectorAB.AngleTo(vectorCB);
            var euclideanDistance = vertexC.euclideanDistance;
            if (i + 1 < orderedVertices.Count)
            {
                var minIndex = i + 1;

                // If there are more vertices with the same euclidean distance choose the vertex with the smallest angle a-b-c
                // This solves the issue when multiple vertices have the same euclidean distance from the centroid
                for (var j = i + 2; j < orderedVertices.Count; j++)
                {
                    vertexC = orderedVertices[j];
                    if (vertexC.euclideanDistance - euclideanDistance > 0.01f)
                    {
                        // Euclidean distance varies sufficiently, break
                        break;
                    }
                    
                    vectorCB = new Vertex(vertexC.vertex.X - centroid.X, vertexC.vertex.Y - centroid.Y);
                    var angle = vectorAB.AngleTo(vectorCB);
                    if (angle >= minAngle)
                    {
                        continue;
                    }

                    // Angle is smaller
                    minIndex = j;
                    minAngle = angle;
                }

                // Swap the current vertex with the new one
                (orderedVertices[i + 1], orderedVertices[minIndex]) = (orderedVertices[minIndex], orderedVertices[i + 1]);
            }
            
            angles[i] = minAngle * minAngle * vertexA.euclideanDistance * vertexC.euclideanDistance;
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