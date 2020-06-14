using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class ClueSpawner : MonoBehaviour
{
    //private ARSessionOrigin arOrigin;

    public Camera Camera;

    public LocationVariable startingLocation;
    public LocationVariable currentLocation;
    public LocationVariable scaleApprox;

    public FloatVariable maxTrackingDistance;
    public FloatVariable minTrackingDistance;
    public FloatVariable groundDistance;

    public IntVariable clueCount;

    public ClueMarker ClueMarkerPrefab;
    public AnimalMarker AnimalMarkerPrefab;

    public ARMarkerRuntimeSet ActiveMarkers;
    public ARMarkerRuntimeSet HiddenMarkers;
    public AnimalRuntimeSet ActiveAnimalMarkers;
    public GameObjectSet Animals;

    //public GameObject FoxCluePrefab;
    //public GameObject FoxPrefab;

    public Text MessageBox;

    // Start is called before the first frame update
    void Start()
    {
        //arOrigin = FindObjectOfType<ARSessionOrigin>();
    }

    public void GenerateTrackingObjects()
    {
        GenerateAnimal();
        GenerateClues();
    }

    private void GenerateAnimal() //creates one instance of given animal marker prefab
    {
        RandomInSquare(minTrackingDistance.Value, maxTrackingDistance.Value, out float LatInMeters, out float LonInMeters);
        AnimalMarker am = Instantiate(AnimalMarkerPrefab);

        MetersToGeographic(LonInMeters, LatInMeters, out float lonGeo, out float latGeo);

        am.SetLonLat(
            lonGeo,
            latGeo,
            LonInMeters,
            LatInMeters
         );

        //set to random animal
        am.MeshPrefab = Animals.GetRandomItem();

        //testing position
        //am.transform.position = new Vector3(1.0f, 0.0f, 1.0f);
    }

    private void AttachClueToRandomAnimal(ClueMarker cm)
    {
        //get random animalmarker clue belongs to
        cm.BelongsTo = ActiveAnimalMarkers.GetRandomItem();

        //set rotation based on markers geo location
        Vector3 AnimalGeoLocation = new Vector3(cm.BelongsTo.Lon, 0.0f, cm.BelongsTo.Lat);
        Vector3 ClueGeoLocation = new Vector3(cm.Lon, 0.0f, cm.Lat);
        
        cm.transform.rotation = Quaternion.LookRotation(AnimalGeoLocation - ClueGeoLocation);

        //get random clue from attached animal
        Animal animal = (Animal)cm.BelongsTo.MeshPrefab.GetComponent(typeof(Animal));
        if (animal)
        {
            cm.MeshPrefab = animal.Clues.GetRandomItem();
        } else
        {
            Debug.LogError("Couldn't find Animal MonoBehaviour component on " + cm.BelongsTo.name);
        }
    }

    private void GenerateClues() //generates clueCount amount of instances of ClueMarkerPrefab
    {
        for (int i = 1; i <= clueCount.Value; i++)
        {
            RandomInSquare(0, maxTrackingDistance.Value, out float LatInMeters, out float LonInMeters);
            ClueMarker cm = Instantiate(ClueMarkerPrefab);

            MetersToGeographic(LonInMeters, LatInMeters, out float lonGeo, out float latGeo);
            cm.SetLonLat(
                lonGeo,
                latGeo,
                LonInMeters,
                LatInMeters
            );
            //--------Name and tag of created clues
            cm.name = "ClueObject" + i;

            AttachClueToRandomAnimal(cm);
        }
    }

    private void RandomInSquare(float minDistance, float maxDistance, out float LatInMeters, out float LonInMeters) //generates random coordinates in square area
    {
        LatInMeters = UnityEngine.Random.Range(-(maxDistance - minDistance), maxDistance - minDistance);
        LonInMeters = UnityEngine.Random.Range(-(maxDistance - minDistance), maxDistance - minDistance);

        LatInMeters = LatInMeters >= 0 ? LatInMeters + minDistance : LatInMeters - minDistance;
        LonInMeters = LonInMeters >= 0 ? LonInMeters + minDistance : LonInMeters - minDistance;
    }

    public void SpawnMarkerInARScene(ARMarker am)
    {
        GeolocationSpawn(am);

        if (!am.isSpawned)
        {
            Instantiate(am.MeshPrefab, am.transform);
        } else
        {
            am.transform.GetChild(0).gameObject.SetActive(true);
        }

        HiddenMarkers.Remove(am);
        ActiveMarkers.Add(am);
        am.isSpawned = true;
        

        //TODO set height

        ////spawns clue ahead of you
        //Ray ray = Camera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        //Vector3 d = ray.direction;
        //d.y = 0.0f;
        //ray.direction = d;

        ////TODO fix
        //am.transform.SetPositionAndRotation(Camera.transform.position, Quaternion.Euler(0, 0, 0));
        //am.transform.Translate(ray.direction * 3, Space.World);
        //am.transform.Translate(Vector3.down * 1.5f, Space.World);

        //MessageBox.text = "A clue is very near!";
    }

    public void HideMarkerInTheScene(ARMarker am)
    {
        if (am.isSpawned)
        {
            am.transform.GetChild(0).gameObject.SetActive(false);

            ActiveMarkers.Remove(am);
            HiddenMarkers.Add(am);
        }
    }

    private void GeolocationSpawn(ARMarker am)
    {
        // get current postion based on geolocation and change it to meters
        GeographicToMeters(currentLocation, out float lonMeters, out float latMeters);

        Vector3 animalVector = new Vector3(
            am.LonInMeters - lonMeters,
            groundDistance.Value, //hard coded "ground" level
            am.LatInMeters - latMeters
        );
        //Debug.LogWarning(groundDistance.Value);

        am.transform.position = Camera.transform.position;
        am.transform.position += animalVector;
    }

    //public void SpawnAnimal()
    //{
    //    AnimalMarker am = Instantiate(AnimalMarkerPrefab);

    //    Ray ray = Camera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
    //    Vector3 d = ray.direction;
    //    d.y = 0.0f;
    //    ray.direction = d;

    //    am.transform.SetPositionAndRotation(Camera.transform.position, Quaternion.Euler(0, 0, 0));
    //    am.transform.Translate(ray.direction * 3, Space.World);
    //    am.transform.Translate(Vector3.down * 1.5f, Space.World);

    //    Instantiate(FoxPrefab, am.transform);
    //    MessageBox.text = "Animal is very near!";
    //}

    private void GeographicToMeters(LocationVariable location, out float lonMeters, out float latMeters)
    {
        lonMeters = (location.Lon - startingLocation.Lon) * scaleApprox.Lon;
        latMeters = (location.Lat - startingLocation.Lat) * scaleApprox.Lat;
    }

    private void GeographicToMeters(float lon, float lat, out float lonMeters, out float latMeters)
    {
        lonMeters = (lon - startingLocation.Lon) * scaleApprox.Lon;
        latMeters = (lat - startingLocation.Lat) * scaleApprox.Lat;
    }

    private void MetersToGeographic(LocationVariable location, out float lonGeo, out float latGeo)
    {
        lonGeo = location.Lon / scaleApprox.Lon - startingLocation.Lon;
        latGeo = location.Lat / scaleApprox.Lat - startingLocation.Lat;
    }

    private void MetersToGeographic(float lon, float lat, out float lonGeo, out float latGeo)
    {
        lonGeo = lon / scaleApprox.Lon - startingLocation.Lon;
        latGeo = lat / scaleApprox.Lat - startingLocation.Lat;
    }
}
