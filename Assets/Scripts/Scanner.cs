using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    [SerializeField] private float _speed;

    private Box _target;

    public Box Target => _target;

    private void Update()
    {
        transform.Rotate(new Vector3(0, _speed * Time.deltaTime, 0));

        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            if (hit.collider.gameObject.TryGetComponent(out Box box))
            {
                if (box.IsTaken == false)
                {
                    _target = box;
                }
            }
        }
    }
}
