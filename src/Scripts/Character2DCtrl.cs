/*  Christian Krueger, 2019
 *  Quality & Usability Lab,
 *  Technische Universitaet Berlin
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*================================================================================*/

public class Character2DCtrl : MonoBehaviour {

    public Character2D.Mood mood;
    public GameObject face;
    public GameObject eyes;

    private SkinnedMeshRenderer meshRendererFace;
    private SkinnedMeshRenderer meshRendererEyes;

    /*--------------------------------------------------------------------------------*/

	void Start () {
        meshRendererFace = face.GetComponent<SkinnedMeshRenderer>();
        meshRendererEyes = eyes.GetComponent<SkinnedMeshRenderer>();

        SetMood(mood);
	}

    /*--------------------------------------------------------------------------------*/

    public void SetMood(Character2D.Mood newMood)
    {
        mood = newMood;
        for (int key = 0; key < Character2D.MAX_FACE_KEYS; key++)
            meshRendererFace.SetBlendShapeWeight(key, Character2D.getKeyShapeVal(Character2D.Part.FACE, mood, key));

        for (int key = 0; key < Character2D.MAX_EYE_KEYS; key++)
            meshRendererEyes.SetBlendShapeWeight(key, Character2D.getKeyShapeVal(Character2D.Part.EYES, mood, key));
    }

    /*--------------------------------------------------------------------------------*/

    public void SetMoodInterPolated(float[] ratio)
    {
        SetMoodInterPolated(ratio, Character2D.Part.FACE, Character2D.MAX_FACE_KEYS);
        SetMoodInterPolated(ratio, Character2D.Part.EYES, Character2D.MAX_EYE_KEYS);
    }

    /*--------------------------------------------------------------------------------*/

    private void SetMoodInterPolated(float[] ratio, Character2D.Part part, int maxKeys){
        float value = 0;
        for (int key = 0; key < maxKeys; key++)
        {
            //value  = ratio[0] * Character2D.getKeyShapeVal(part, Character2D.Mood.NEUTRAL, key);
            //value += ratio[1] * Character2D.getKeyShapeVal(part, Character2D.Mood.CALM, key);
            //value += ratio[2] * Character2D.getKeyShapeVal(part, Character2D.Mood.CHEERFUL, key);
            //value += ratio[3] * Character2D.getKeyShapeVal(part, Character2D.Mood.TENSE, key);
            //value += ratio[4] * Character2D.getKeyShapeVal(part, Character2D.Mood.BORED, key);
            //value += ratio[5] * Character2D.getKeyShapeVal(part, Character2D.Mood.RELAXED, key);
            //value += ratio[6] * Character2D.getKeyShapeVal(part, Character2D.Mood.EXCITED, key);
            //value += ratio[7] * Character2D.getKeyShapeVal(part, Character2D.Mood.IRRITATED, key);
            //value += ratio[8] * Character2D.getKeyShapeVal(part, Character2D.Mood.SAD, key);
            //value += ratio[9] * Character2D.getKeyShapeVal(part, Character2D.Mood.CALM_RELAXED_EXCITED_CHEERFUL, key);
            //value += ratio[10] * Character2D.getKeyShapeVal(part, Character2D.Mood.CHEERFUL_EXCITED_IRRITATED_TENSE, key);
            //value += ratio[11] * Character2D.getKeyShapeVal(part, Character2D.Mood.TENSE_IRRITATED_SAD_BORED, key);
            //value += ratio[12] * Character2D.getKeyShapeVal(part, Character2D.Mood.BORED_SAD_RELAXED_CALM, key);

            value = 0;
            for (int mood = 0; mood < Character2D.NUM_RATIOS; ++mood){
                    value += ratio[mood] * Character2D.getKeyShapeVal(part, (Character2D.Mood) mood, key);
            }

            switch(part){
                case Character2D.Part.FACE:
                    meshRendererFace.SetBlendShapeWeight(key, value);
                    break;
                case Character2D.Part.EYES:
                    meshRendererEyes.SetBlendShapeWeight(key, value);
                    break;
            }
        }
    }

    /*--------------------------------------------------------------------------------*/


    public void SetMoodInterPolated(float intensity, Character2D.Mood face_1, Character2D.Mood face_2, float ratio){
        float value;
        for (int key = 0; key < Character2D.MAX_FACE_KEYS; key++)
        {
            value = ratio * Character2D.getKeyShapeVal(Character2D.Part.FACE, face_1, key);
            value += (1-ratio) * Character2D.getKeyShapeVal(Character2D.Part.FACE, face_2, key);
            value *= intensity;
            meshRendererFace.SetBlendShapeWeight(key, value);
        }

        for (int key = 0; key < Character2D.MAX_EYE_KEYS; key++)
        {
            value = ratio * Character2D.getKeyShapeVal(Character2D.Part.EYES, face_1, key);
            value += (1-ratio) * Character2D.getKeyShapeVal(Character2D.Part.EYES, face_2, key);
            value *= intensity;
            meshRendererEyes.SetBlendShapeWeight(key, value);
        }
    }

