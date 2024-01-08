using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    [SerializeField] private float _speed;

    private void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward.normalized * _speed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.S)) 
        {
            transform.Translate(Vector3.back.normalized * _speed * Time.deltaTime);
        }

        if (Input.GetKey (KeyCode.D))
        {
            transform.Translate(Vector3.right.normalized * _speed * Time.deltaTime);
        }

        if(Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left.normalized * _speed * Time.deltaTime);
        }

        if( Input.GetKey(KeyCode.RightArrow))
        {
            transform.localEulerAngles += new Vector3(0, _speed * Time.deltaTime, 0);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.localEulerAngles += new Vector3(0, -_speed * Time.deltaTime, 0);
        }
    }
}
