using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Main Game manager.
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField]
    private TilePresenter _tilePrefab;
    [SerializeField]
    private RectTransform _gridParent;
    [SerializeField]
    private GameDetailsPresenter _detailsPresenter;
    [SerializeField]
    private int _spacingX;
    [SerializeField]
    private int _spacingY;


    private int _totalRowCount = 0;
    private int _totalColumnCount = 0;
    private int _totalMineCount = 0;

    private TilePresenter [,] _gameGridTiles { get; set; }

    /// <summary>
    /// Singleton
    /// </summary>
    public static GameManager _instance;

    private void Awake ( )
    {
        if ( _instance == null )
        {
            _instance = this;
        }
    }

    private void OnDestroy ( )
    {
        TilePresenter.TileClicked -= OnTileClicked;
        _gameGridTiles = null;
    }

    /// <summary>
    /// Initial setting up.
    /// </summary>
    private void Start ( )
    {
        _detailsPresenter._startGame += InitializeGame;
        _detailsPresenter._restartGame += RestartGame;

        GameDetailsModel gameDetailsModel = new GameDetailsModel ( );
        gameDetailsModel._isGameInactive = true;
        gameDetailsModel._timerInSeconds = 0;
        _detailsPresenter.Initialize ( gameDetailsModel);
    }

    /// <summary>
    /// Game is initialized with given difficulty.
    /// New GameModel is created accordingly and passed to GamePresenter.
    /// </summary>
    /// <param name="difficulty">Game Difficulty</param>
    private void InitializeGame ( GameDifficulty difficulty)
    {
        switch ( difficulty )
        {
            case GameDifficulty.Easy:
                _totalRowCount = 5;
                _totalColumnCount = 3;
                _totalMineCount = 3;
                break;

            case GameDifficulty.Medium:
                _totalRowCount = 7;
                _totalColumnCount = 5;
                _totalMineCount = 6;
                break;

            case GameDifficulty.Hard:
                _totalRowCount = 10;
                _totalColumnCount = 6;
                _totalMineCount = 8;
                break;

            default:
                break;
        }

        _detailsPresenter.DisplayMineCount ( _totalMineCount );
        CreateGame (_totalRowCount, _totalColumnCount, _totalMineCount );
    }

    /// <summary>
    /// Game Creation. TIles instantiated.
    /// </summary>
    /// <param name="rows">Number of rows in the grid</param>
    /// <param name="cols">Number of columns in the grid</param>
    /// <param name="mines">Number of mines in the grid</param>
    private void CreateGame ( int rows, int cols, int mines)
    {
        TilePresenter.TileClicked += OnTileClicked;

        Vector2 _tileDimensions;

        _gameGridTiles = new TilePresenter [ rows, cols ];
        _tileDimensions = _tilePrefab.GetComponent<RectTransform> ( ).sizeDelta;

        Vector2 lastTilePosition = Vector2.zero;
        Vector2 gridSpacing = Vector2.zero;

        for ( int i = 0; i < _gameGridTiles.GetLength ( 0 ); i++ )
        {
            for ( int j = 0; j < _gameGridTiles.GetLength ( 1 ); j++ )
            {
                TileModel gameTileModel = new TileModel ( );

                _gameGridTiles [ i, j ] = Instantiate<TilePresenter> ( _tilePrefab, _gridParent );
                _gameGridTiles [ i, j ].Initialize ( gameTileModel );

                GridCoordinates gridCoordinates;
                gridCoordinates.row = i;
                gridCoordinates.column = j;

                Vector2 tilePos = new Vector2 ( lastTilePosition.x + gridSpacing.x, -( lastTilePosition.y + gridSpacing.y ) );

                _gameGridTiles [ i, j ].PlaceTile ( tilePos, gridCoordinates );

                lastTilePosition.x += _tileDimensions.x;
                gridSpacing.x += _spacingX;
            }
            lastTilePosition.x = 0;
            lastTilePosition.y += _tileDimensions.y;
            gridSpacing.x = 0;
            gridSpacing.y += _spacingY;
        }

        AssignTiles ( );
    }

    /// <summary>
    /// Assigns random tiles as either empty or mines
    /// </summary>
    private void AssignTiles ( )
    {
        List<GridCoordinates> minesCoordinates = new List<GridCoordinates> ( );
        GridCoordinates tempCoordinates;
        for ( int i = 0; i < _totalMineCount; i++ )
        {
            do
            {
                tempCoordinates = GetRandomGridCoordinates ( );
            } while ( minesCoordinates.Contains ( tempCoordinates ) );

            minesCoordinates.Add ( tempCoordinates );
        }

        for ( int i = 0; i < minesCoordinates.Count; i++ )
        {
            _gameGridTiles [ minesCoordinates [ i ].row, minesCoordinates [ i ].column ].AssignTile ( TileType.Mine );
        }

        AssignHints ( );
    }

    /// <summary>
    /// Calculates and assigns tiles near mines as hints.
    /// </summary>
    private void AssignHints ( )
    {
        for ( int i = 0; i < _totalRowCount; i++ )
        {
            for ( int j = 0; j < _totalColumnCount ; j++ )
            {
                if ( _gameGridTiles [ i, j ].GetTileType ( ) == TileType.Mine )
                    continue;

                int mineCount = 0;

                if ( j - 1 >= 0 && _gameGridTiles [ i, j - 1 ].GetTileType() == TileType.Mine ) //left
                    mineCount++;
                if ( j + 1 < _gameGridTiles.GetLength ( 1 ) && _gameGridTiles [ i, j + 1 ].GetTileType ( ) == TileType.Mine )//right
                    mineCount++;

                if ( i - 1 >= 0 )
                {
                    if ( _gameGridTiles [ i - 1, j ].GetTileType ( ) == TileType.Mine )//up-center
                        mineCount++;
                    if ( j - 1 >= 0 && _gameGridTiles [ i - 1, j - 1 ].GetTileType ( ) == TileType.Mine ) //up-left
                        mineCount++;
                    if ( j + 1 < _gameGridTiles.GetLength ( 1 ) && _gameGridTiles [ i - 1, j + 1 ].GetTileType ( ) == TileType.Mine )//up-right
                        mineCount++;
                }

                if ( i + 1 < _gameGridTiles.GetLength ( 0 ) )
                {
                    if ( _gameGridTiles [ i + 1, j ].GetTileType ( ) == TileType.Mine )//down-center
                        mineCount++;
                    if ( j - 1 >= 0 && _gameGridTiles [ i + 1, j - 1 ].GetTileType ( ) == TileType.Mine )//down-left
                        mineCount++;
                    if ( j + 1 < _gameGridTiles.GetLength ( 1 ) && _gameGridTiles [ i + 1, j + 1 ].GetTileType ( ) == TileType.Mine )//down-right
                        mineCount++;
                }

                GridCoordinates gc;
                gc.row = i;
                gc.column = j;
                if ( mineCount > 0 )
                    _gameGridTiles [ i, j ].AssignTile ( TileType.Hint, mineCount );
            }
        }
    }

    /// <summary>
    /// Listener for Tile Clicked.
    /// </summary>
    /// <param name="type">Type of the tile</param>
    /// <param name="coordinates">Coordinates of the tile on the grid.</param>
    private void OnTileClicked ( TileType type, GridCoordinates coordinates )
    {
        switch ( type )
        {
            case TileType.Hint:
                _gameGridTiles [ coordinates.row, coordinates.column ].UncoverTile ( );
                break;

            case TileType.Empty:
                UncoverEmpty (coordinates );
                break;

            case TileType.Mine:
                UncoverAllMines ( );
                break;
        }

        CheckGameStatus ( type );
    }


    /// <summary>
    /// Restarts game by calling the scene again from scratch.
    /// </summary>
    private void RestartGame ( )
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene ( "EmptySpace" , UnityEngine.SceneManagement.LoadSceneMode.Single);
    }

    /// <summary>
    /// Uncovers an empty tile along with all empty tiles connected to it.
    /// </summary>
    /// <param name="gridID"></param>
    public void UncoverEmpty ( GridCoordinates gridID )
    {
        if ( !CheckGridCellValid ( gridID ) )
            return;

        if ( !_gameGridTiles [ gridID.row, gridID.column ].IsSafeAndCovered() )
            return;

        if ( _gameGridTiles [ gridID.row, gridID.column ].GetTileType() == TileType.Hint )
            return;

        _gameGridTiles [ gridID.row, gridID.column ].UncoverTile ( );

        GridCoordinates gridLeft = gridID;
        gridLeft.column -= 1;
        GridCoordinates gridRight = gridID;
        gridRight.column += 1;
        GridCoordinates gridUp = gridID;
        gridUp.row -= 1;
        GridCoordinates gridDown = gridID;
        gridDown.row += 1;

        UncoverEmpty ( gridLeft );
        UncoverEmpty ( gridRight );
        UncoverEmpty ( gridUp );
        UncoverEmpty ( gridDown );
    }

    /// <summary>
    /// Shows all mines after losing game.
    /// </summary>
    public void UncoverAllMines ( )
    {
        for ( int i = 0; i < _gameGridTiles.GetLength ( 0 ); i++ )
        {
            for ( int j = 0; j < _gameGridTiles.GetLength ( 1 ); j++ )
            {
                if ( _gameGridTiles [ i, j ].GetTileType() == TileType.Mine )
                {
                    GridCoordinates coordinates;
                    coordinates.column = j;
                    coordinates.row = i;
                    _gameGridTiles [ coordinates.row, coordinates.column ].UncoverTile ( );
                }
            }
        }
    }

    /// <summary>
    /// Checks if the game is over yet.
    /// Calls End game if a mine was hit or all tiles were uncovered.
    /// </summary>
    /// <param name="tileType"></param>
    private void CheckGameStatus ( TileType tileType )
    {
        if ( tileType == TileType.Mine )
        {
            _detailsPresenter.EndGame ( false );
        }
        else if ( IsGameSuccessful ( ) )
        {
            _detailsPresenter.EndGame ( true );
        }        
    }

   /// <summary>
   /// Checks every time whether the game was successful on the basis of each tile being uncovered and without a mine.
   /// </summary>
   /// <returns></returns>
    private bool IsGameSuccessful ( )
    {
        for ( int i = 0; i < _gameGridTiles.GetLength ( 0 ); i++ )
        {
            for ( int j = 0; j < _gameGridTiles.GetLength ( 1 ); j++ )
            {
                if ( _gameGridTiles [ i, j ].IsSafeAndCovered ( ) )
                    return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Basic check used by all functions in View to see if the passed coordinates are valid or not.
    /// </summary>
    /// <param name="gridID"></param>
    /// <returns></returns>
    private bool CheckGridCellValid ( GridCoordinates gridID )
    {
        return gridID.row >= 0 && gridID.column >= 0 && gridID.row < _gameGridTiles.GetLength ( 0 ) && gridID.column < _gameGridTiles.GetLength ( 1 );
    }

    /// <summary>
    /// Random tile coordinates are generated.
    /// </summary>
    /// <returns></returns>
    private GridCoordinates GetRandomGridCoordinates ( )
    {
        GridCoordinates gc;

        gc.row = UnityEngine.Random.Range ( 0, _totalRowCount - 1 );
        gc.column = UnityEngine.Random.Range ( 0, _totalColumnCount - 1 );

        return gc;
    }
}
