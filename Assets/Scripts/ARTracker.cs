using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARTracker : MonoBehaviour
{
    public Camera camera;

    private ARSessionOrigin arOrigin;
    private Ray ray;
    // Start is called before the first frame update
    void Start()
    {
        arOrigin = FindObjectOfType<ARSessionOrigin>();
    }

    // Update is called once per frame
    void Update()
    {
        ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform.tag == "Clue")
            {
                //start indicating animation
                //after some time elapses raise cluefound
                Debug.LogWarning("############## FOUND A CLUE");
            }
        } else
        {
            //slow
        }
    }
}
