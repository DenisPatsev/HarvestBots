using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBuilder : MonoBehaviour
{
    [SerializeField] private Base _oldBase;
    [SerializeField] private BaseFlag _newBaseFlagPrefab;
    [SerializeField] private Base _basePrefab;
    [SerializeField] private Border _frontBorder;
    [SerializeField] private Border _rightBorder;
    [SerializeField] private Border _leftBorder;
    [SerializeField] private Border _backBorder;

    private Bot _builderBot;
    private BaseFlag _newBaseFlag;
    private BaseFlag _newBase;
    private float _frontBorderPositionZ;
    private float _rightBorderPositionX;
    private float _leftBorderPositionX;
    private float _backBorderPositionZ;

    private void OnEnable()
    {
        _oldBase.NewBaseAdded += CreateNewBasePoint;
        _oldBase.MovementActivated += MoveBase;
    }

    private void OnDisable()
    {
        _oldBase.NewBaseAdded -= CreateNewBasePoint;
        _oldBase.MovementActivated -= MoveBase;
    }

    private void Start()
    {
        _frontBorderPositionZ = _frontBorder.transform.position.z;
        _backBorderPositionZ = _backBorder.transform.position.z;
        _rightBorderPositionX = _rightBorder.transform.position.x;
        _leftBorderPositionX = _leftBorder.transform.position.x;
    }

    private void CreateNewBasePoint()
    {
        var newBase = Instantiate(_newBaseFlagPrefab);
        _newBase = newBase;

        StartCoroutine(MoveNewBase(newBase));
    }

    private IEnumerator MoveNewBase(BaseFlag newBase)
    {
        while (!Input.GetKey(KeyCode.Return))
        {
            if (Input.GetKey(KeyCode.Mouse1))
            {
                Destroy(newBase.gameObject);
                StopCoroutine(MoveNewBase(newBase));
                newBase = null;
            }

            var coursorPosition = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(coursorPosition, out hit))
            {
                if (hit.point.z < _backBorderPositionZ && hit.point.z > _frontBorderPositionZ &&
                    hit.point.x > _rightBorderPositionX && hit.point.x < _leftBorderPositionX)
                {
                    newBase.transform.position = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                }
            }

            yield return new WaitForEndOfFrame();
        }

        _newBaseFlag = newBase;
        StartCoroutine(CreateNewBase());
    }

    private IEnumerator CreateNewBase()
    {
        while (_oldBase.BoxCount < _oldBase.BaseCost)
        {
            yield return new WaitForEndOfFrame();
        }

        Base newBase = Instantiate(_basePrefab);
        Destroy(_newBaseFlag.gameObject);

        newBase.transform.position = _newBaseFlag.transform.position;
        _builderBot = _oldBase.Bots[0];
        _oldBase.SpendResources(_oldBase.BaseCost);

        while (_oldBase.Bots[0].IsBusy != false)
        {
            yield return new WaitForEndOfFrame();
        }

        _oldBase.Bots[0].GetComponent<BotMover>().MoveToNewBase(newBase.GetComponentInChildren<BotSpawnPoint>().transform);
        _oldBase.RemoveBot(_oldBase.Bots[0]);
        newBase.AddBot(_builderBot);
        _oldBase.ResetNewBaseCreationState();
    }

    private void MoveBase()
    {
        StartCoroutine(MoveNewBase(_newBase));
    }
}
