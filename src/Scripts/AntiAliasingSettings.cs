//#define OCULUS
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class AntiAliasingSettings : MonoBehaviour
{

    public enum DISPLAYFREQ { FREQ_72HZ, FREQ_60HZ };
    public DISPLAYFREQ displayFrequency = DISPLAYFREQ.FREQ_72HZ;

#if OCULUS
    public OVRManager.TiledMultiResLevel resLevel = OVRManager.TiledMultiResLevel.LMSHigh;
#endif

    public bool xrEnabled = true;

    [Range(0.5f, 2.0f)]
    float eyeTextureResolution = 2.0f;

    /*--------------------------------------------------------------------------------*/

    void Start()
    {
        // provide sufficient resolution and prevent anti-aliasing
        XRSettings.enabled = xrEnabled;
        XRSettings.eyeTextureResolutionScale = eyeTextureResolution;

#if OCULUS
        // ensure that frames update quickly, prevent frozen frames
        OVRManager.display.displayFrequency = (displayFrequency == DISPLAYFREQ.FREQ_72HZ) ? 72.0f : 60.0f;
        OVRManager.tiledMultiResLevel = resLevel;
#endif
	}
}
