# PolyMatcher
An experimental orientation and scale invariant algorithm for identifying a collection of vertices

## Initial solution

### Identification
- Sort the vertices in ascending order by their distance to the centroid. This order will be the same for any orientation / scale.
- Normalize the vertices using the furthest point from the centroid.
- Calculate the angle between each pair of points and the centroid. (If there are an odd number of vertices, pair the last vertex with the first)
- Return an array of floating point, each element is calculated by the angle between the pair of vertices and the centroid squared then multiplied by the normalized euclidean distance to the centroid for both points

Note, the order of the elements in the array is important so if multiple vertices have the same euclidean distance to the centroid the angle for each is compared and the smallest is chosen first.

### Comparison
- Calculate the Euclidean distance between two arrays of floating point angles in radians.
- The lower the Euclidean distance the better the match (between 0.0 and 1.0)

### ToDo
- Store the angle array in a MySql database and construct a query to retrieve it.
- Consider using a database which supports k-Nearest Neighbor similarity algorithm
