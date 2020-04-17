using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleMesh2D {

    Triangle2D[] triangles;

    /*----------------------------------------------------------------------*/

    public TriangleMesh2D(int[,] mesh, Vector2[] vertices){
        triangles = new Triangle2D[mesh.GetLength(0)];
        for (int i = 0; i < triangles.Length; ++i)
            triangles[i] = new Triangle2D(vertices[mesh[i, 0]], vertices[mesh[i, 1]], vertices[mesh[i, 2]]);
    }

    /*----------------------------------------------------------------------*/

    public int IsInside(Vector2 point){
        for (int i = 0; i < triangles.Length; ++i)
            if(triangles[i].IsInside(point)) return i;    

        return -1;
    }

    /*----------------------------------------------------------------------*/

    public Vector3 GetWeights(Vector2 point, int triangleIndex){
        return triangles[triangleIndex].GetWeights(point);
    }
}
