using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// View class for MVP implementation of game UI in Minesweeper.
/// Controlled by GameDetailsPresenter.
/// Displays basic UI during gameplay.
/// </summary>
public class GameplayUIView : MonoBehaviour
{
    [SerializeField]
    private Text _timerText;
    [SerializeField]
    private Text _totalMinesText;

    public Action _restartPressed;

    public void DisplayTimer ( int time )
    {
        _timerText.text = "Time: " + time.ToString ( );
    }

    public void DisplayMineCount ( int numOfMines )
    {
        _totalMinesText.text = "Mines: " + numOfMines.ToString ( );
    }

    public void Restart ( )
    {
        _restartPressed.Invoke ( );
    }
}
