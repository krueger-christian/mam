using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshMapping {
    
    public Vector2 cursor = Vector2.zero;

    public Character2DCtrl characterCtrl;
    public IndicatorGrid indicatorGrid;

    private TriangleMesh2D triangleMesh;
    private Bounding2D bounding;

    /*--------------------------------------------------------------------------------*/

    public MeshMapping(IndicatorGrid _indicatorGrid)
    {
        indicatorGrid = _indicatorGrid;
        cursor = Character2D.mappings[(int)Character2D.Mood.NEUTRAL];

        triangleMesh = new TriangleMesh2D(Character2D.map, Character2D.mappings);
        bounding = new Bounding2D(
            Character2D.mappings[(int)Character2D.Mood.SAD],
            Character2D.mappings[(int)Character2D.Mood.RELAXED],
            Character2D.mappings[(int)Character2D.Mood.EXCITED],
            Character2D.mappings[(int)Character2D.Mood.IRRITATED]
        );

        cursor = bounding.Clip(cursor);
        indicatorGrid.SetCursor(bounding.GetMappedCoordinates(-1.0f, 1.0f));
    }

    /*--------------------------------------------------------------------------------*/

    public void MoveCursor(Vector2 movement)
    {
        Vector3 newPos = cursor + movement;

        int triangleIndex = triangleMesh.IsInside(newPos);

        if (triangleIndex > -1)
        {
            float[] ratios = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            GetKeyShapeRatios(triangleIndex, newPos, ref ratios);
            characterCtrl.SetMoodInterPolated(ratios);
            cursor = newPos;
        }

        cursor = bounding.Clip(newPos);

        indicatorGrid.SetCursor(bounding.GetMappedCoordinates(-1.0f,1.0f));
    }

    /*--------------------------------------------------------------------------------*/

    bool GetKeyShapeRatios(int triangleIndex, Vector2 pos, ref float[] ratios)
    {
        if (ratios.Length < Character2D.NUM_RATIOS)
            return false;

        Vector3 triangleRatios = triangleMesh.GetWeights(pos, triangleIndex);

        int p0 = Character2D.map[triangleIndex, 0];
        int p1 = Character2D.map[triangleIndex, 1];
        int p2 = Character2D.map[triangleIndex, 2];
        ratios[p0] = triangleRatios[0];
        ratios[p1] = triangleRatios[1];
        ratios[p2] = triangleRatios[2];
        return true;
    }
}
