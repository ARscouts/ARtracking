using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OriginRotator : MonoBehaviour
{
    public FloatVariable CurrentCompass;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RotateOrigin()
    {
        gameObject.transform.eulerAngles = new Vector3 (
                gameObject.transform.eulerAngles.x,
                CurrentCompass.Value, 
                gameObject.transform.eulerAngles.z
            );
    }
}
