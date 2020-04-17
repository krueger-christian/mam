/*  Christian Krueger, 2019
 *  Quality & Usability Lab,
 *  Technische Universitaet Berlin
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*================================================================================*/

public static class Character2D
{
    public const int NUMMOODS = 8;

    public enum Mood
    {
        NEUTRAL = 0,
        CALM = 1,
        CHEERFUL = 2,
        TENSE = 3,
        BORED = 4,
        RELAXED = 5,
        EXCITED = 6,
        IRRITATED = 7,
        SAD = 8,
        CALM_RELAXED_EXCITED_CHEERFUL = 9,
        CHEERFUL_EXCITED_IRRITATED_TENSE = 10,
        TENSE_IRRITATED_SAD_BORED = 11,
        BORED_SAD_RELAXED_CALM = 12,
        NONE = -1
    };

    public enum MoodHighRes
    {
        NEUTRAL = 0,
        CALM = 1,
        CALM_CHEERFUL = 2,
        CHEERFUL = 3,
        CHEEFUL_TENSE = 4,
        TENSE = 5,
        TENSE_BORED = 6,
        BORED = 7,
        BORED_CALM = 8,
        RELAXED = 9,
        RELAXED_EXCITED = 10,
        EXCITED = 11,
        EXCITED_IRRITATED = 12,
        IRRITATED = 13,
        IRRITATED_SAD = 14,
        SAD = 15,
        SAD_RELAXED = 16,
        NONE = -1
    };

    public enum Part { FACE, EYES };

    public static int NUM_RATIOS = 13;

    public static int MAX_FACE_KEYS = 22;
    public static int MAX_EYE_KEYS = 3;

    public static float MAX_VALENCE = 4.92f;
    public static float MIN_VALENCE = 1.61f;

    public static float MAX_AROUSAL = 4.68f;
    public static float MIN_AROUSAL = 1.36f;

    /*--------------------------------------------------------------------------------*/

    // vertices
    public static Vector2[] mappings = {
        new Vector2(2.88f, 2.59f), // neutral
        new Vector2(3.38f, 2.09f), // calm
        new Vector2(4.55f, 3.80f), // cheerful
        new Vector2(2.54f, 3.26f), // tense
        new Vector2(2.42f, 1.90f), // bored
        new Vector2(4.47f, 1.36f), // relaxed
        new Vector2(4.92f, 4.68f), // excited
        new Vector2(1.62f, 4.03f), // irritated
        new Vector2(1.61f, 1.87f), // sad
        new Vector2(4.33f, 2.9825f),   //virtual: calm x relaxed x cheerful x excited
        new Vector2(3.4075f, 3.9425f), // virtual: irritated x tense x cheerful x excited
        new Vector2(2.0475f, 2.765f),  // virtual: tense x irritated x sad x bored
        new Vector2(2.97f, 1.805f)     // virtual: bored x sad x relaxed x calm
    };
    /*             ( V   ,   A  )
     *  V: valence
     *  A: arousal
     */

    /*--------------------------------------------------------------------------------*/

    public static Vector2[] valenceArousal = {
        new Vector2(2.88f, 2.59f), // neutral, 0
        new Vector2(3.38f, 2.09f), // calm, 1
        new Vector2(3.965f, 2.945f),
        new Vector2(4.55f, 3.8f), // cheerful, 3
        new Vector2(3.545f, 3.53f),
        new Vector2(2.54f, 3.26f), // tense, 5
        new Vector2(2.48f, 2.58f),
        new Vector2(2.42f, 1.9f), // bored, 7
        new Vector2(2.9f, 1.995f),
        new Vector2(4.47f, 1.36f), // relaxed, 9
        new Vector2(4.695f, 3.02f),
        new Vector2(4.92f, 4.68f), // excited, 11
        new Vector2(3.27f, 4.355f),
        new Vector2(1.62f, 4.03f), // irritated, 13
        new Vector2(1.615f, 2.95f),
        new Vector2(1.61f, 1.87f), // sad, 15
        new Vector2(3.04f, 1.615f)
    };

