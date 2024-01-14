using UnityEngine;

public class Box : MonoBehaviour
{
    private bool _isTaken;

    public bool IsTaken => _isTaken;

    private void Start()
    {
        _isTaken = false;
    }

    public void ChangeState()
    {
        _isTaken = true;
    }
}
