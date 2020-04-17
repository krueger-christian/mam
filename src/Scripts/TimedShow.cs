using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class TimedShow : MonoBehaviour {

    public float hintDuration;

    public GameObject[] objectsToShow;
    public TextMeshProUGUI[] textToShow;
    public Image[] imagesToShow;

    public bool enableOnStart = false;

    public bool fade = false;
    [Range(0.0f, 1.0f)]
    public float fadeStep = 0.001f;
    private int numActiveObjects = 0;

	/*--------------------------------------------------------------------------------*/

	private void Start()
	{
        if (enableOnStart)
            EnableHint();
	}

	/*--------------------------------------------------------------------------------*/

	private void Update()
	{
        if (numActiveObjects > 0)
            FadeOut(fadeStep);
	}

	/*--------------------------------------------------------------------------------*/

	public void EnableHint(){
        StartCoroutine("ShowHint");
    }

    /*--------------------------------------------------------------------------------*/

    IEnumerator ShowHint()
    {
        SetAllObjectsActive(true);
        
        yield return new WaitForSeconds(hintDuration);

        if (fade)
        {
            numActiveObjects =  (objectsToShow == null) ? 0 : objectsToShow.Length;
            numActiveObjects += (textToShow == null) ?    0 : textToShow.Length;
            numActiveObjects += (imagesToShow == null) ?  0 : imagesToShow.Length;
            Debug.Log(numActiveObjects + " objects to be faded out");
        }
        else
        {
            SetAllObjectsActive(false);
        }

        StopCoroutine("ShowHint");
    }

    /*--------------------------------------------------------------------------------*/

    private void FadeOut(float alpha)
    {
        foreach (GameObject g in objectsToShow)
        {
            Color c = g.GetComponent<MeshRenderer>().material.color;
            c.a -= alpha;
            if (c.a <= 0 && g.activeSelf)
            {
                c.a = 0;
                --numActiveObjects;
                g.SetActive(false);
            }
            g.GetComponent<MeshRenderer>().material.color = c;
        }

        foreach (TextMeshProUGUI t in textToShow)
        {
            Color c = t.color;
            c.a -= alpha;
            if (c.a <= 0 && t.gameObject.activeSelf)
            {
                c.a = 0;
                --numActiveObjects;
                t.gameObject.SetActive(false);
            }
            t.color = c;
        }

        foreach (Image i in imagesToShow)
        {
            Color c = i.color;
            c.a -= alpha;
            if (c.a <= 0 && i.gameObject.activeSelf)
            {
                c.a = 0;
                --numActiveObjects;
                i.gameObject.SetActive(false);
            }
            i.color = c;
        }
    }

    /*--------------------------------------------------------------------------------*/

    public void DisableHint(){
        StopCoroutine("ShowHint");

        SetAllObjectsActive(false);
    }

    /*--------------------------------------------------------------------------------*/

    private void SetAllObjectsActive(bool active){
        foreach (GameObject g in objectsToShow)
            g.SetActive(active);
        foreach (TextMeshProUGUI t in textToShow)
            t.gameObject.SetActive(active);
        foreach (Image i in imagesToShow)
            i.gameObject.SetActive(active);
    }
}
