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
    public GameEvent CaptureCompleteEvent;
    public ClueMarkerPassableEvent ClueFoundEvent;
    public AnimalMarkerPassableEvent AnimalFoundEvent;
    public ARMarkerPassableEvent MarkerCloseEvent;
    public ARMarkerPassableEvent MarkerLostEvent;

    public ClueRuntimeSet clues;
    public ARMarkerRuntimeSet markers;
    public ARMarkerRuntimeSet activeMarkers;
    public ARMarkerRuntimeSet hiddenMarkers;

    public FloatVariable loadingBarFill;
    public LocationVariable startingLocation;
    public LocationVariable currentLocation;
    public LocationVariable scaleApprox;

    public float timeToAquireClue;
    public float clueVibrationDistanceThreshold;
    public float distanceToStartCapture; //should be much smaller than clueVibrationDistanceThreshold

    private float distanceToStartCapture2; //distance to start capture pow 2
   // private ARSessionOrigin arOrigin;
    private Ray ray;
    private bool markerSighted = false;
    private float timeLastFrame;
    private float timeThisFrame;
    private float distanceToClosestMarker; //distance to closest clue power of 2
    private float distanceToARMarker;
    private ARMarker closestMarker;

    private bool vibrating = false;

    public Text DebugText;
    public Text MessageBox;

    // Start is called before the first frame update
    void Start()
    {
        //arOrigin = FindObjectOfType<ARSessionOrigin>();
        ResetLoadingBar();
        //distanceToStartCapture2 = distanceToStartCapture * distanceToStartCapture;
    }

    // Update is called once per frame
    void Update()
    {
        distanceToStartCapture2 = distanceToStartCapture * distanceToStartCapture;

        timeLastFrame = timeThisFrame;
        timeThisFrame = Time.timeSinceLevelLoad;

        //----------- Here we look for closest clue
        if (GameManager.CurrentGameState == GameState.GS_TRACKING)
        {
            UpdateCloseMarkers();

            if (distanceToClosestMarker <= clueVibrationDistanceThreshold)
            {
                StartCoroutine(Vibrate(distanceToClosestMarker));
            }

            //----------- Here we look for clue objects via raycast
            ray = Camera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform.CompareTag("Clue") || hit.transform.CompareTag("Animal"))
                {
                    MarkerInSightAction(hit);
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
        //else if (GameManager.CurrentGameState == GameState.GS_CLOSE_TO_CLUE || GameManager.CurrentGameState == GameState.GS_CLOSE_TO_ANIMAL) 
        //{

        //    if (distanceToARMarker >= 2 * distanceToStartCapture)
        //    {
        //        //MarkerTooFarEvent.Raise();
        //    }
        //    StartCoroutine(Vibrate(distanceToARMarker)); //change vibration to depend on inGame distance

            
        //}

        //----------- Take the closest clue and vibrate depending on distance
    }

    //private void AnimalInSightAction()
    //{
    //    if (!markerSighted)
    //    {
    //        ClueInSightEvent.Raise();
    //        //Debug.LogWarning("############## FOUND A CLUE");
    //        markerSighted = true;
    //    }
    //    float timeElapsed = timeThisFrame - timeLastFrame;
    //    loadingBarFill.Value += timeElapsed / timeToAquireClue;

    //    if (loadingBarFill.Value >= 1.0f)
    //    {
    //        markerSighted = false;
    //        AnimalFoundEvent.Raise();
    //        ResetLoadingBar();
    //    }
    //}

    private IEnumerator Vibrate(float distance)
    {
        if (vibrating) yield break;
        else
        {
            vibrating = true;
            float interval = distance / clueVibrationDistanceThreshold;
            WaitForSeconds wait = new WaitForSeconds(interval);
            float t;
            for (t = 0; t < 1; t += interval) // Change the end condition (t < 1) if you want
            {
                //MessageBox.text = "Vibrating!";
                Handheld.Vibrate();
                yield return wait;
                //MessageBox.text = "";
                //yield return wait;
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

    private void UpdateCloseMarkers()
    {
        float closestDistance2 = -1.0f;

        float currentLatInMeters = (currentLocation.Lat - startingLocation.Lat) * scaleApprox.Lat;
        float currentLonInMeters = (currentLocation.Lon - startingLocation.Lon) * scaleApprox.Lon;

        DebugText.text = "Clue distances: "; //----------- ONLY FOR DEBUG

        //get early size of active so you wont have to check moved items in this frame
        int activeMarkersCount = activeMarkers.Items.Count;

        //find markers close enough to spawn
        for (int i = hiddenMarkers.Items.Count - 1; i >= 0;  i--)
        {
            ARMarker am = hiddenMarkers.Items[i];
            float latDist = am.LatInMeters - currentLatInMeters;
            float lonDist = am.LonInMeters - currentLonInMeters;
            float dist2 = lonDist * lonDist + latDist * latDist;

            if (dist2 <= distanceToStartCapture2)
            {
                MarkerCloseEvent.Raise(am);
            }

            //----------- ONLY FOR DEBUG
            float sqrtdist = (float)Math.Sqrt(dist2);
            DebugText.text += "\n" + sqrtdist;

            if (dist2 < closestDistance2 || closestDistance2 == -1)
            {
                closestDistance2 = dist2;
                closestMarker = am;
            }
        }

        //check if got too far from any marker
        for (int i = activeMarkersCount - 1; i >= 0; i--)
        {
            ARMarker am = activeMarkers.Items[i];
            closestDistance2 = -1;

            float dist2 = (float)Math.Pow(Vector3.Distance(Camera.transform.position, am.transform.position), 2);

            //----------- ONLY FOR DEBUG
            float sqrtdist = (float)Math.Sqrt(dist2);
            DebugText.text += "\n" + sqrtdist;

            if (dist2 > distanceToStartCapture2 + 5.0f) //to avoid problems with margin of error give it 5 more meters to stay in ARScene
            {
                MarkerLostEvent.Raise(am);
            } 
            else if (dist2 < closestDistance2 || closestDistance2 == -1)
            {
                closestDistance2 = dist2;
                closestMarker = am;
            }
        }

        distanceToClosestMarker = (float)Math.Sqrt(closestDistance2);
        DebugText.text += "\nClosest distance = " + distanceToClosestMarker; //DEBUG TEXT
    }

    private void MarkerInSightAction(RaycastHit hit)
    {
        if (!markerSighted)
        {
            ClueInSightEvent.Raise();
            //Debug.LogWarning("############## FOUND A CLUE");
            markerSighted = true;
        }
        float timeElapsed = timeThisFrame - timeLastFrame;
        loadingBarFill.Value += timeElapsed / timeToAquireClue;

        if (loadingBarFill.Value >= 1.0f)
        {
            markerSighted = false;
            if (hit.transform.CompareTag("Animal"))
            {
                AnimalFoundEvent.Raise((AnimalMarker)hit.transform.parent.GetComponent(typeof(AnimalMarker))); //Raise animal found event
            }
            else if (hit.transform.CompareTag("Clue"))
            {
                ClueFoundEvent.Raise((ClueMarker)hit.transform.parent.GetComponent(typeof(ClueMarker))); //Raise clue found event
            }
            CaptureCompleteEvent.Raise();
            ResetLoadingBar();
        }
    }

    private void NotInSightAction()
    {
        if (markerSighted)
        {
            float timeElapsed = timeThisFrame - timeLastFrame;
            loadingBarFill.Value -= timeElapsed / timeToAquireClue;

            if (loadingBarFill.Value <= 0.0f)
            {
                markerSighted = false;
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
