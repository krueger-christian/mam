/* This class serves to change the precompiler flags being used in some scripts of the Morph-A-Mood Asset.
 * To change all flags automatically use the Unity editor menu "Edit/MorphAMood/Use Oculus SDK".
 * It will change the "#define OCULUS" line on top of the respective files. Due to the update delay of the editor
 * it can take some seconds, until the editor realized the changes. Switching to another application
 * and going back to the Unity editor might refresh and update.
 */


using UnityEngine;
using UnityEditor;
using System.Linq;
using System.IO;
using System;

public class ToggleSDKMode : MonoBehaviour
{
    private const string MENU_NAME_DISABLE = "Edit/MorphAMood/Disable Oculus SDK";
    private const string MENU_NAME_ENABLE = "Edit/MorphAMood/Enable Oculus SDK";

    private const string ENABLE_TOKEN = "#define OCULUS";
    private const string DISABLE_TOKEN = "//#define OCULUS";

    private static readonly string[] filenames = { 
        "AntiAliasingSettings.cs", 
        "ButtonPress.cs", 
        "IndicatorGrid.cs", 
        "MorphAMoodCtrl.cs", 
        "MoveObjects.cs" 
    };


    [MenuItem(MENU_NAME_DISABLE)]
    static void Disable()
    {
        foreach (var filename in filenames){

            string filepath = Application.dataPath + "/Morph A Mood/Scripts/" + filename;

            string[] lines = File.ReadAllLines(filepath);
            string firstline = File.ReadLines(filepath).First();


            if (firstline == ENABLE_TOKEN)
            {
                File.WriteAllText(filepath, DISABLE_TOKEN + Environment.NewLine);
                File.AppendAllLines(filepath, lines.Skip(1).ToArray());
                Menu.SetChecked(MENU_NAME_DISABLE, true);
                EditorPrefs.SetBool(MENU_NAME_DISABLE, true);
                Menu.SetChecked(MENU_NAME_ENABLE, false);
                EditorPrefs.SetBool(MENU_NAME_ENABLE, false);
            }
        }
    }

    [MenuItem(MENU_NAME_ENABLE)]
    static void Enable()
    {
        foreach (var filename in filenames)
        {

            string filepath = Application.dataPath + "/Morph A Mood/Scripts/" + filename;

            string[] lines = File.ReadAllLines(filepath);
            string firstline = File.ReadLines(filepath).First();


            if (firstline == DISABLE_TOKEN)
            {
                File.WriteAllText(filepath, ENABLE_TOKEN + Environment.NewLine);
                File.AppendAllLines(filepath, lines.Skip(1).ToArray());
                Menu.SetChecked(MENU_NAME_DISABLE, false);
                EditorPrefs.SetBool(MENU_NAME_DISABLE, false);
                Menu.SetChecked(MENU_NAME_ENABLE, true);
                EditorPrefs.SetBool(MENU_NAME_ENABLE, true);
            }
        }
    }
}
