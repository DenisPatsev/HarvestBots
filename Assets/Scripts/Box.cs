using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    private bool _isTaked;

    public bool IsTaken => _isTaked;

    private void Start()
    {
        _isTaked = false;
    }

    public void BeTaken()
    {
        _isTaked = true;
    }
}
