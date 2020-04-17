//#define OCULUS

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IndicatorGrid : MonoBehaviour
{
    public GameObject cursor;
    private Vector3 initialPosition;
    private Vector3 targetPosition;
    public Vector3 offsetOnActive;

    public float fadeSpeed = 22.0f;
    public int speedFactorFadeIn = 3;
    private float step;
    private int numSteps, stepCounter;

    public Material offMaterial;
    public Material[] materials;
    public MeshRenderer[] frame;
    public TextMeshProUGUI[] labels;

    [Range(0.0f, 7.0f)]
    public float scaleFactor = 3.5f;

    bool cursorIsActive = false;

    /*----------------------------------------------------------------------*/

    void Start()
    {
        UpdatePositions();
        GrayTone();
    }

    /*----------------------------------------------------------------------*/

    void Update()
    {
#if OCULUS
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
#else
        if(Input.GetMouseButtonDown(0))
#endif
        {
            cursorIsActive = true;
        }
#if OCULUS
        else if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger))
#else
        if(Input.GetMouseButtonUp(0))
#endif
        {
            cursorIsActive = false;
        }

        if(cursorIsActive && (stepCounter < numSteps)){
            FadeIn();
        }
        else if(!cursorIsActive && (stepCounter > 0)){
            FadeOut();
        }
    }

    /*----------------------------------------------------------------------*/

    public void SetCursor(Vector2 value)
    {
        cursor.transform.localPosition = value * scaleFactor;
    }

    /*----------------------------------------------------------------------*/

    public void UpdatePositions()
    {
        if (cursorIsActive || stepCounter > 0) return;

        initialPosition = transform.localPosition;
        targetPosition = transform.localPosition + offsetOnActive;
        step = fadeSpeed * Time.deltaTime;
        numSteps = (int) (Vector3.Distance(targetPosition, initialPosition) / step);
    } 

    /*----------------------------------------------------------------------*/

    private void FadeIn()
    {
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPosition, speedFactorFadeIn*step);
        stepCounter += speedFactorFadeIn;

        ColorTone();
    }

    /*----------------------------------------------------------------------*/

    private void FadeOut()
    {
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, initialPosition, step);
        --stepCounter;

        if(stepCounter == 0)
            GrayTone();
    }

    /*----------------------------------------------------------------------*/

    private void GrayTone(){
        foreach (MeshRenderer mr in frame)
            mr.material = offMaterial;
        foreach (TextMeshProUGUI label in labels)
            label.color = offMaterial.color;
    }

    /*----------------------------------------------------------------------*/

    private void ColorTone(){
        for (int i = 0; i < frame.Length; ++i)
            frame[i].material = materials[i];
        for (int i = 0; i < labels.Length; ++i)
            labels[i].color = materials[i].color;
    }
           
}
