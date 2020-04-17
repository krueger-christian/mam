using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounding2D {

    HesseNormalForm right;
    HesseNormalForm top;
    HesseNormalForm left;
    HesseNormalForm bottom;
    HesseNormalForm leftBottomToRightTop;

    Vector2 leftBottom, rightBottom, rightTop, leftTop;


    /*  leftTop  ____
     *          |   /|
     *          |  / |
     *          | /  |
     *          |/___| rightBottom
     */
    float areaLeftTop;
    float areaRightBottom;

    /*          |      |
     *     0110 | 0010 | 0011
     *    ______|______|______
     *          |      |
     *     0100 | 0000 | 0001
     *    ______|______|______
     *          |      |
     *     1100 | 1000 | 1001
     *          |      |
     */
    public enum SPACE { INSIDE = 0, RIGHT = 1, RIGHT_TOP = 3, TOP = 2, LEFT_TOP = 6, LEFT = 4, LEFT_BOTTOM = 12, BOTTOM = 8, RIGHT_BOTTOM = 9};
    private enum HALF_SPACE { LEFT_TOP, RIGHT_BOTTOM};

    public Vector2 normedCursor;
    /*----------------------------------------------------------------------*/

    /*      leftTop     rightTop
     *             +---+
     *             |   |
     *             +---+
     *   leftBottom     rightBottom
     */
    public Bounding2D(Vector2 leftBottom, Vector2 rightBottom, Vector2 rightTop, Vector3 leftTop){
        this.leftBottom = leftBottom;
        this.rightBottom = rightBottom;
        this.rightTop = rightTop;
        this.leftTop = leftTop;

        right  = new HesseNormalForm(rightTop, rightBottom);
        top    = new HesseNormalForm(leftTop, rightTop);
        left   = new HesseNormalForm(leftBottom, leftTop);
        bottom = new HesseNormalForm(rightBottom, leftBottom);
        leftBottomToRightTop = new HesseNormalForm(rightTop, leftBottom);

        areaLeftTop = Triangle2D.Area(leftBottom, rightTop, leftTop);
        areaRightBottom = Triangle2D.Area(leftBottom, rightBottom, rightTop);
    }

    /*----------------------------------------------------------------------*/

    public SPACE IsInSpace(Vector2 pt)
    {
        int retVal = 0;

        if (leftBottomToRightTop.HNF(pt) < 0){
            if (right.HNF(pt) < 0)
                retVal |= (int)SPACE.RIGHT;
            if (bottom.HNF(pt) < 0)
                retVal |= (int)SPACE.BOTTOM;

            normedCursor = ComputeNormedCoordinates(pt, HALF_SPACE.RIGHT_BOTTOM);
        }
        else{
            if (top.HNF(pt) < 0)
                retVal |= (int)SPACE.TOP;
            if (left.HNF(pt) < 0)
                retVal |= (int)SPACE.LEFT;

            normedCursor = ComputeNormedCoordinates(pt, HALF_SPACE.LEFT_TOP);
        }
        return (SPACE) retVal;
    }

    /*----------------------------------------------------------------------*/

    public Vector2 Clip(Vector2 point){
        SPACE space = IsInSpace(point);
        switch(space){
            case SPACE.RIGHT:
                return right.PointXOnLine(point.y);
            case SPACE.RIGHT_TOP:
                return rightTop;
            case SPACE.TOP:
                return top.PointYOnLine(point.x);
            case SPACE.LEFT_TOP:
                return leftTop;
            case SPACE.LEFT:
                return left.PointXOnLine(point.y);
            case SPACE.LEFT_BOTTOM:
                return leftBottom;
            case SPACE.BOTTOM:
                return bottom.PointYOnLine(point.x);
            case SPACE.RIGHT_BOTTOM:
                return rightBottom;
        }
        return point;
    }

    /*----------------------------------------------------------------------*/

    private Vector2 ComputeNormedCoordinates(Vector2 point, HALF_SPACE halfSpace){
        Vector3 weights;
        switch(halfSpace){
            case HALF_SPACE.LEFT_TOP:
                weights = Triangle2D.GetWeights(point, leftBottom, rightTop, leftTop, areaLeftTop);
                return new Vector2(weights.y, weights.y + weights.z);
            case HALF_SPACE.RIGHT_BOTTOM:
                weights = Triangle2D.GetWeights(point, leftBottom, rightBottom, rightTop, areaRightBottom);
                return new Vector2(weights.y + weights.z, weights.z);
        }
        return Vector2.zero;
    }

    /*----------------------------------------------------------------------*/

    public Vector2 GetMappedCoordinates(float min, float max){
        return new Vector2(GetMappedValue(min,max,normedCursor.x), GetMappedValue(min,max,normedCursor.y));
    }

    /*----------------------------------------------------------------------*/

    private float GetMappedValue(float min, float max, float val){
        float retVal = (max - min) * val + min;
        if (retVal > max) return max;
        if (retVal < min) return min;
        return retVal;
    }
}
