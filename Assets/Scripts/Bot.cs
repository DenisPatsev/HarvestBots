using UnityEngine;
using UnityEngine.Events;

public class Bot : MonoBehaviour
{
    [SerializeField] private BotMover _mover;

    private bool _isBusy;
    private Box _target;

    public bool IsBusy => _isBusy;
    public Box Target => _target;

    public BotMover Mover => _mover;

    public event UnityAction BotIsBack;

    private void OnEnable()
    {
        _mover.BoxIsBrought += ReportReturning;
    }

    private void OnDisable()
    {
        _mover.BoxIsBrought -= ReportReturning;
    }

    private void ReportReturning()
    {
        BotIsBack?.Invoke();
    }

    public void SetEmployment()
    {
        _isBusy = true;
    }

    public void ResetEmployment()
    {
        _isBusy = false;
    }

    public void SetTarget(Box target)
    {
        _target = target;
    }
}
