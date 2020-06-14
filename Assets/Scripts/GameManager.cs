using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.SceneManagement;

public enum GameState
{
    GS_GAME_START,
    GS_GAME_INITIALIZING,
    GS_TRACKING,
    //GS_CLOSE_TO_CLUE,
    //GS_CLOSE_TO_ANIMAL,
    GS_GAME_OVER
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameState CurrentGameState { get; private set; } //AFTER DEBUGGING SET -> PRIVATE SET

    public LocationVariable startLocation;
    public LocationVariable currentLocation;
    public LocationVariable scaleApprox;
    public IntVariable clueCount;

    public GameEvent GenerateWorldEvent;
    //public GameEvent GenerateAnimalEvent;
    public GameEvent GameStartEvent;
    public GameEvent GameOverEvent;

    //public int requiredAmountOfClues;

    //Zmiany dla funkcji IsClueNear()
    //public ClueRuntimeSet ClueMarkerSet;
    public ClueRuntimeSet FoundClues;
    //public Text DebugText;
    public Text MessageBox;
    public Text ClueCountText;

    float timer;
    public float timerLimit;
    public GameObject YouWon;

    //public FloatVariable maxTrackingDistance; //Area of placed clues

    private int cluesFoundCount = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        CurrentGameState = GameState.GS_GAME_START;
        timerLimit = 5f;
    }

    private void Update()
    {
        ClueCountText.text = "Clues found: " + cluesFoundCount;

        //if (CurrentGameState == GameState.GS_CLOSE_TO_CLUE)
        //{
        //    MessageBox.text = "A clue is very close!";
        //}
        //else if (CurrentGameState == GameState.GS_CLOSE_TO_ANIMAL)
        //{
        //    MessageBox.text = "Animal is very close!";
        //}
    }

    public void StartGame()
    {
        CurrentGameState = GameState.GS_GAME_INITIALIZING;

        GameStartEvent.Raise();
        startLocation.SetValue(currentLocation);
        GenerateWorld();
        CurrentGameState = GameState.GS_TRACKING;
        //Game is in tracking state
    }

    public void GenerateWorld()
    {
        ApproximateDistance();

        //create game area
        GenerateWorldEvent.Raise();
    }
    
    private void ApproximateDistance() //approximates distance between 0.001 degrees
    {
        //REFACTOR ME - maybe move somewhere else?
        double latSampleDist = 0.001f;
        double lonSampleDist = 0.000f;

        double earthRadius = 6371000.0f; //meters
        double dLat = ToRadian(latSampleDist);
        double dLng = ToRadian(lonSampleDist);
        double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                   Math.Cos(ToRadian(currentLocation.Lat)) * Math.Cos(ToRadian(currentLocation.Lat + latSampleDist)) *
                   Math.Sin(dLng / 2) * Math.Sin(dLng / 2);
        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        float dist = (float)(earthRadius * c);
        scaleApprox.Lat = dist * 1000f;

        //Debug.LogWarning("Approx distance between 0.001 degrees Lat: " + scaleApprox.Lat);

        latSampleDist = 0.000f;
        lonSampleDist = 0.001f;

        earthRadius = 6371000.0f; //meters
        dLat = ToRadian(latSampleDist);
        dLng = ToRadian(lonSampleDist);
        a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                   Math.Cos(ToRadian(currentLocation.Lat)) * Math.Cos(ToRadian(currentLocation.Lat + latSampleDist)) *
                   Math.Sin(dLng / 2) * Math.Sin(dLng / 2);
        c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        dist = (float)(earthRadius * c);
        scaleApprox.Lon = dist * 1000f;

        //Debug.LogWarning("Approx distance between 0.001 degrees Lon: " + scaleApprox.Lon);
    }

    public void ClueFound(ClueMarker cm)
    {
        FoundClues.Add(cm);
        cm.gameObject.SetActive(false);

        cluesFoundCount++;
        MessageBox.text = "You found " + cluesFoundCount + " clues!";
        //if (cluesFoundCount >= requiredAmountOfClues)
        //{
        //    CurrentGameState = GameState.GS_CLOSE_TO_ANIMAL; //for now it will jump imidiatly to animal close state
        //    GenerateAnimalEvent.Raise();
        //    //MessageBox.text = "Animal is close!";
        //}
        //else
        //{
        //    CurrentGameState = GameState.GS_CLUE_TRACKING;
        //}
        //Debug.LogWarning("Clues found: " + cluesFoundCount);
    }

    public void AnimalFound(AnimalMarker am)
    {
        CurrentGameState = GameState.GS_GAME_OVER;
        GameOverEvent.Raise();
        GameOver();
    }

    private double ToRadian(double degrees)
    {
        return degrees * (Math.PI / (double)180.0f);
    }

    //private void GenerateClues()
    //{
    //    GenerateCluesEvent.Raise();
    //}

    //public void StartClueCapturePhase(ARMarker cm)
    //{
    //    CurrentGameState = GameState.GS_CLOSE_TO_CLUE;
    //    //Spawn clue
    //}

    //public void StartAnimalCapturePhase(ARMarker am)
    //{
    //    CurrentGameState = GameState.GS_CLOSE_TO_ANIMAL;
    //    //Spawn clue
    //}

    public void TimerCheck()
    {
        timer -= 1f;
        if (timer == 0)
        {
            CancelInvoke("TimerCheck");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }

    }

    public void GameOver()
    {
        CurrentGameState = GameState.GS_GAME_OVER;
        YouWon.SetActive(true);
        timer = timerLimit;
        InvokeRepeating("TimerCheck", 1f, 1f);
    }
}