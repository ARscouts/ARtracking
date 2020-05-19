using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    GS_GAME_START,
    GS_GAME_INITIALIZING,
    GS_CLUE_TRACKING,
    GS_ANIMAL_TRACKING,
    GS_GAME_OVER
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public LocationVariable startLocation;
    public LocationVariable currentLocation;
    public LocationVariable scaleApprox; //approximated distance between 0.001 degrees in meters
    public IntVariable clueCount;

    public GameEvent GenerateCluesEvent;
    public GameEvent GenerateAnimalEvent;
    public GameEvent GameOverEvent;

    public int requiredAmountOfClues;

    //public FloatVariable maxTrackingDistance; //Area of placed clues

    private int cluesFoundCount = 0;
    private GameState currentGameState; //current state of the game maybe will be useful

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
        currentGameState = GameState.GS_GAME_START;
    }

    private void Update()
    {

    }

    public void StartGame()
    {
        currentGameState = GameState.GS_GAME_INITIALIZING;
        GenerateWorld();
        currentGameState = GameState.GS_CLUE_TRACKING;
        //Game is in tracking state
    }

    public void GenerateWorld()
    {
        ApproximateDistance();

        //create game area
        GenerateClues();
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

    public void ClueFound()
    {
        cluesFoundCount++;
        if (cluesFoundCount >= requiredAmountOfClues)
        {
            currentGameState = GameState.GS_ANIMAL_TRACKING;
            GenerateAnimalEvent.Raise();
        }
        Debug.LogWarning("Clues found: " + cluesFoundCount);
    }

    public void AnimalFound()
    {
        currentGameState = GameState.GS_GAME_OVER;
        GameOverEvent.Raise();
    }

    private double ToRadian(double degrees)
    {
        return degrees * (Math.PI / (double)180.0f);
    }

    private void GenerateClues()
    {
        GenerateCluesEvent.Raise();
    }
}