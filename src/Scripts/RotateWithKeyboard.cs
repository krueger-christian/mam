using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWithKeyboard : MonoBehaviour
{

    public float speed = 0.1f;


    void Update()
    {
        if(Input.GetKey(KeyCode.RightArrow))
            transform.Rotate(new Vector3(0, speed, 0));
        else if(Input.GetKey(KeyCode.LeftArrow))
            transform.Rotate(new Vector3(0, -speed, 0));
            
        if(Input.GetKey(KeyCode.UpArrow))
            transform.Rotate(new Vector3(-speed, 0, 0));
        else if(Input.GetKey(KeyCode.DownArrow))    
            transform.Rotate(new Vector3(speed, 0, 0));
        
    }
}
