using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.UI;

public class Draw : MonoBehaviour
{
    private IMixedRealityHandJointService handJointService;

    private IMixedRealityHandJointService HandJointService =>
        handJointService ??
        (handJointService = CoreServices.GetInputSystemDataProvider<IMixedRealityHandJointService>());

    private MixedRealityPose? previousLeftHandPose;

    private MixedRealityPose? previousRightHandPose;

    private const float GrabThreshold = 0.02f;

    public Camera m_camera;
    public GameObject brush;

    private Vector3 indexTip;
    private Vector3 thumbTip;

    GameObject lines;
    LineRenderer currentLineRenderer;

    MixedRealityPose pose;

    Vector3 lastPos;

    bool isStartDraw;

    void Start()
    {
        lines = GameObject.Find("Layers");
    }
    private void Update()
    {
        Drawing();

        if(HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip,Handedness.Both,out pose))
        {
            indexTip = pose.Position;
        }
        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.ThumbTip, Handedness.Both, out pose))
        {
            thumbTip = pose.Position;
        }
    }

    void Drawing()
    {
        float drawing = Mathf.Abs(Vector3.Distance(indexTip, thumbTip));
        if(drawing != 0)
        {
            if (drawing > GrabThreshold)
            {
                isStartDraw = false;
            }

            if (drawing < GrabThreshold && !isStartDraw)
            {
                CreateBrush();
                isStartDraw = true;
            } 
            else if(drawing < GrabThreshold && isStartDraw)
            {
                PointToMousePos();
            }
            else
            {
                currentLineRenderer = null;
            }

        }


        //else if (Input.GetKey(KeyCode.Mouse0))
        //{
        //    PointToMousePos();
        //}
        //else
        //{
        //    currentLineRenderer = null;
        //}
    }

    void CreateBrush()
    {
        GameObject brushInstance = Instantiate(brush);
        currentLineRenderer = brushInstance.GetComponent<LineRenderer>();
        //because you gotta have 2 points to start a line renderer, 
        // Vector3 playerWordDir = m_camera.WorldToScreenPoint(Input.mousePosition);

        // Vector3 _mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);

        // Vector3 mousePos = new Vector3(_mousePos.x, _mousePos.y, playerWordDir.z);\
        brushInstance.transform.parent = lines.transform;
        currentLineRenderer.SetPosition(0, indexTip);
        currentLineRenderer.SetPosition(1, indexTip);

    }

    void AddAPoint(Vector3 pointPos)
    {
        currentLineRenderer.positionCount++;
        int positionIndex = currentLineRenderer.positionCount - 1;
        currentLineRenderer.SetPosition(positionIndex, pointPos);
    }

    void PointToMousePos()
    {
        Vector3 _mousePos = indexTip;//(new Vector3(Input.mousePosition.x,Input.mousePosition.y, playerWordDir.z));
        //Debug.Log("mouseWordDir: " + _mousePos);
        //Vector3 _mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 mousePos = new Vector3(_mousePos.x, _mousePos.y, _mousePos.z);//transform.position.z);
        if (lastPos != mousePos)
        {
            AddAPoint(mousePos);
            lastPos = mousePos;
        }
    }

    public void ClearLine()
    {
        GameObject[] DrewLines = new GameObject[lines.transform.childCount];
        for (int i = 0; i < lines.transform.childCount; i++)
        {
            DrewLines[i] = lines.transform.GetChild(i).gameObject;
            Destroy(DrewLines[i]);
        }
    }

}