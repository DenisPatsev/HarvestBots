using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BoxScore : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Base _base;

    private float _score;
    private string _info;

    private void Start()
    {
        _score = 0;
        _info = _text.text;
    }

    private void OnEnable()
    {
        _base.AddScore += AddScore;
    }

    private void OnDisable()
    {
        _base.AddScore -= AddScore;
    }

    private void AddScore()
    {
        _score++;
        _text.text = _info + " " + _score.ToString();
    }
}
