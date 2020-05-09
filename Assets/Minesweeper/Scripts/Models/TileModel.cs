/// <summary>
/// Model class for MVP implementation of the Tile in Minesweeper.
/// </summary>
public class TileModel
{
    public bool _isCovered;
    public bool _isFlagged;
    public int _numberOfMines;
    public GridCoordinates _coordinates;
    public TileType _tileType;
}