    /*--------------------------------------------------------------------------------*/

    // triangle mesh
    public static int[,] map = {
        {0,1,2},
        {0,2,3},
        {0,3,4},
        {0,4,1},
        {9,2,1},
        {9,1,5},
        {9,5,6},
        {9,6,2},
        {10,3,2},
        {10,2,6},
        {10,6,7},
        {10,7,3},
        {11,4,3},
        {11,3,7},
        {11,7,8},
        {11,8,4},
        {12,1,4},
        {12,4,8},
        {12,8,5},
        {12,5,1}
    };

    /*--------------------------------------------------------------------------------*/

    static float[,] faceKeys = {
        {0.0f , 0.0f , 0.0f, 0.0f, 0.0f, 0.0f  , 0.0f, 0.0f  , 0.0f  , 0.0f , 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f , 0.0f , 0.0f, 0.0f, 0.37f, 0.0f}, // neutral
        {0.08f, 0.75f, 0.0f, 0.0f, 0.0f, 0.0f  , 0.0f, 0.0f  , 0.0f  , 0.33f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.3f, 0.3f, 0.0f , 0.0f , 0.0f, 0.0f, 0.37f, 0.0f}, // calm
        {0.5f , 1.0f , 0.0f, 0.2f, 0.0f, 0.0f  , 0.0f, 0.0f  , 0.0f  , 0.0f , 0.0f, 0.0f, 0.0f, 0.0f, 0.2f, 0.2f, 0.0f, 0.0f, 0.0f , 0.0f , 0.0f, 0.0f, 0.33f, 0.0f}, // cheerful
        {0.0f , 0.0f , 0.0f, 0.0f, 0.1f, 0.0f  , 0.0f, 0.586f, 0.539f, 0.0f , 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.44f, 0.44f, 0.0f, 0.0f, 0.0f , 0.1f}, // tense
        {0.0f , 0.66f, 0.0f, 0.0f, 0.0f, 0.125f, 1.0f, 0.274f, 0.0f  , 0.4f , 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 1.0f, 0.0f, 0.0f, 0.0f , 0.0f , 0.0f, 0.0f, 0.33f, 0.0f}, // bored
        {0.0f , 0.0f , 0.7f, 0.0f, 0.0f, 0.0f  , 0.0f, 0.0f  , 0.0f  , 1.0f , 0.0f, 0.0f, 0.0f, 0.0f, 0.7f, 0.7f, 0.3f, 0.3f, 0.0f , 0.0f , 0.0f, 0.0f, 0.33f, 0.0f}, // relaxed
        {0.0f , 0.0f , 0.0f, 1.0f, 0.0f, 0.0f  , 0.0f, 0.0f  , 0.0f  , 0.0f , 1.0f, 0.0f, 0.0f, 0.0f, 0.9f, 0.9f, 0.0f, 0.0f, 0.0f , 0.0f , 0.0f, 0.0f, 0.2f , 0.0f}, // excited
        {0.0f , 0.0f , 0.0f, 0.0f, 0.0f, 1.0f  , 0.0f, 0.0f  , 0.0f  , 0.0f , 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f , 0.0f , 1.0f, 0.9f, 0.0f , 0.0f}, // irritated
        {0.0f , 0.0f , 0.0f, 0.0f, 1.0f, 0.0f  , 0.0f, 0.2f  , 0.0f  , 0.25f, 0.0f, 0.0f, 0.0f, 0.0f, 0.7f, 0.7f, 1.0f, 1.0f, 0.0f , 0.0f , 0.0f, 0.0f, 1.0f , 0.0f}, // sad
        {0.145f, 0.4375f, 0.175f, 0.3f, 0.0f  , 0.0f    , 0.0f , 0.0f   , 0.0f    , 0.3325f, 0.25f, 0.0f, 0.0f, 0.0f, 0.45f , 0.45f , 0.15f, 0.15f, 0.0f , 0.0f , 0.0f , 0.0f  , 0.3075f, 0.0f},   //virtual: calm x relaxed x cheerful x excited
        {0.125f, 0.25f  , 0.0f  , 0.3f, 0.025f, 0.25f   , 0.0f , 0.1465f, 0.13475f, 0.0f   , 0.25f, 0.0f, 0.0f, 0.0f, 0.275f, 0.275f, 0.0f , 0.0f , 0.11f, 0.11f, 0.25f, 0.225f, 0.1325f, 0.025f}, // virtual: irritated x tense x cheerful x excited
        {0.0f  , 0.165f , 0.0f  , 0.0f, 0.275f, 0.28125f, 0.25f, 0.265f , 0.13475f, 0.1625f, 0.0f , 0.0f, 0.0f, 0.0f, 0.425f, 0.425f, 0.25f, 0.25f, 0.11f, 0.11f, 0.25f, 0.225f, 0.3325f, 0.025f}, // virtual: tense x irritated x sad x bored
        {0.02f , 0.3525f, 0.175f, 0.0f, 0.25f , 0.03125f, 0.25f, 0.1185f, 0.0f    , 0.495f , 0.0f , 0.0f, 0.0f, 0.0f, 0.6f  , 0.6f  , 0.4f , 0.4f , 0.0f , 0.0f , 0.0f , 0.0f  , 0.5075f, 0.0f}    // virtual: bored x sad x relaxed x calm
    };
    //  { A   ,  B   ,  C  ,  D  ,  E  ,  F    ,  G  ,  H    ,  I    ,  J   ,  K  ,  L  ,  M  ,  N  ,  O  ,  P  ,  Q   ,  R ,  S   ,  T   ,  U  ,  V  ,  W   ,  X  }


