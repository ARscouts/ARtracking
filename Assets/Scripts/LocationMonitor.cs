using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocationMonitor : MonoBehaviour //prints current location in LonLat element
{
    public LocationVariable CurrentLocation;
    public Text Text;

    private LocationVariable PreviousLocation;
    // Start is called before the first frame update
    void Start()
    {
        PreviousLocation = ScriptableObject.CreateInstance<LocationVariable>();
    }

    // Update is called once per frame
    void Update()
    {
        //Text.text = "Lon: " + PreviousLocation.Lon + "\n" + "Lat: " + PreviousLocation.Lat;
        if (!CurrentLocation.Equals(PreviousLocation))
        {
            PreviousLocation.SetValue(CurrentLocation);
            UpdateText();
        }
    }

    private void UpdateText()
    {
        Text.text = "Lon: " + PreviousLocation.Lon + "\n" + "Lat: " + PreviousLocation.Lat;
    }
}
