using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HesseNormalForm {
    // line l(a) = p + a*v
    private Vector2 start;
    private Vector2 end;
    private Vector2 direction;
    private Vector2 normal;
    private float distance;
    private float xMin, xMax, yMin, yMax;
    private float gradient;

    /*----------------------------------------------------------------------*/

    public HesseNormalForm(Vector2 p1, Vector2 p2){
        start = p1;
        end = p2;
        direction = end - start;
        direction.Normalize();
        normal = new Vector2(direction.y, -direction.x);
        normal.Normalize();
        //if (Vector2.Dot(p1, normal) < 0) normal *= -1;
        distance = distanceToOrigin();

        xMin = (start.x < end.x) ? start.x : end.x;
        xMax = (start.x > end.x) ? start.x : end.x;

        yMin = (start.y < end.y) ? start.y : end.y;
        yMax = (start.y > end.y) ? start.y : end.y;

        gradient = (end.y - start.y) / (end.x - start.x);
    }

    /*----------------------------------------------------------------------*/

    public Vector2 PointYOnLine(float x){
        if (x < xMin) x = xMin;
        else if (x > xMax) x = xMax;

        float y = (x-start.x) * gradient + start.y;

        return new Vector2(x, y);
    }

    /*----------------------------------------------------------------------*/

    public Vector2 PointXOnLine(float y)
    {
        if (y < yMin) y = yMin;
        else if (y > yMax) y = yMax;

        float x = (y - start.y)/ gradient + start.x;

        return new Vector2(x, y);
    }

    /*----------------------------------------------------------------------*/

    private float distanceToOrigin(){
        return Vector2.Dot(start, normal);
    }

    /*----------------------------------------------------------------------*/

    public float HNF(Vector2 point){
        return Vector2.Dot(point, normal) - distance;
    }

    /*----------------------------------------------------------------------*/

    public Vector2 Project(Vector2 vec){
        return (Vector2.Dot(vec, direction) / direction.sqrMagnitude) * direction;
    }

    /*----------------------------------------------------------------------*/

}
