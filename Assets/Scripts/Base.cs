using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Base : MonoBehaviour
{
    public UnityAction AddScore;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Box box) && box.IsTaken == true)
        {
            AddScore.Invoke();
        }
    }
}
