using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public class BotMover : MonoBehaviour
{
    private const string Walk = "Walking";

    [SerializeField] private float _speed;
    [SerializeField] private Raycaster _raycaster;
    [SerializeField] private float _dropForce;
    [SerializeField] private Scanner _scanner;
    [SerializeField] private Boxplace _boxplace;

    private Box _box;
    private Vector3 _startPosition;
    private Animator _animator;
    private bool _isWalking;
    private bool _isBusy;
    private bool _isFound;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _startPosition = transform.position;
        _isBusy = false;
        _isFound = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (_isBusy == false && _scanner.Target.IsTaken == false)
            {
                _box = _scanner.Target;
                _box.BeTaken();
                _isBusy = true;

                Stop();
                StartCoroutine(MoveToTarget());
            }
        }

        CheckBoxForward();
    }

    private IEnumerator MoveToTarget()
    {
        _animator.SetBool(Walk, true);
        transform.rotation = Quaternion.LookRotation(transform.position - _box.transform.position);
        _isWalking = true;

        while (_isWalking == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, _box.transform.position, _speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator MoveBack()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - _startPosition);

        while (transform.position != _startPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, _startPosition, _speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        _animator.SetBool(Walk, false);

        DropBox();
        _isBusy = false;
    }

    private void TakeBox()
    {
        _box.transform.SetParent(transform, worldPositionStays: true);
        _box.transform.position = _boxplace.transform.position;
        _box.transform.rotation = transform.rotation;
        _box.gameObject.GetComponent<Rigidbody>().isKinematic = true;
    }

    private void DropBox()
    {
        Rigidbody rigidbody = _box.GetComponent<Rigidbody>();

        _box.transform.parent = null;
        rigidbody.isKinematic = false;
        rigidbody.AddForce(_box.transform.forward * _dropForce * -1, ForceMode.Force);
        _isFound = false;
    }

    private void CheckBoxForward()
    {
        RaycastHit hit;
        float distance = 3;

        if (Physics.Raycast(_raycaster.transform.position, transform.forward * -1, out hit, distance))
        {
            if (hit.collider.gameObject.TryGetComponent(out Box box) && _isFound == false)
            {
                _isFound = true;
                Stop();
                TakeBox();
                StartCoroutine(MoveBack());
            }
        }
    }

    private void Stop()
    {
        _isWalking = false;
    }
}