    static float[,] faceKeysPolarHighRes = {
        {0.0f , 0.0f  , 0.0f , 0.0f, 0.0f , 0.0f   , 0.0f, 0.0f  , 0.0f   , 0.0f  , 0.0f, 0.0f, 0.0f, 0.0f, 0.0f , 0.0f , 0.0f , 0.0f , 0.0f , 0.0f , 0.0f, 0.0f , 0.37f , 0.0f }, // neutral
        {0.08f, 0.75f , 0.0f , 0.0f, 0.0f , 0.0f   , 0.0f, 0.0f  , 0.0f   , 0.33f , 0.0f, 0.0f, 0.0f, 0.0f, 0.0f , 0.0f , 0.3f , 0.3f , 0.0f , 0.0f , 0.0f, 0.0f , 0.37f , 0.0f }, // calm
        {0.29f, 0.875f, 0.0f , 0.1f, 0.0f , 0.0f   , 0.0f, 0.0f  , 0.0f   , 0.165f, 0.0f, 0.0f, 0.0f, 0.0f, 0.1f , 0.1f , 0.15f, 0.15f, 0.0f , 0.0f , 0.0f, 0.0f , 0.35f , 0.0f }, // calm/cheerful
        {0.5f , 1.0f  , 0.0f , 0.2f, 0.0f , 0.0f   , 0.0f, 0.0f  , 0.0f   , 0.0f  , 0.0f, 0.0f, 0.0f, 0.0f, 0.2f , 0.2f , 0.0f , 0.0f , 0.0f , 0.0f , 0.0f, 0.0f , 0.33f , 0.0f }, // cheerful
        {0.25f, 0.5f  , 0.0f , 0.1f, 0.05f, 0.0f   , 0.0f, 0.293f, 0.2695f, 0.0f  , 0.0f, 0.0f, 0.0f, 0.0f, 0.1f , 0.1f , 0.0f , 0.0f , 0.22f, 0.22f, 0.0f, 0.0f , 0.165f, 0.05f}, // cheerful/tense
        {0.0f , 0.0f  , 0.0f , 0.0f, 0.1f , 0.0f   , 0.0f, 0.586f, 0.539f , 0.0f  , 0.0f, 0.0f, 0.0f, 0.0f, 0.0f , 0.0f , 0.0f , 0.0f , 0.44f, 0.44f, 0.0f, 0.0f , 0.0f  , 0.1f }, // tense
        {0.0f , 0.33f , 0.0f , 0.0f, 0.05f, 0.0625f, 0.5f, 0.43f , 0.2695f, 0.2f  , 0.0f, 0.0f, 0.0f, 0.0f, 0.5f , 0.5f , 0.0f , 0.0f , 0.22f, 0.22f, 0.0f, 0.0f , 0.165f, 0.05f}, // tense/bored
        {0.0f , 0.66f , 0.0f , 0.0f, 0.0f , 0.125f , 1.0f, 0.274f, 0.0f   , 0.4f  , 0.0f, 0.0f, 0.0f, 0.0f, 1.0f , 1.0f , 0.0f , 0.0f , 0.0f , 0.0f , 0.0f, 0.0f , 0.33f , 0.0f }, // bored
        {0.04f, 0.705f, 0.0f , 0.0f, 0.0f , 0.0625f, 0.5f, 0.137f, 0.0f   , 0.365f, 0.0f, 0.0f, 0.0f, 0.0f, 0.5f , 0.5f , 0.15f, 0.15f, 0.0f , 0.0f , 0.0f, 0.0f , 0.35f , 0.0f }, // bored/calm
        {0.0f , 0.0f  , 0.7f , 0.0f, 0.0f , 0.0f   , 0.0f, 0.0f  , 0.0f   , 1.0f  , 0.0f, 0.0f, 0.0f, 0.0f, 0.7f , 0.7f , 0.3f , 0.3f , 0.0f , 0.0f , 0.0f, 0.0f , 0.33f , 0.0f }, // relaxed
        {0.0f , 0.0f  , 0.35f, 0.5f, 0.0f , 0.0f   , 0.0f, 0.0f  , 0.0f   , 0.0f  , 0.0f, 0.0f, 0.0f, 0.0f, 0.8f , 0.8f , 0.15f, 0.15f, 0.0f , 0.0f , 0.0f, 0.0f , 0.265f, 0.0f }, // relaxed/excited
        {0.0f , 0.0f  , 0.0f , 1.0f, 0.0f , 0.0f   , 0.0f, 0.0f  , 0.0f   , 0.0f  , 1.0f, 0.0f, 0.0f, 0.0f, 0.9f , 0.9f , 0.0f , 0.0f , 0.0f , 0.0f , 0.0f, 0.0f , 0.2f  , 0.0f }, // excited
        {0.0f , 0.0f  , 0.0f , 0.2f, 0.0f , 0.2f   , 0.0f, 0.0f  , 0.0f   , 0.0f  , 0.1f, 0.0f, 0.0f, 0.0f, 0.45f, 0.45f, 0.0f , 0.0f , 0.0f , 0.0f , 0.5f, 0.45f, 0.1f  , 0.0f }, // excited/irritated
        {0.0f , 0.0f  , 0.0f , 0.0f, 0.0f , 1.0f   , 0.0f, 0.0f  , 0.0f   , 0.0f  , 0.0f, 0.0f, 0.0f, 0.0f, 0.0f , 0.0f , 0.0f , 0.0f , 0.0f , 0.0f , 1.0f, 0.9f , 0.0f  , 0.0f }, // irritated
        {0.0f , 0.0f  , 0.0f , 0.0f, 0.5f , 0.5f   , 0.0f, 0.1f  , 0.0f   , 0.125f, 0.0f, 0.0f, 0.0f, 0.0f, 0.35f, 0.35f, 0.5f , 0.5f , 0.0f , 0.0f , 0.5f, 0.45f, 0.5f  , 0.0f }, // irritated/sad
        {0.0f , 0.0f  , 0.0f , 0.0f, 1.0f , 0.0f   , 0.0f, 0.2f  , 0.0f   , 0.25f , 0.0f, 0.0f, 0.0f, 0.0f, 0.7f , 0.7f , 1.0f , 1.0f , 0.0f , 0.0f , 0.0f, 0.0f , 1.0f  , 0.0f }, // sad
        {0.0f , 0.0f  , 0.25f, 0.0f, 0.0f , 0.0f   , 0.0f, 0.1f  , 0.0f   , 0.5f  , 0.0f, 0.0f, 0.0f, 0.0f, 0.7f , 0.7f , 0.65f, 0.65f, 0.0f , 0.0f , 0.0f, 0.0f , 0.665f, 0.0f }  // sad/relaxed
    };
    /*  { A   ,  B    ,  C   ,  D  ,  E   ,  F     ,  G  ,  H    ,  I     ,  J    ,  K  ,  L  ,  M  ,  N  ,  O   ,  P   ,  Q   ,  R   ,  S   ,  T   ,  U  ,  V   ,  W    ,  X   }
     * 
     * 
     * A: Mouth Happy
     * B: Mouth Angle
     * C: Mouth Angled +
     * D: Mouth Lively
     * E: Mouth Sad
     * F: Mouth Annoyed turned 
     * G: Mouth Bored
     * H: Mouth Tense vertical
     * I: Mouth Tense horizontal
     * J: Eyelids Close
     * K: Eyelids Close lively
     * L: Eyelids Open
     * M: Eyelid forward R
     * N: Eyelid forward L
     * O: Eyebrow Up R
     * P: Eyebrow Up L
     * Q: Eyebrow Down R
     * R: Eyebrow Down L
     * S: Eyebrow Down+ R
     * T: Eyebrow Down+ L
     * U: Eyebrow Angry R
     * V: Eyebrow Angry L
     * W: Eyebrow Inner Up
     * X: Eyebrow Tense
     */




