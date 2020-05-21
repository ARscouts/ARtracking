using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class ClueSpawner : MonoBehaviour
{
    private ARSessionOrigin arOrigin;
    public Camera Camera;
    public LocationVariable currentLocation;
    public LocationVariable scaleApprox;
    public FloatVariable maxTrackingDistance;
    public IntVariable clueCount;
    public ClueMarker ClueMarkerPrefab;
    public AnimalMarker AnimalMarkerPrefab;

    public GameObject FoxCluePrefab;
    public GameObject FoxPrefab;
    //public ClueModel clueModelPrefab;
    // Start is called before the first frame update

    public Text MessageBox;

    void Start()
    {
        arOrigin = FindObjectOfType<ARSessionOrigin>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateClueObjects()
    {
        for (int i = 1; i <= clueCount.Value; i++)
        {
            RandomInSquare(maxTrackingDistance.Value, out float LatInMeters, out float LonInMeters);
            ClueMarker cm = Instantiate(ClueMarkerPrefab);
            cm.SetLonLat(LonInMeters / scaleApprox.Lon + currentLocation.Lon, 
                LatInMeters / scaleApprox.Lon + currentLocation.Lat,
                LonInMeters,
                LatInMeters);
            //--------Name and tag of created clues
            cm.name = "ClueObject" + i;

            //--------Debug Text (comment if no need)

            //DebugText.gameObject.SetActive(true);
            //DebugText.text += "\nClueObject" + i + "\nLat: " + cm.Lon + "\nLon: " + cm.Lat;


            //Debug.LogWarning("Added new clue locations - Lat: " + cm.Lat + " Lon: " + cm.Lon);
        }
    }

    private void RandomInSquare(float maxTrackingDistance, out float LatInMeters, out float LonInMeters) //generates random coordinates in square area
    {
        LatInMeters = Random.Range(-maxTrackingDistance, maxTrackingDistance);
        LonInMeters = Random.Range(-maxTrackingDistance, maxTrackingDistance);
    }

    public void SpawnClueInARScene(ClueMarker cm) //FOR NOW SPAWNS CLUE AHEAD OF THE PLAYER 
    {
        Ray ray = Camera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        Vector3 d = ray.direction;
        d.y = -0.5f;
        ray.direction = d;
        
        cm.transform.SetPositionAndRotation(Camera.transform.position, Quaternion.Euler(0, 0, 0));
        cm.transform.Translate(ray.direction * 4, Space.World);

        Instantiate(FoxCluePrefab, cm.transform);
        MessageBox.text = "A clue is very near!";
    }

    public void SpawnAnimal()
    {
        AnimalMarker am = Instantiate(AnimalMarkerPrefab);

        Ray ray = Camera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        Vector3 d = ray.direction;
        d.y = -0.5f;
        ray.direction = d;

        am.transform.SetPositionAndRotation(Camera.transform.position, Quaternion.Euler(0, 0, 0));
        am.transform.Translate(ray.direction * 4, Space.World);

        Instantiate(FoxPrefab, am.transform);
        MessageBox.text = "Animal is very near!";
    }
}
