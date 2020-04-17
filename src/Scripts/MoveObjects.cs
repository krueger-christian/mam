//#define OCULUS
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObjects : MonoBehaviour
{

    public GameObject[] sceneObjects;

    private Vector2 lastTouchPos;
#if OCULUS
    private bool touching = false;
#endif
    public float touchPadSpeed = 0.5f;
    private const int TOUCH_SPEED_FACTOR = 3;

    public float Z_MAX_MOVE;
    public float Z_MIN_MOVE;
    private float movedDistance = 0;

    public IndicatorGrid indicatorGrid;

    /*--------------------------------------------------------------------------------*/

	void Start () {}

    /*--------------------------------------------------------------------------------*/

    void Update()
    {
#if OCULUS
        if (!OVRInput.Get(OVRInput.Button.PrimaryTouchpad) && !OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
        {
            if (OVRInput.Get(OVRInput.Touch.PrimaryTouchpad, OVRInput.Controller.RTrackedRemote))
            {
                if (touching == false)
                {
                    lastTouchPos = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad, OVRInput.Controller.RTrackedRemote);
                    touching = true;
                }
                OnTouch(OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad, OVRInput.Controller.RTrackedRemote));
            }
            else if (touching)
            {
                touching = false;
                indicatorGrid.UpdatePositions();
            }
        }
#endif
    }

    /*--------------------------------------------------------------------------------*/

    void OnTouch(Vector2 pos)
    {
        float move = (pos.y - lastTouchPos.y);
        move *= touchPadSpeed * TOUCH_SPEED_FACTOR;

        if ((movedDistance + move) < Z_MIN_MOVE || (movedDistance + move) > Z_MAX_MOVE) return;

        movedDistance += move;

        foreach (GameObject obj in sceneObjects)
        {
            Vector3 newPos = obj.transform.position;
            newPos.z += move;
            obj.transform.position = newPos;
        }

        lastTouchPos = pos;
    }
}
