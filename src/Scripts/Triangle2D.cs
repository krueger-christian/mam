using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triangle2D
{

    HesseNormalForm side0;
    HesseNormalForm side1;
    HesseNormalForm side2;

    Vector2 p0, p1, p2;

    float area;

    /*----------------------------------------------------------------------*/

    /*             p2
     *             /\
     *            /  \
     *     side1 /    \ side0
     *          /      \
     *         /________\
     *       p0  side2   p1
     *
     */
    public Triangle2D(Vector2 p0, Vector2 p1, Vector2 p2)
    {
        this.p0 = p0;
        this.p1 = p1;
        this.p2 = p2;

        side0 = new HesseNormalForm(p2, p1);
        side1 = new HesseNormalForm(p0, p2);
        side2 = new HesseNormalForm(p1, p0);
        area = Area(p0, p1, p2);
    }

    /*----------------------------------------------------------------------*/

    public bool IsInside(Vector2 pt)
    {
        return (side0.HNF(pt) >= 0 && side1.HNF(pt) >= 0 && side2.HNF(pt) >= 0);
    }

    /*----------------------------------------------------------------------*/

    public int IsInSpace(Vector2 pt){
        int retVal = 0;
        if (side0.HNF(pt) < 0)
            retVal |= 1;
        if (side1.HNF(pt) < 0)
            retVal |= 2;
        if (side2.HNF(pt) < 0)
            retVal |= 4;

        return retVal;
    }

    /*----------------------------------------------------------------------*/

    public Vector3 GetWeights(Vector2 point){
        float w0 = Area(p1, p2, point) / area;
        float w1 = Area(p2, p0, point) / area;
        float w2 = Area(p0, p1, point) / area;

        return new Vector3(w0, w1, w2);
    }

    /*----------------------------------------------------------------------*/

    public static Vector3 GetWeights(Vector2 point, Vector2 t0, Vector2 t1, Vector2 t2, float totalArea)
    {
        float w0 = Area(t1, t2, point) / totalArea;
        float w1 = Area(t2, t0, point) / totalArea;
        float w2 = Area(t0, t1, point) / totalArea;

        return new Vector3(w0, w1, w2);
    }

    /*----------------------------------------------------------------------*/

    public static float Area(Vector2 a, Vector2 b, Vector2 c)
    {
        Vector3 u = new Vector3(b.x - a.x, b.y - a.y);
        Vector3 v = new Vector3(c.x - a.x, c.y - a.y);
        return Vector3.Cross(u, v).magnitude;
    }
}