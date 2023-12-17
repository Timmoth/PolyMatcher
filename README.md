# PolyMatcher
An experimental orientation and scale invariant algorithm for identifying a collection of vertices

## Initial solution

### Identification
Sort the vertices in ascending order by their distance to the centroid. This order will be the same for any orientation / scale.
Calculate the angle between each pair of points and the centroid. (If there are an odd number of vertices, pair the last vertex with the first)
Return an array of floating point angles in radians.

### Comparison
Calculate the Euclidean distance between two arrays of floating point angles in radians.
The lower the Euclidean distance the better the match (between 0.0 and 1.0)

### Edge cases
What if two vertices exist at the same distance from the centroid for a given set?

### ToDo
Store the angle array in a MySql database and construct a query to retrieve it.
Consider using a database which supports k-Nearest Neighbor similarity algorithm