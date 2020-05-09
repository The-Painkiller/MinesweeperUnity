using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Presenter class for MVP implementation of game details UI in Minesweeper.
/// </summary>
public class GameDetailsPresenter : MonoBehaviour
{
    [SerializeField]
    private GameStartView _startView;
    [SerializeField]
    private GameEndView _endView;
    [SerializeField]
    private GameplayUIView _gameplayUIVIew;

    private GameDetailsModel _model;

    public Action<GameDifficulty> _startGame;
    public Action _restartGame;

    private void Start ( )
    {
        _startView.TogglePopup ( true );

        _startView._gameStarted += GameStart;
        _endView._gameRestarted += GameRestart;
        _gameplayUIVIew._restartPressed += GameRestart;
    }

    /// <summary>
    /// Game start is called with chosen difficulty
    /// </summary>
    /// <param name="difficulty">Chosen difficulty level</param>
    private void GameStart ( GameDifficulty difficulty )
    {
        _startGame.Invoke ( difficulty );
        _model._isGameInactive = false;
        _startView.TogglePopup ( false );
    }

    /// <summary>
    /// Game Restart
    /// </summary>
    private void GameRestart ( )
    {
        _restartGame.Invoke ( );
        _endView.TogglePopup ( false );
    }

    /// <summary>
    /// Initialize with given Game Details model.
    /// </summary>
    /// <param name="model">Game details model</param>
    public void Initialize ( GameDetailsModel model)
    {
        _model = model;
    }

    /// <summary>
    /// Sends mine count to view to display in game UI.
    /// </summary>
    /// <param name="mineCount">Amount of mines</param>
    public void DisplayMineCount ( int mineCount )
    {
        _gameplayUIVIew.DisplayMineCount ( mineCount );
    }

    /// <summary>
    /// End game called.
    /// </summary>
    /// <param name="gameWon">Whether game was won at the end</param>
    public void EndGame ( bool gameWon )
    {
        _model._isGameInactive = true;

        string resultText = gameWon ? "Yay! All mines flagged!" : "Boom! You blew to smithereens!";
        _endView.DisplayResult ( resultText );
        _endView.TogglePopup ( true );
    }

    private void Update ( )
    {
        if ( _model == null )
            return;

        if ( _model._isGameInactive )
            return;

        ///Simple timer is run.
        _model._timerInSeconds += Time.deltaTime;
        _gameplayUIVIew.DisplayTimer ( (int)_model._timerInSeconds );
    }
}
