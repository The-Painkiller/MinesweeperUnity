using System;
using UnityEngine;

/// <summary>
/// Presenter class for MVP implementation of tile in Minesweeper.
/// </summary>
public class TilePresenter : MonoBehaviour
{
    private TileModel _model;
    private TileView _view;

    /// <summary>
    /// Static Action call made when any tile is clicked. Sends Tile type and its coordinates on the grid as parameters. 
    /// Read by GameManager.
    /// </summary>
    public static Action<TileType, GridCoordinates> TileClicked;


    /// <summary>
    /// Initialization. Model & View set, and listeners to view attached.
    /// </summary>
    /// <param name="model">Tile Model</param>
    public void Initialize ( TileModel model)
    {
        _model = model;
        _view = GetComponent<TileView> ( );

        _view.LeftButtonClicked += OnLeftButton;
        _view.RightButtonClicked += OnRightButton;
    }

    /// <summary>
    /// Places tile on the grid.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="coordinates"></param>
    public void PlaceTile (Vector2 position, GridCoordinates coordinates )
    {
        _view._rectTransform.anchoredPosition = position;
        _model._coordinates = coordinates;
        _model._isCovered = true;
    }

    /// <summary>
    /// Assigns tile as empty, hint or mine. 
    /// In case of hints, assigns hint of number of mines around.
    /// </summary>
    /// <param name="tileType">Tile type(Mine/Empty/Hint</param>
    /// <param name="hints">If tile is a hint, assigns number of mines around as a hint.</param>
    public void AssignTile (TileType tileType, int hints = 0 )
    {        
        _model._tileType = tileType;
        _model._numberOfMines = hints;

        if ( tileType == TileType.Empty || tileType == TileType.Mine )
            _view.SetTileType ( tileType );
        else
            _view.SetTileHint ( hints );
    }

    /// <summary>
    /// Uncovers tile
    /// </summary>
    public void UncoverTile ( )
    {
        _model._isCovered = false;
        _view.Uncover ( );
    }

    /// <summary>
    /// Returns whether the tile is covered & is not a mine.
    /// </summary>
    /// <returns></returns>
    public bool IsSafeAndCovered ( )
    {
        return _model._isCovered && _model._tileType != TileType.Mine;
    }

    /// <summary>
    /// Returns tile type
    /// </summary>
    /// <returns></returns>
    public TileType GetTileType ( )
    {
        return _model._tileType;
    }

    /// <summary>
    /// Listener for left button click on this tile.
    /// </summary>
    private void OnLeftButton ( )
    {
        TileClicked?.Invoke ( _model._tileType, _model._coordinates );
    }

    /// <summary>
    /// Listener for right button click on this tile.
    /// </summary>
    private void OnRightButton ( )
    {
        if ( !_model._isCovered )
            return;

        _model._isFlagged = !_model._isFlagged;
        _view.ToggleFlag ( _model._isFlagged );
    }

    /// <summary>
    /// Destroyer
    /// </summary>
    private void OnDestroy ( )
    {
        _view.LeftButtonClicked -= OnLeftButton;
        _view.RightButtonClicked -= OnRightButton;
    }
}
