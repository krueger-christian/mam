//#define OCULUS
#define REPORT_ACTIVE

using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class MorphAMoodCtrl : MonoBehaviour
{
    private enum STATE { INIT, SELECT, CONFIRM };

#if OCULUS
    private OVRInput.Controller CONTROLLER = OVRInput.Controller.RTrackedRemote;
#endif

    [Tooltip("Disables the report and the event handling after confirmation.")]
    public bool playgroundMode = false;

    #region MAPPING_ALGORITHM
    public enum MAPPING_TYPE { POLAR, CARTESIAN };
    [Header("Computation Model")]
    [Tooltip("The algorithm used to interpret the cursor space")]
    public MAPPING_TYPE mapping = MAPPING_TYPE.POLAR;
    private PolarMapping polarMapping;
    private MeshMapping meshMapping;
    #endregion

    #region ACTION_AFTER_CONFIRM
    public enum END_ACTION { LOAD_SCENE, CALLBACK, NONE};
    [Header("Event performed after confirmation")]
    [Tooltip("The type of event being executed after confirming the edited expression. This is disabled by default in playground mode.")]
    public END_ACTION eventOnEnd = END_ACTION.NONE;

    [Tooltip("The scene that will be loaded after confirming the rating")]
    public string sceneToLoad;

    [Tooltip("The method that will be called after confirming the rating")]
    public UnityEvent eventAfterConfirm = null;
    #endregion

    #region GUI_SETTINGS
    [Header("GUI Settings")]
    public Character2DCtrl characterCtrl;
    public IndicatorGrid indicatorGrid;

    #region CURSOR_CTRL
    public float cursorSpeed = 8.0f;
    private Vector2 lastControllerPosition;
    #endregion
    #endregion

    #region REPORT
    [Header("Export Settings")]
    public Report report;
    [Tooltip("Define an ID that helps to identify what has been rated")]
    public string reportID;
    #endregion

    private bool selectionExecuted = false;

    /*--------------------------------------------------------------------------------*/

    void Start()
    {
        SetCursorSpeed(8);

        if(mapping == MAPPING_TYPE.POLAR){
            polarMapping = new PolarMapping();
            polarMapping.HighResolutionActive(true);
            polarMapping.characterCtrl = characterCtrl;
            polarMapping.indicatorGrid = indicatorGrid;
        }
        else{
            meshMapping = new MeshMapping(indicatorGrid);
            meshMapping.characterCtrl = characterCtrl;
        }



        selectionExecuted = false;
        report.AddLine(RatiosToReportLine(STATE.INIT));
    }

    /*--------------------------------------------------------------------------------*/

    void Update()
    {
#if OCULUS
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
            OnTriggerButtonDown();
        else if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
            OnControllerDrag();
        else if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger))
            OnTriggerButtonUp();

        if (OVRInput.GetDown(OVRInput.Button.PrimaryTouchpad) && selectionExecuted)
        {
            Confirm();
        }
#else
        if (Input.GetMouseButtonDown(0)) OnMouseDown();
        else if (Input.GetMouseButton(0)) OnMouseDrag();
        else if (Input.GetMouseButtonUp(0)) OnMouseUp();

        if (Input.GetKeyDown(KeyCode.E) && selectionExecuted)
            Confirm();
#endif
    }

    /*--------------------------------------------------------------------------------*/

    public void SetCursorSpeed(float speed)
    {
        cursorSpeed = speed;
#if !OCULUS
        cursorSpeed *= 0.01f;
#endif
    }

    /*--------------------------------------------------------------------------------*/

#if OCULUS
#region OCULUS_CONTROL
    private Vector3 ControllerPosition()
    {
        return OVRInput.GetLocalControllerPosition(CONTROLLER);
    }

    /*--------------------------------------------------------------------------------*/

    private void OnTriggerButtonDown()
    {
        lastControllerPosition = ControllerPosition();
    }

    /*--------------------------------------------------------------------------------*/

    private void OnTriggerButtonUp()
    {
        selectionExecuted = true;
        
#if REPORT_ACTIVE
        report.AddLine(RatiosToReportLine(STATE.SELECT));
#endif
    }

    /*--------------------------------------------------------------------------------*/

    private void OnControllerDrag()
    {
        Vector2 position = ControllerPosition();
        Vector2 movement = position - lastControllerPosition;
        movement *= cursorSpeed;

        if (mapping == MAPPING_TYPE.POLAR)
            polarMapping.MoveCursor(movement);
        else
            meshMapping.MoveCursor(movement);

        lastControllerPosition = position;
    }

#endregion
#else

    /*--------------------------------------------------------------------------------*/

    private void OnMouseUp()
    {
        selectionExecuted = true;

#if REPORT_ACTIVE
        report.AddLine(RatiosToReportLine(STATE.SELECT));
#endif
    }

    /*--------------------------------------------------------------------------------*/

    private void OnMouseDown()
    {
        lastControllerPosition = Input.mousePosition;
    }

    /*--------------------------------------------------------------------------------*/

    private void OnMouseDrag()
    {
        Vector2 movement = Input.mousePosition;
        movement -= lastControllerPosition;
        movement *= cursorSpeed;

        if (mapping == MAPPING_TYPE.POLAR)
            polarMapping.MoveCursor(movement);
        else
            meshMapping.MoveCursor(movement);

        lastControllerPosition = Input.mousePosition;
    }
#endif

    /*--------------------------------------------------------------------------------*/

    private string RatiosToReportLine(STATE state)
    {
        string line = Report.CSV_QUOTATION_MARK;

        switch (state)
        {
            case STATE.INIT:
                line += "init" + Report.CSV_COMMA;
                break;
            case STATE.SELECT:
                line += "select" + Report.CSV_COMMA;
                break;
            case STATE.CONFIRM:
                line += "confirm" + Report.CSV_COMMA;
                break;
        }

        Vector2 cursor = (mapping == MAPPING_TYPE.POLAR) ? polarMapping.GetVA() : meshMapping.cursor;

        line += reportID + Report.CSV_COMMA;
        line += cursor.x.ToString() + Report.CSV_COMMA;
        line += cursor.y.ToString() + Report.CSV_COMMA;

        Debug.Log(line);

        string timestamp = DateTime.Now.ToString("G", DateTimeFormatInfo.InvariantInfo);
        line += timestamp + Report.CSV_ENDL;

        return line;
    }

    /*--------------------------------------------------------------------------------*/

    public void ExportReport()
    {
        if (playgroundMode) return;

#if REPORT_ACTIVE
        report.Export(Report.SAVETYPE.APPEND);
#endif
    }

    /*--------------------------------------------------------------------------------*/

    private void Confirm()
    {
        if (!playgroundMode)
        {
            ExportReport();
            report.AddLine(RatiosToReportLine(STATE.CONFIRM));
            switch (eventOnEnd)
            {
                case END_ACTION.CALLBACK:
                    if (eventAfterConfirm != null)
                    {
                        eventAfterConfirm.Invoke();
                    }
                    break;
                case END_ACTION.LOAD_SCENE:
                    if (sceneToLoad != "")
                    {
                        SceneManager.LoadScene(sceneToLoad);
                    }
                    break;
                case END_ACTION.NONE:
                    break;
            }
        }
    }
}
