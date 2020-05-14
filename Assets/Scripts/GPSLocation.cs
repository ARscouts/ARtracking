using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;

public class GPSLocation : MonoBehaviour
{
    private float longitude;
    private float latitude;
    GameObject dialog = null;

    public Text textLonLat;
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
        else if (dialog != null)
        {
            Destroy(dialog); 
        }
#endif

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
        float maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1.0f);
            maxWait--;
        }
        
        if (maxWait <= 0)
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

        longitude = Input.location.lastData.longitude;
        latitude = Input.location.lastData.latitude;

        textLonLat.text = "Longitude: " + longitude + "\n" + "Latitude: " + latitude;
        yield break;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
