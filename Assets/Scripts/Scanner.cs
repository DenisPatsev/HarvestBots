using UnityEngine;

public class Scanner : MonoBehaviour
{
    [SerializeField] private float _speed;

    private Box _target;

    public Box Target => _target;

    private void Update()
    {
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

        transform.Rotate(new Vector3(0, _speed * Time.deltaTime, 0));
    }
}
