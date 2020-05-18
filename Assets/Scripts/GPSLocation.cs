using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;
using System.Runtime.InteropServices.WindowsRuntime;

public class GPSLocation : MonoBehaviour
{
    public float startLongitude;
    public float startLatitude;
    GameObject dialog = null;

    public Text textLonLat;
    // Start is called before the first frame update
    void Start()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
            dialog = new GameObject();
        }
    }

    void OnGUI()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            // The user denied permission to use the location.
            // Display a message explaining why you need it with Yes/No buttons.
            // If the user says yes then present the request again
            // Display a dialog here.
            dialog.AddComponent<PermissionsRationaleDialog>();
            return;
        }
        else if (dialog != null)
        {
            Destroy(dialog); 
        }

        StartCoroutine(StartLocationService());
    }

    private IEnumerator StartLocationService()
    {
        Permission.RequestUserPermission(Permission.FineLocation);

        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("User didn't permit geolocation");
            textLonLat.text = "User didn't permit geolocation";
            yield break;
        }

        Input.location.Start();
        float _maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && _maxWait > 0)
        {
            yield return new WaitForSeconds(1.0f);
            _maxWait--;
        }
        
        if (_maxWait <= 0)
        {
            Debug.Log("Location service initiation timeout");
            textLonLat.text = "Location service initiation timeout";
            yield break;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.LogWarning("Location service status: Failed");
            textLonLat.text = "Location service status: Failed";
        }

        startLongitude = Input.location.lastData.longitude;
        startLatitude = Input.location.lastData.latitude;

        textLonLat.text = "Lon: " + startLongitude + "\n" + "Lat: " + startLatitude;
        yield break;
    }

    private bool UpdateLocation()
    {
        bool _retVal = false;

        return _retVal;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
