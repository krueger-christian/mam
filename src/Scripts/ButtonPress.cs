//#define OCULUS
using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ButtonPress : MonoBehaviour
{


    public enum CALLBACKBEHAVIOUR { ON_BUTTON_DOWN, ON_BUTTON_UP, OFF };

#if !OCULUS
    public enum MOUSEBUTTONS { LEFT_CLICK = 0, RIGHT_CLICK = 1, MIDDLE_CLICK = 2 }
#endif
    DateTime clickDown;

    public bool longDurationPress = true;
    public float buttonPressedDuration = 3.0f;

#if OCULUS
    public OVRInput.Button button;
#else
    public MOUSEBUTTONS button;
#endif

    public CALLBACKBEHAVIOUR callBackBehaviour;

    public UnityEvent onShortClick;
    public UnityEvent onLongClick;

    private bool buttonClicked = false;

    /*--------------------------------------------------------------------------------*/

    void Update()
    {
#if OCULUS
        if (OVRInput.GetDown(button))
#else
        if (Input.GetMouseButtonDown((int)button))
#endif
        {
            clickDown = DateTime.Now;
            buttonClicked = true;

            if (callBackBehaviour == CALLBACKBEHAVIOUR.ON_BUTTON_DOWN)
            {
                if (onShortClick != null)
                    onShortClick.Invoke();
            }
        }
#if OCULUS
        else if (OVRInput.Get(button) && longDurationPress)
#else
        else if (Input.GetMouseButton((int)button) && longDurationPress)
#endif
        {
            if (buttonClicked && (DateTime.Now - clickDown).TotalSeconds > buttonPressedDuration)
                onLongClick.Invoke();
        }
#if OCULUS
        else if (OVRInput.GetUp(button))
#else
        else if (Input.GetMouseButtonUp((int) button))
#endif
        {
            buttonClicked = false;

            if (callBackBehaviour == CALLBACKBEHAVIOUR.ON_BUTTON_UP)
            {
                if (onShortClick != null)
                    onShortClick.Invoke();
            }
        }
	}
}
