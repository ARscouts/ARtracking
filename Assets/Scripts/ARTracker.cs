using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARTracker : MonoBehaviour
{
    public Camera camera;
    public GameEvent ClueInSightEvent;
    public GameEvent LostSightEvent;
    public GameEvent ClueFoundEvent;
    public FloatVariable loadingBarFill;
    public float timeToAquireClue;

    private ARSessionOrigin arOrigin;
    private Ray ray;
    private bool clueSighted = false;
    private float timeLastFrame;
    private float timeThisFrame;

    // Start is called before the first frame update
    void Start()
    {
        arOrigin = FindObjectOfType<ARSessionOrigin>();
    }

    // Update is called once per frame
    void Update()
    {
        timeLastFrame = timeThisFrame;
        timeThisFrame = Time.timeSinceLevelLoad;

        ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform.tag == "Clue")
            {
                InSightAction();
            }
            else
            {
                
                NotInSightAction();
             
            }
        } else
        {
            NotInSightAction();
        }
    }

    private void InSightAction()
    {
        if (!clueSighted)
        {
            ClueInSightEvent.Raise();
            //Debug.LogWarning("############## FOUND A CLUE");
            clueSighted = true;
        }
        float timeElapsed = timeThisFrame - timeLastFrame;
        loadingBarFill.Value += timeElapsed / timeToAquireClue;

        if (loadingBarFill.Value >= 1.0f)
        {
            loadingBarFill.Value = 1.0f;
            clueSighted = false;
            ClueFoundEvent.Raise();
        }
    }

    private void NotInSightAction()
    {
        if (clueSighted)
        {
            float timeElapsed = timeThisFrame - timeLastFrame;
            loadingBarFill.Value -= timeElapsed / timeToAquireClue;

            if (loadingBarFill.Value <= 0.0f)
            {
                loadingBarFill.Value = 0.0f;
                clueSighted = false;
                LostSightEvent.Raise();
            }
        }
    }
}
