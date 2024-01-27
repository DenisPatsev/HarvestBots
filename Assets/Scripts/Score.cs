using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private TMP_Text _bots;
    [SerializeField] private Base _base;

    private string _boxInfo;
    private string _botInfo;

    private void Start()
    {
        _boxInfo = _text.text;
        _botInfo = _bots.text;

        _text.text += " " + _base.BoxCount.ToString();
        _bots.text += " " + _base.BotCount.ToString();
    }

    private void OnEnable()
    {
        _base.BoxAdded += RefreshScore;
        _base.BotAdded += RefreshBotInfo;
    }

    private void OnDisable()
    {
        _base.BoxAdded -= RefreshScore;
        _base.BotAdded -= RefreshBotInfo;
    }

    private void RefreshScore()
    {
        _text.text = _boxInfo + " " + _base.BoxCount.ToString();
    }

    private void RefreshBotInfo()
    {
        _bots.text = _botInfo + " " + _base.BotCount.ToString();
    }
}
