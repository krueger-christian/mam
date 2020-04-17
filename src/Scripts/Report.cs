/*  Christian Krueger, 2019
 *  Quality & Usability Lab,
 *  Technische Universitaet Berlin
 */

using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.IO;
using System.Text;
using System.Globalization;

using System.Security;
using System.Security.Permissions;

/*================================================================================*/

public class Report : MonoBehaviour {

    public enum SAVETYPE { WRITE_NEW, APPEND };

    private StringBuilder csv = null;

    public const string CSV_COMMA = "\",\"";
    public const string CSV_ENDL = "\"\n";
    public const string CSV_QUOTATION_MARK = "\"";
    public const string CSV_FILE_ENDING = ".csv";


    [Header("Export Settings")]
    [Tooltip("If used with Oculus filepath will be automatically set to the persistant data path of the application")]
    public string filepath;
    public string filename;

    /*--------------------------------------------------------------------------------*/

    public void Start(){
        csv = new StringBuilder();

        // check whether Oculus package is installed
        if ((from assembly in System.AppDomain.CurrentDomain.GetAssemblies()
                 from type in assembly.GetTypes()
                 where type.Name == "OVRInput"
             select type).FirstOrDefault() != null || filepath.Length == 0)
        {
            filepath = Application.persistentDataPath;
        }

        if (filename.Length == 0) filename = "mam_report_";
	}

    /*--------------------------------------------------------------------------------*/

	public void AddLine (string newLine) {
        Debug.Log("[Report] add: " + newLine);
        csv.AppendLine(newLine);
	}

    /*--------------------------------------------------------------------------------*/

    //public bool Export(string myFilepath, string myFilename, SAVETYPE saveType = SAVETYPE.WRITE_NEW)
    //{
    //    return Export(myFilepath, myFilename, saveType);
    //}

    /*--------------------------------------------------------------------------------*/

    public bool Export(SAVETYPE saveType = SAVETYPE.WRITE_NEW)
    {
        string timestamp = DateTime.Now.ToString("G", DateTimeFormatInfo.InvariantInfo);
        timestamp = timestamp.Replace("/", "");
        timestamp = timestamp.Replace(":", "");
        timestamp = timestamp.Replace(" ", "");
        return Export(filepath, filename + timestamp, saveType);
    }

    /*--------------------------------------------------------------------------------*/

    public bool Export(string myFilename, SAVETYPE saveType = SAVETYPE.WRITE_NEW)
    {
        if (myFilename == "")
            return Export();
        else
            return Export(filepath, myFilename, saveType);
    }

    /*--------------------------------------------------------------------------------*/

    public bool Export(string myFilepath, string myFilename, SAVETYPE type){
        if (csv == null || myFilepath == null || myFilepath.Length == 0) 
            return false;

        if (!myFilepath.EndsWith("/") && !myFilename.StartsWith("/"))
            filepath += "/";
        myFilepath += myFilename;

        string appendix = CSV_FILE_ENDING;
        if (type == SAVETYPE.WRITE_NEW)
        {
            int index = 1;
            while (File.Exists(myFilepath + appendix))
            {
                appendix = "_" + index + CSV_FILE_ENDING;
                index++;
            }
        }
        myFilepath += appendix;


        Debug.Log("[Report] Export to: " + myFilepath);
        try
        {
            if(type == SAVETYPE.WRITE_NEW)
                File.WriteAllText(myFilepath, csv.ToString());
            else
                File.AppendAllText(myFilepath, csv.ToString());
        }
        catch(IOException e){
            Debug.Log("[Report] IO Exception: " + e.Message);
            return false;
        }
        catch(UnauthorizedAccessException e){
            Debug.Log("[Report] Unautorized Access Exception: " + e.Message);
            return false;
        }
            
        return true;
    }

    /*--------------------------------------------------------------------------------*/

    public void ShowOnDebugLine(){
        Debug.Log("[Report] CSV: ");
        Debug.Log(csv.ToString());
    }

    /*--------------------------------------------------------------------------------*/

    public static void ReadReport(string myFilepath, string myFilename, out string[] content)
    {
        if (!myFilepath.EndsWith("/") && !myFilename.StartsWith("/"))
            myFilepath += "/";
        myFilepath += myFilename;
        myFilepath += CSV_FILE_ENDING;

        Debug.Log("[Report] Read: " + myFilepath);

        if (File.Exists(myFilepath))
        {
            content = File.ReadAllLines(myFilepath);
            return;
        }
        else
            Debug.Log("[Report] File does not exist: " + myFilepath);

        content = null;
    }
}
