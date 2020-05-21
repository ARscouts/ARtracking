using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateObject : MonoBehaviour
{
    public Transform Player;
    public Transform Object1;
    Vector3 center;

    // Start is called before the first frame update
    void Start()
    {
        center.x = Player.position.x;
        center.y = Player.position.y;
        center.z = Player.position.z;

        Object1.position = new Vector3(center.x+1, center.y-2, center.z);
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }
}
