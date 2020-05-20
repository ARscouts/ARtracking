using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class ARTracker : MonoBehaviour
{
    public Camera Camera;
    public GameManager GameManager;
    public GameEvent ClueInSightEvent;
    public GameEvent LostSightEvent;
    public ClueMarkerPassableEvent ClueFoundEvent;
    public ClueMarkerPassableEvent ClueCaptureBegin;

    public FloatVariable loadingBarFill;
    public ClueRuntimeSet clues;
    public LocationVariable startingLocation;
    public LocationVariable currentLocation;
    public LocationVariable scaleApprox;

    public float timeToAquireClue;
    public float clueVibrationDistanceThreshold;
    public float distanceToStartClueCapture; //should be much smaller than clueVibrationDistanceThreshold
    //public float vibrationInterval;

   // private ARSessionOrigin arOrigin;
    private Ray ray;
    private bool clueSighted = false;
    private float timeLastFrame;
    private float timeThisFrame;
    private float distanceToClosestClue; //distance to closest clue power of 2
    private ClueMarker closestClue;

    private bool vibrating = false;

    public Text DebugText;
    public Text MessageBox;

    // Start is called before the first frame update
    void Start()
    {
        //arOrigin = FindObjectOfType<ARSessionOrigin>();
        ResetLoadingBar();
    }

    // Update is called once per frame
    void Update()
    {
        timeLastFrame = timeThisFrame;
        timeThisFrame = Time.timeSinceLevelLoad;

        //----------- Here we look for closest clue
        if (GameManager.CurrentGameState == GameState.GS_CLUE_TRACKING)
        {
            UpdateClosestClue();
            if (distanceToClosestClue <= distanceToStartClueCapture)
            {
                ClueCaptureBegin.Raise((ClueMarker)closestClue);
            }
            else if (distanceToClosestClue <= clueVibrationDistanceThreshold)
            {
                StartCoroutine(Vibrate());
            }
        }
        else if (GameManager.CurrentGameState == GameState.GS_CLOSE_TO_CLUE)
        {
            MessageBox.text = "A clue is very near!";
            //----------- Here we look for clue objects via raycast
            ray = Camera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform.CompareTag("Clue"))
                {
                    InSightAction();
                }
                else
                {

                    NotInSightAction();

                }
            }
            else
            {
                NotInSightAction();
            }
        }

        //----------- Take the closest clue and vibrate depending on distance
    }

    private IEnumerator Vibrate()
    {
        if (vibrating) yield break;
        else
        {
            vibrating = true;
            float interval = distanceToClosestClue / clueVibrationDistanceThreshold;
            WaitForSeconds wait = new WaitForSeconds(interval);
            float t;
            for (t = 0; t < 1; t += interval) // Change the end condition (t < 1) if you want
            {
                MessageBox.text = "Vibrating!";
                Handheld.Vibrate();
                yield return wait;
                MessageBox.text = "";
                yield return wait;
            }

            //yield return new WaitForSeconds(2f);

            //for (t = 0; t < 1; t += interval) // Change the end condition (t < 1) if you want
            //{
            //    Handheld.Vibrate();
            //    yield return wait;
            //}
            vibrating = false;
            yield break;
        }
    }

    private void UpdateClosestClue()
    {
        float closestDistance2 = -1.0f;

        float currentLatInMeters = (currentLocation.Lat - startingLocation.Lat) * scaleApprox.Lat;
        float currentLonInMeters = (currentLocation.Lon - startingLocation.Lon) * scaleApprox.Lon;

        DebugText.text = "Clue distances: "; //----------- ONLY FOR DEBUG

        foreach (ClueMarker cm in clues.Items)
        {
            float latDist = cm.LatInMeters - currentLatInMeters;
            float lonDist = cm.LonInMeters - currentLonInMeters;
            float dist2 = lonDist * lonDist + latDist * latDist;

            //----------- ONLY FOR DEBUG
            float sqrtdist = (float)Math.Sqrt(dist2);
            DebugText.text += "\n" + sqrtdist;

            if (dist2 < closestDistance2 || closestDistance2 == -1)
            {
                closestDistance2 = dist2;
                closestClue = cm;
            }
        };

        distanceToClosestClue = (float)Math.Sqrt(closestDistance2);
        DebugText.text += "\nClosest distance = " + distanceToClosestClue; //DEBUG TEXT
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
            clueSighted = false;
            ClueFoundEvent.Raise(closestClue);
            ResetLoadingBar();
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
                clueSighted = false;
                LostSightEvent.Raise();
                ResetLoadingBar();
            }
        }
    }

    public void ResetLoadingBar()
    {
        loadingBarFill.Value = 0.0f;
    }
}
