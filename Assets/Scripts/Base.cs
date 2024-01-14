using UnityEngine;
using UnityEngine.Events;

public class Base : MonoBehaviour
{
    [SerializeField] private Bot[] _bots;
    [SerializeField] private Scanner _scanner;

    public event UnityAction BoxAdded;

    private void Start()
    {
        foreach (Bot bot in _bots)
        {
            bot.ResetEmployment();
        }
    }

    private void OnEnable()
    {
        foreach (Bot bot in _bots)
        {
            bot.BotIsBack += bot.ResetEmployment;
        }
    }

    private void OnDisable()
    {
        foreach (Bot bot in _bots)
        {
            bot.BotIsBack -= bot.ResetEmployment;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Box box))
        {
            BoxAdded?.Invoke();
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            for (int i = 0; i < _bots.Length; i++)
            {
            Debug.Log("Box is find");
                if (_bots[i].IsBusy == false && _scanner.Target.IsTaken == false)
                {
                    Debug.Log(_bots[i].name);
                    Debug.Log(_bots[i].IsBusy);
                    _bots[i].SetTarget(_scanner.Target);
                    _bots[i].SetEmployment();
                    _scanner.Target.ChangeState();
                    _bots[i].Mover.Go();
                }
            }
        }
    }
}
