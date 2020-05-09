using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// View class for MVP implementation of game details UI in Minesweeper.
/// </summary>
public class GameDetailsView : MonoBehaviour
{
    [SerializeField]
    private Text _timerText;
    [SerializeField]
    private Text _resultText;
    [SerializeField]
    private GameObject _finalPopup;

    public Action<GameDifficulty> _DifficultyChosen;


    public void DisplayTimer (int timer )
    {
        _timerText.text = timer.ToString ( );
    }
}
