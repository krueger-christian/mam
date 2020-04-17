using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolarMapping {

    #region CONSTANTS
    const float PI_2 = 2 * Mathf.PI;
    const float PI_2_DIV_4 = PI_2 / 4;
    const float PI_2_DIV_8 = PI_2 / 8;
    const float PI_DIV_4 = Mathf.PI / 4;

    const float MAX_R = 1.0f;
    const float HALF_R = 0.5f;
    #endregion


    #region GUI_ELEMENTS
    public IndicatorGrid indicatorGrid;

    public Character2DCtrl characterCtrl;
    #endregion

    #region CURSOR_CTRL
    public Vector2 cursor = Vector2.zero;
    #endregion

    #region RESOLUTION_SETTINGS
    public bool highResolutionExpression = false;

    private float phi_offset = -PI_DIV_4;
    private float angle_between_indices = PI_2_DIV_4;
    private float indices_per_ring = 4;
    #endregion

    private Vector2 VA = Character2D.valenceArousal[0];

    /*--------------------------------------------------------------------------------*/

    public PolarMapping()
    {
        HighResolutionActive(highResolutionExpression);
    }

    /*--------------------------------------------------------------------------------*/

    public void HighResolutionActive(bool active)
    {
        highResolutionExpression = active;
        angle_between_indices = (active) ? PI_2_DIV_8 : PI_2_DIV_4;
        indices_per_ring = (active) ? 8 : 4;
    }

    /*--------------------------------------------------------------------------------*/

    // phi --> mood selection
    // r ----> intensity
    public Vector2 MoveCursor(Vector2 movement)
    {
        Vector2 cursorPos = cursor;


        /*-------- Transformation to polar system ----------*/

        Vector2 newPos = cursorPos + movement;

        #region TRANSFORMATION_TO_POLAR
        float phi = Mathf.Atan2(newPos.y, newPos.x);
        float r = Mathf.Sqrt(Mathf.Pow(newPos.x, 2) + Mathf.Pow(newPos.y, 2));

        if (r > MAX_R) r = MAX_R; // clip radius

        if (newPos.x > 1) newPos.x = 1;
        else if (newPos.x < -1) newPos.x = -1;

        if (newPos.y > 1) newPos.y = 1;
        else if (newPos.y < -1) newPos.y = -1;

        float phi_shifted = PhiShift(phi_offset, phi);
        #endregion

        #region UPDATE_EXPRESSION
        float fraction;
        int phi_low, phi_high;
        GetFaceIndex(phi_shifted, out phi_low, out phi_high, out fraction);
        if (r > HALF_R)
        {
            if (highResolutionExpression)
                characterCtrl.SetMoodInterPolatedHR(2 * (r - 0.5f), fraction,
                                                  (Character2D.MoodHighRes)phi_low,
                                                  (Character2D.MoodHighRes)phi_high,
                                                  (Character2D.MoodHighRes)phi_low + 8,
                                                  (Character2D.MoodHighRes)phi_high + 8);
            else
                characterCtrl.SetMoodInterPolated(2 * (r - 0.5f), fraction,
                                              (Character2D.Mood)phi_low,
                                              (Character2D.Mood)phi_high,
                                              (Character2D.Mood)phi_low + 4,
                                              (Character2D.Mood)phi_high + 4);
        }
        else
        {
            if (highResolutionExpression)
                characterCtrl.SetMoodInterPolatedHR(2 * r, (Character2D.MoodHighRes)phi_low, (Character2D.MoodHighRes)phi_high, fraction);
            else
                characterCtrl.SetMoodInterPolated(2 * r, (Character2D.Mood)phi_low, (Character2D.Mood)phi_high, fraction);
        }

        #endregion

        indicatorGrid.SetCursor(newPos);
        cursor = newPos;


        #region GET_VA_VALUES 
        if (r > HALF_R)
        {
            if (highResolutionExpression)
                VA = characterCtrl.GetVAInterPolatedHR(2 * (r - 0.5f), fraction,
                                                      (Character2D.MoodHighRes)phi_low,
                                                      (Character2D.MoodHighRes)phi_high,
                                                      (Character2D.MoodHighRes)phi_low + 8,
                                                      (Character2D.MoodHighRes)phi_high + 8);
            else
                VA = characterCtrl.GetVAInterPolated(2 * (r - 0.5f), fraction,
                                                  (Character2D.Mood)phi_low,
                                                  (Character2D.Mood)phi_high,
                                                  (Character2D.Mood)phi_low + 4,
                                                  (Character2D.Mood)phi_high + 4);
        }
        else
        {
            if (highResolutionExpression)
                VA = characterCtrl.GetVAInterPolatedHR(2 * r, fraction,
                                                         Character2D.MoodHighRes.NEUTRAL,
                                                         Character2D.MoodHighRes.NEUTRAL,
                                                        (Character2D.MoodHighRes)phi_low,
                                                        (Character2D.MoodHighRes)phi_high);
            else
                VA = characterCtrl.GetVAInterPolated(2 * r, fraction,
                                                       Character2D.Mood.NEUTRAL,
                                                       Character2D.Mood.NEUTRAL,
                                                      (Character2D.Mood)phi_low,
                                                      (Character2D.Mood)phi_high);
        }
        #endregion

        return VA;
    }

    /*--------------------------------------------------------------------------------*/

    private float PhiShift(float offset, float phi)
    {
        float shifted = 0;
        if (phi < offset)
            shifted = PI_2 + phi;
        else
            shifted = phi;

        shifted -= offset;
        return shifted;
    }

    /*--------------------------------------------------------------------------------*/

    private void GetFaceIndex(float phi, out int lower, out int higher, out float fraction)
    {
        phi /= angle_between_indices;
        lower = (int)phi + 1;
        higher = (lower == indices_per_ring) ? 1 : lower + 1;
        fraction = lower - phi;
    }

    /*--------------------------------------------------------------------------------*/

    public Vector2 GetVA(){
        return VA;
    }
}
