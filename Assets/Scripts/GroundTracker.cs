using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARSubsystems;

public class GroundTracker : MonoBehaviour
{
    public Camera camera;
    public FloatVariable groundLevel;
    public GameObject cube; //cube for debug

    private ARRaycastManager arRayManager;
    private Pose placementPose;
    private bool poseIsValid = false;
    // Start is called before the first frame update
    void Start()
    {
        arRayManager = GetComponent<ARRaycastManager>();
        groundLevel.Value = -1.1f; //default ground level
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGroundLevel();
        UpdatePlacementCube();
    }

    private void UpdatePlacementCube()
    {
        if (poseIsValid)
        {
            cube.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }
    }

    private void UpdateGroundLevel()
    {
        List<List<ARRaycastHit>> hits = new List<List<ARRaycastHit>>();
        List<ARRaycastHit> hit = new List<ARRaycastHit>();

        hits.Add(hit);

        var scanPoint = camera.ViewportToScreenPoint(new Vector3(0.5f, 0.15f));
        arRayManager.Raycast(scanPoint, hits[0], TrackableType.Planes);

        poseIsValid = hits[0].Count > 0;

        if (poseIsValid)
        {
            placementPose = hits[0][0].pose;
            groundLevel.Value = placementPose.position.y;
            //Debug.LogWarning(groundLevel.Value);
        }
    }
}
