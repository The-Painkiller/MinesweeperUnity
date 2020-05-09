using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

/// <summary>
/// View class for MVP implementation of game details UI in Minesweeper.
/// Displays End popup.
/// Controlled by GameDetailsController.
/// </summary>
public class GameEndView : GamePopupBaseView
{
    [SerializeField]
    private Text _resultText;

    public Action _gameRestarted;

    public void DisplayResult ( string result )
    {
        _resultText.text = result;
    }

    public void Restart ( )
    {
        _gameRestarted.Invoke ( );
    }
}
