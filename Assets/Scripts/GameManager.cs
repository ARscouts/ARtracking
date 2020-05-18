using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager: MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public LocationVariable startLocation;
    public LocationVariable currentLocation;
    public float maxTrackingDistance; //Area of placed clues
    public ClueRuntimeSet clueSet;

    //approximated distance between 0.001 degrees in meters
    private float lonApprox;
    private float latApprox;
    private readonly List<LocationVariable> clueLocations = new List<LocationVariable>();

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
        
    }

    private void Update()
    {

    }

    public void GenerateWorld()
    {
        ApproximateDistance();

        //create game area
        GenerateClues(10);

        //set tracked animal in lon and lat

        //set all the clues
    }

    private void ApproximateDistance() //approximates distance between 0.001 degrees
    {
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
        latApprox = dist * 1000f;

        Debug.LogWarning("Approx distance between 0.001 degrees Lat: " + latApprox);

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
        lonApprox = dist * 1000f;

        Debug.LogWarning("Approx distance between 0.001 degrees Lon: " + lonApprox);
    }

    private double ToRadian(double degrees)
    {
        return degrees * (Math.PI / (double)180.0f);
    }

    private void GenerateClues(int cluesAmount)
    {
        //For now it generates clues randomly in a square area with sides perperticulat to world directions
        for (int i = 0; i < cluesAmount; i++)
        {
            RandomInSquare(maxTrackingDistance, out float Lat, out float Lon);
            LocationVariable lv = ScriptableObject.CreateInstance<LocationVariable>();
            lv.SetValue(Lon, Lat);

            Debug.LogWarning("Added new clue locations - Lat: " + lv.Lat + " Lon: " + lv.Lon);
            clueLocations.Add(lv);
        }
    }

    private void RandomInSquare(float maxTrackingDistance, out float lat, out float lon)
    {
        lat = UnityEngine.Random.Range(-maxTrackingDistance, maxTrackingDistance) / latApprox + currentLocation.Lat;
        lon = UnityEngine.Random.Range(-maxTrackingDistance, maxTrackingDistance) / lonApprox + currentLocation.Lon;
    }
}