    /*--------------------------------------------------------------------------------*/

    public enum FacialKeys
    {
        MOUTH_HAPPY = 0,
        MOUTH_ANGLED = 1,
        MOUTH_ANGLED_EXTRA = 2,
        MOUTH_LIVELY = 3,
        MOUTH_SAD = 4,
        MOUTH_ANNOYED = 5,
        MOUTH_BORED = 6,
        MOUTH_TENSE_VERTICAL = 7,
        MOUTH_TENSE_HORIZONTAL = 8,
        EYELIDS_CLOSE = 9,
        EYELIDS_CLOSE_LIVELY = 10,
        EYELIDS_OPEN = 11,
        EYELIDS_R_FORWARD = 12,
        EYELIDS_L_FORWARD = 13,
        EYEBROW_R_UP = 14,
        EYEBROW_L_UP = 15,
        EYEBROW_R_DOWN = 16,
        EYEBROW_L_DOWN = 17,
        EYEBROW_R_DOWN_EXTRA = 18,
        EYEBROW_L_DOWN_EXTRA = 19,
        EYEBROW_R_ANGRY = 20,
        EYEBROW_L_ANGRY = 21,
        EYEBROW_INNER_UP = 22,
        EYEBROW_TENSE = 23
    };


    /*--------------------------------------------------------------------------------*/

