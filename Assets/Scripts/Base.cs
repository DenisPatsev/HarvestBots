using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Base : MonoBehaviour
{
    [SerializeField] private List<Bot> _bots;
    [SerializeField] private Scanner _scanner;
    [SerializeField] private BotSpawnPoint _spawnPoint;
    [SerializeField] private Bot _prefab;

    private float _newBotOffsetX;

    private int _newBotCost;
    private int _newBaseCost;
    private int _boxCount;
    private int _botCount;

    private bool _isBuilded;

    public event UnityAction BoxAdded;
    public event UnityAction BotAdded;
    public event UnityAction NewBaseAdded;
    public event UnityAction MovementActivated;

    public int BoxCount => _boxCount;
    public int BotCount => _botCount;
    public int BaseCost => _newBaseCost;
    public int BotCost => _newBotCost;

    public List<Bot> Bots => _bots;

    private void Start()
    {
        foreach (Bot bot in _bots)
        {
            bot.ResetEmployment();
        }

        _newBotCost = 3;
        _newBaseCost = 5;
        _boxCount = 0;
        _botCount = _bots.Count;
        _isBuilded = false;
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
            _boxCount++;
            BoxAdded?.Invoke();
            Destroy(box.gameObject);
        }
    }

    private void OnMouseDown()
    {
        if (_isBuilded == false)
        {
            NewBaseAdded?.Invoke();
            _isBuilded = true;
        }
        else
        {
            MovementActivated?.Invoke();
        }
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            foreach (var bot in _bots)
            {
                if (bot.IsBusy == false && _scanner.Target.IsTaken == false)
                {
                    bot.SetTarget(_scanner.Target);
                    bot.SetEmployment();
                    _scanner.Target.ChangeState();
                    bot.Mover.Go();
                }
            }
        }
    }

    public void TryCreateNewBot()
    {
        if (_boxCount >= _newBotCost)
        {
            float maxOffset = 10f;

            _newBotOffsetX = Random.Range(0, maxOffset);
            Vector3 offset = new Vector3(_newBotOffsetX, 0, _newBotOffsetX);
            Bot bot = Instantiate(_prefab);

            bot.transform.rotation = _bots[0].transform.rotation;
            bot.transform.position = _spawnPoint.transform.position + offset;
            bot.ResetEmployment();

            _bots.Add(bot);
            _boxCount -= _newBotCost;
            _botCount++;
            BoxAdded?.Invoke();
            BotAdded?.Invoke();
        }
        else
        {
            return;
        }
    }

    public void AddBot(Bot bot)
    {
        _bots.Add(bot);
    }

    public void RemoveBot(Bot bot)
    {
        _bots.Remove(bot);
    }

    public void SpendResources(int resourcesCount)
    {
        _boxCount -= resourcesCount;
    }

    public void ResetNewBaseCreationState()
    {
        _isBuilded = false;
    }
}
