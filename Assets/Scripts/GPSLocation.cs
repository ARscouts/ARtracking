using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;
using System.Runtime.InteropServices.WindowsRuntime;

public class GPSLocation : MonoBehaviour
{
    //public float startLongitude;
    //public float startLatitude;
    //public float currentLongitude;
    //public float currentLatitude;
    public LocationVariable startLocation;
    public LocationVariable currentLocation;

    GameObject dialog = null;

    private bool locationServiceStarted = false;
    // Start is called before the first frame update
    void Start()
    {
#if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
            dialog = new GameObject();
        }
#endif
    }

    void OnGUI()
    {
#if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            // The user denied permission to use the location.
            // Display a message explaining why you need it with Yes/No buttons.
            // If the user says yes then present the request again
            // Display a dialog here.
            dialog.AddComponent<PermissionsRationaleDialog>();
            return;
        }
        else
        {
            if (dialog != null)
            {
                Destroy(dialog);
            }
            StartCoroutine(StartLocationService());
        }
#endif
    }

    private IEnumerator StartLocationService()
    {
        Permission.RequestUserPermission(Permission.FineLocation);

        if (!Input.location.isEnabledByUser)
        {
            //Debug.LogWarning("User didn't permit geolocation");
            //textLonLat.text = "User didn't permit geolocation";
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
            Debug.LogWarning("Location service initiation timeout");
            //textLonLat.text = "Location service initiation timeout";
            yield break;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.LogWarning("Location service status: Failed");
            //textLonLat.text = "Location service status: Failed";
        }

        locationServiceStarted = true;

        startLocation.Lon = Input.location.lastData.longitude;
        startLocation.Lat = Input.location.lastData.latitude;

        currentLocation.SetValue(startLocation);

        yield break;
    }

    private void UpdateLocation()
    {
        if (Input.location.status == LocationServiceStatus.Initializing || Input.location.status == LocationServiceStatus.Running)
        {
            currentLocation.Lon = Input.location.lastData.longitude;
            currentLocation.Lat = Input.location.lastData.latitude;
        } else 
        {
            Debug.LogWarning("Can't get current location: " + Input.location.status);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (locationServiceStarted)
        {
            UpdateLocation();
        }
    }
}