    static float[,] eyeKeys = {
        {0.0f, 0.0f , 0.0f , 0.0f }, // neutral
        {0.0f, 0.0f , 0.05f, 0.0f }, // calm 
        {0.0f, 0.0f , 0.0f , 0.0f }, // cheerful
        {0.0f, 0.33f, 0.0f , 0.75f}, // tense
        {0.0f, 0.25f, 0.0f , 0.7f }, // bored
        {0.0f, 0.0f , 0.0f , 0.0f }, // relaxed
        {0.0f, 0.0f , 0.0f , 0.0f }, // excited
        {0.0f, 0.0f , 0.0f , 0.0f }, // irritated
        {0.0f, 0.0f , 0.15f, 0.0f }, // sad
        {0.0f, 0.0f   , 0.0125f, 0.0f},    //virtual: calm x relaxed x cheerful x excited
        {0.0f, 0.0825f, 0.0f   , 0.1875f}, // virtual: irritated x tense x cheerful x excited
        {0.0f, 0.145f , 0.0375f, 0.3625f}, // virtual: tense x irritated x sad x bored
        {0.0f, 0.0625f, 0.05f  , 0.175f}   // virtual: bored x sad x relaxed x calm
    };
    //  { a  ,  b   ,  c   ,  d   }

    static float[,] eyeKeysPolarHighRes = {
        {0.0f, 0.0f  , 0.0f  , 0.0f  }, // neutral
        {0.0f, 0.0f  , 0.05f , 0.0f  }, // calm 
        {0.0f, 0.0f  , 0.025f, 0.0f  }, // calm/cheerful
        {0.0f, 0.0f  , 0.0f  , 0.0f  }, // cheerful
        {0.0f, 0.165f, 0.0f  , 0.375f}, // cheerful/tense
        {0.0f, 0.33f , 0.0f  , 0.75f }, // tense
        {0.0f, 0.29f , 0.0f  , 0.725f}, // tense/bored
        {0.0f, 0.25f , 0.0f  , 0.7f  }, // bored
        {0.0f, 0.125f, 0.025f, 0.35f }, // bored/calm
        {0.0f, 0.0f  , 0.0f  , 0.0f  }, // relaxed
        {0.0f, 0.0f  , 0.0f  , 0.0f  }, // relaxed/excited
        {0.0f, 0.0f  , 0.0f  , 0.0f  }, // excited
        {0.0f, 0.0f  , 0.0f  , 0.0f  }, // excited/irritated
        {0.0f, 0.0f  , 0.0f  , 0.0f  }, // irritated
        {0.0f, 0.0f  , 0.075f, 0.0f  }, // irritated/sad
        {0.0f, 0.0f  , 0.15f , 0.0f  }, // sad
        {0.0f, 0.0f  , 0.075f, 0.0f  }  // sad/relaxed
    };
    /*  { a  ,  b    ,  c    ,  d    }
     * 
     *
     * a: Look right
     * b: Look up
     * c: Look down
     * d: Look left
     */