    /*--------------------------------------------------------------------------------*/

    public void SetMoodInterPolatedHR(float intensity, Character2D.MoodHighRes face_1, Character2D.MoodHighRes face_2, float ratio)
    {
        float value;
        for (int key = 0; key < Character2D.MAX_FACE_KEYS; key++)
        {
            value = ratio * Character2D.getKeyShapeVal(Character2D.Part.FACE, face_1, key);
            value += (1 - ratio) * Character2D.getKeyShapeVal(Character2D.Part.FACE, face_2, key);
            value *= intensity;
            meshRendererFace.SetBlendShapeWeight(key, value);
        }

        for (int key = 0; key < Character2D.MAX_EYE_KEYS; key++)
        {
            value = ratio * Character2D.getKeyShapeVal(Character2D.Part.EYES, face_1, key);
            value += (1 - ratio) * Character2D.getKeyShapeVal(Character2D.Part.EYES, face_2, key);
            value *= intensity;
            meshRendererEyes.SetBlendShapeWeight(key, value);
        }
    }

    /*--------------------------------------------------------------------------------*/

    public void SetMoodInterPolated(float radialRatio, float angularRatio,
                                    Character2D.Mood innerface_1, Character2D.Mood innerface_2,
                                    Character2D.Mood outerface_1, Character2D.Mood outerface_2)
    {
        float value, innerValue, outerValue;
        for (int key = 0; key < Character2D.MAX_FACE_KEYS; key++)
        {
            innerValue = angularRatio * Character2D.getKeyShapeVal(Character2D.Part.FACE, innerface_1, key);
            innerValue += (1 - angularRatio) * Character2D.getKeyShapeVal(Character2D.Part.FACE, innerface_2, key);
            outerValue = angularRatio * Character2D.getKeyShapeVal(Character2D.Part.FACE, outerface_1, key);
            outerValue += (1 - angularRatio) * Character2D.getKeyShapeVal(Character2D.Part.FACE, outerface_2, key);
            value = (1 - radialRatio) * innerValue + radialRatio * outerValue;
            meshRendererFace.SetBlendShapeWeight(key, value);
        }

        for (int key = 0; key < Character2D.MAX_EYE_KEYS; key++)
        {
            innerValue = angularRatio * Character2D.getKeyShapeVal(Character2D.Part.EYES, innerface_1, key);
            innerValue += (1 - angularRatio) * Character2D.getKeyShapeVal(Character2D.Part.EYES, innerface_2, key);
            outerValue = angularRatio * Character2D.getKeyShapeVal(Character2D.Part.EYES, outerface_1, key);
            outerValue += (1 - angularRatio) * Character2D.getKeyShapeVal(Character2D.Part.EYES, outerface_2, key);
            value = (1 - radialRatio) * innerValue + radialRatio * outerValue;
            meshRendererEyes.SetBlendShapeWeight(key, value);
        }
    }

    /*--------------------------------------------------------------------------------*/

    // TODO Solve Code Duplication!!!
    public void SetMoodInterPolatedHR(float radialRatio, float angularRatio,
                                      Character2D.MoodHighRes innerface_1, Character2D.MoodHighRes innerface_2,
                                      Character2D.MoodHighRes outerface_1, Character2D.MoodHighRes outerface_2)
    {
        float value, innerValue, outerValue;
        for (int key = 0; key < Character2D.MAX_FACE_KEYS; key++)
        {
            innerValue = angularRatio * Character2D.getKeyShapeVal(Character2D.Part.FACE, innerface_1, key);
            innerValue += (1 - angularRatio) * Character2D.getKeyShapeVal(Character2D.Part.FACE, innerface_2, key);
            outerValue = angularRatio * Character2D.getKeyShapeVal(Character2D.Part.FACE, outerface_1, key);
            outerValue += (1 - angularRatio) * Character2D.getKeyShapeVal(Character2D.Part.FACE, outerface_2, key);
            value = (1 - radialRatio) * innerValue + radialRatio * outerValue;
            meshRendererFace.SetBlendShapeWeight(key, value);
        }

        for (int key = 0; key < Character2D.MAX_EYE_KEYS; key++)
        {
            innerValue = angularRatio * Character2D.getKeyShapeVal(Character2D.Part.EYES, innerface_1, key);
            innerValue += (1 - angularRatio) * Character2D.getKeyShapeVal(Character2D.Part.EYES, innerface_2, key);
            outerValue = angularRatio * Character2D.getKeyShapeVal(Character2D.Part.EYES, outerface_1, key);
            outerValue += (1 - angularRatio) * Character2D.getKeyShapeVal(Character2D.Part.EYES, outerface_2, key);
            value = (1 - radialRatio) * innerValue + radialRatio * outerValue;
            meshRendererEyes.SetBlendShapeWeight(key, value);
        }
    }

