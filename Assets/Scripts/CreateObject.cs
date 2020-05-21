using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class CreateObject : MonoBehaviour
{
    public Transform Player;
    public Transform Object1;
    public Transform ARplane_my;
    Vector3 center;
    Vector3 ar_plane;

    // Start is called before the first frame update
    void Start()
    {
        center.x = Player.position.x;
        center.y = Player.position.y;
        center.z = Player.position.z;
        ar_plane.y = ARplane_my.position.y;

        Object1.position = new Vector3(center.x+1, ar_plane.y, center.z);
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }
}