    /*--------------------------------------------------------------------------------*/

    public static float getKeyShapeVal(Part part, Mood mood, int keyVal){
        switch (part)
        {
            case Part.EYES:
                if (keyVal >= MAX_EYE_KEYS) return 0.0f;
                return (eyeKeys[(int)mood, keyVal] * 100);
            case Part.FACE:
                if (keyVal >= MAX_FACE_KEYS) return 0.0f;
                return (faceKeys[(int)mood, keyVal] * 100);
            default:
                return 0.0f;

        }
    }

    /*--------------------------------------------------------------------------------*/

    public static float getKeyShapeVal(Part part, MoodHighRes mood, int keyVal)
    {
        switch (part)
        {
            case Part.EYES:
                if (keyVal >= MAX_EYE_KEYS) return 0.0f;
                return (eyeKeysPolarHighRes[(int)mood, keyVal] * 100);
            case Part.FACE:
                if (keyVal >= MAX_FACE_KEYS) return 0.0f;
                return (faceKeysPolarHighRes[(int)mood, keyVal] * 100);
            default:
                return 0.0f;

        }
    }

    /*--------------------------------------------------------------------------------*/

    public static void getRatios(Vector2 position, ref float[] ratios){
        float totalLength = 0;
        for (int i = 0; i < mappings.Length; ++i){
            ratios[i] = Vector2.Distance(position, mappings[i]);
            totalLength += ratios[i];
        }
        for (int i = 0; i < ratios.Length; ++i)
            ratios[i] = ratios[i]/totalLength;
    }

    /*--------------------------------------------------------------------------------*/

    public static float getValence(Mood mood)
    {
        return mappings[(int)mood][0];
    }

    /*--------------------------------------------------------------------------------*/

    public static float getArousal(Mood mood)
    {
        return mappings[(int)mood][1];
    }

    /*--------------------------------------------------------------------------------*/

    public static float getValence(MoodHighRes mood)
    {
        return valenceArousal[(int)mood][0];
    }

    /*--------------------------------------------------------------------------------*/

    public static float getArousal(MoodHighRes mood)
    {
        return valenceArousal[(int)mood][1];
    }
}