    /*--------------------------------------------------------------------------------*/

    private void SetMoodInterPolated(float radialRatio, float angularRatio,
                                int innerface_1, int innerface_2,
                                int outerface_1, int outerface_2)
    {

    }

    /*--------------------------------------------------------------------------------*/

    public void SetFacialKey(Character2D.FacialKeys key, float value){
        meshRendererFace.SetBlendShapeWeight((int)key, value);
    }

    /*--------------------------------------------------------------------------------*/

    public float GetFacialKey(Character2D.FacialKeys key){
        return meshRendererFace.GetBlendShapeWeight((int)key);
    }

    /*--------------------------------------------------------------------------------*/

    public void Tone(Color color, float percentage){
        Color white = new Color(1, 1, 1);
        meshRendererFace.material.color = (1-percentage) * white + percentage * color;
    }

    /*--------------------------------------------------------------------------------*/

    public void Hide(bool active){
        face.SetActive(active);
        eyes.SetActive(active);
    }

    /*--------------------------------------------------------------------------------*/

    private float Damp(float x){
        float p = -Mathf.Pow((x - 1), 2);
        return Mathf.Pow(1000, p);
    }

    /*--------------------------------------------------------------------------------*/

    public Vector2 GetVAInterPolatedHR(float radialRatio, float angularRatio,
                                  Character2D.MoodHighRes innerface_1, Character2D.MoodHighRes innerface_2,
                                  Character2D.MoodHighRes outerface_1, Character2D.MoodHighRes outerface_2)
    {
        float innerValence, innerArousal, outerValence, outerArousal, valence, arousal;
        if (innerface_1 == 0 && innerface_2 == 0)
        {
            innerValence = Character2D.getValence(Character2D.MoodHighRes.NEUTRAL);

            innerArousal = Character2D.getArousal(Character2D.MoodHighRes.NEUTRAL);
        }
        else {
            innerValence = angularRatio * Character2D.getValence(innerface_1);
            innerValence += (1 - angularRatio) * Character2D.getValence(innerface_2);

            innerArousal = angularRatio * Character2D.getArousal(innerface_1);
            innerArousal += (1 - angularRatio) * Character2D.getArousal(innerface_2);
        }
        outerValence = angularRatio * Character2D.getValence(outerface_1);
        outerValence += (1 - angularRatio) * Character2D.getValence(outerface_2);
        valence = (1 - radialRatio) * innerValence + radialRatio * outerValence;

        outerArousal = angularRatio * Character2D.getArousal(outerface_1);
        outerArousal += (1 - angularRatio) * Character2D.getArousal(outerface_2);
        arousal = (1 - radialRatio) * innerArousal + radialRatio * outerArousal;

        return new Vector2(valence, arousal);
    }

    /*--------------------------------------------------------------------------------*/

    public Vector2 GetVAInterPolated(float radialRatio, float angularRatio,
                                  Character2D.Mood innerface_1, Character2D.Mood innerface_2,
                                  Character2D.Mood outerface_1, Character2D.Mood outerface_2)
    {
        float innerValence, innerArousal, outerValence, outerArousal, valence, arousal;
        if (innerface_1 == 0 && innerface_2 == 0)
        {
            innerValence = Character2D.getValence(Character2D.Mood.NEUTRAL);

            innerArousal = Character2D.getArousal(Character2D.Mood.NEUTRAL);
        }
        else
        {
            innerValence = angularRatio * Character2D.getValence(innerface_1);
            innerValence += (1 - angularRatio) * Character2D.getValence(innerface_2);

            innerArousal = angularRatio * Character2D.getArousal(innerface_1);
            innerArousal += (1 - angularRatio) * Character2D.getArousal(innerface_2);
        }
        outerValence = angularRatio * Character2D.getValence(outerface_1);
        outerValence += (1 - angularRatio) * Character2D.getValence(outerface_2);
        valence = (1 - radialRatio) * innerValence + radialRatio * outerValence;

        outerArousal = angularRatio * Character2D.getArousal(outerface_1);
        outerArousal += (1 - angularRatio) * Character2D.getArousal(outerface_2);
        arousal = (1 - radialRatio) * innerArousal + radialRatio * outerArousal;

        return new Vector2(valence, arousal);
    }
}
