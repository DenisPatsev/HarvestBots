using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]

public class BotMover : MonoBehaviour
{
    private const string Walk = "Walking";

    [SerializeField] private float _speed;
    [SerializeField] private float _dropForce;
    [SerializeField] private Raycaster _raycaster;
    [SerializeField] private Boxplace _boxplace;
    [SerializeField] private Base _base;
    [SerializeField] private Bot _bot;

    private Box _target;
    private Vector3 _startPosition;
    private Animator _animator;
    private bool _isWalking;
    private bool _isBoxFound;

    public event UnityAction BoxIsBrought;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _startPosition = transform.position;
        _isBoxFound = false;
    }

    private void Update()
    {
        CheckBoxForward();
    }

    private IEnumerator MoveToTarget()
    {
        Debug.Log("Start");
        _animator.SetBool(Walk, true);
        transform.rotation = Quaternion.LookRotation(transform.position - _target.transform.position);
        _isWalking = true;

        while (_isWalking == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, _speed * Time.deltaTime);
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
        BoxIsBrought?.Invoke();
    }

    private void TakeBox()
    {
        _target.transform.SetParent(transform, worldPositionStays: true);
        _target.transform.position = _boxplace.transform.position;
        _target.transform.rotation = transform.rotation;
        _target.gameObject.GetComponent<Rigidbody>().isKinematic = true;
    }

    private void DropBox()
    {
        Rigidbody rigidbody = _target.GetComponent<Rigidbody>();

        _target.transform.parent = null;
        rigidbody.isKinematic = false;
        rigidbody.AddForce(_target.transform.forward * _dropForce * -1, ForceMode.Force);
        _isBoxFound = false;
    }

    private void CheckBoxForward()
    {
        RaycastHit hit;
        float distance = 3;

        if (Physics.Raycast(_raycaster.transform.position, transform.forward * -1, out hit, distance))
        {
            if (hit.collider.gameObject.TryGetComponent(out Box box) && _isBoxFound == false)
            {
                _isBoxFound = true;
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

    public void Go()
    {
        _target = _bot.Target;
        Stop();
        StartCoroutine(MoveToTarget());
    }
}
