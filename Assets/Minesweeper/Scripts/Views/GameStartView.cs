using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// View class for MVP implementation of game details UI in Minesweeper.
/// Displays Start popup.
/// Controlled by GameDetailsController.
/// </summary>
public class GameStartView : GamePopupBaseView
{
    [SerializeField]
    private Dropdown _difficultyDropdown;
    [SerializeField]
    private Text _totalMinesText;

    public Action<GameDifficulty> _gameStarted;

    public void StartGame ( )
    {
        _gameStarted.Invoke ( ( GameDifficulty ) _difficultyDropdown.value );
    }
}
