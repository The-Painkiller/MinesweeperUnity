using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// View class for MVP implementation of the tile in Minesweeper.
/// </summary>
public class TileView : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private Text _tileHint;
    [SerializeField]
    private GameObject _mineImage;
    [SerializeField]
    private Image _cover;
    [SerializeField]
    private Sprite _flagSprite;

    /// <summary>
    /// Action calls for left & right button clicks on this tile.
    /// Listened by its controller class.
    /// </summary>
    public Action LeftButtonClicked;
    public Action RightButtonClicked;

   /// <summary>
   /// Returns this tile's RectTransform.
   /// </summary>
    public RectTransform _rectTransform
    {
        get
        {
            return GetComponent<RectTransform> ( );
        }
    }

    /// <summary>
    /// Sets the tile type visually
    /// </summary>
    /// <param name="type">Tile type.</param>
    public void SetTileType ( TileType type )
    {
        switch ( type )
        {
            case TileType.Empty:
                _tileHint.gameObject.SetActive ( false );
                _mineImage.SetActive ( false );
                break;

            case TileType.Hint:
                _tileHint.gameObject.SetActive ( true );
                _mineImage.SetActive ( false );
                break;

            case TileType.Mine:
                _tileHint.gameObject.SetActive ( false );
                _mineImage.SetActive ( true );
                break;
        }
    }

    /// <summary>
    /// Assigns the tile as a hint
    /// </summary>
    /// <param name="numberOfMines">Hint of number of mines around</param>
    public void SetTileHint ( int numberOfMines )
    {
        _tileHint.text = numberOfMines.ToString ( );
        SetTileType ( TileType.Hint );
    }

    /// <summary>
    /// Uncovers the tile.
    /// </summary>
    public void Uncover ( )
    {
        Debug.Log ( "Instance ID: " + _cover.GetInstanceID());
        _cover.gameObject.SetActive ( false );
    }

    /// <summary>
    /// Toggles the flag.
    /// </summary>
    public void ToggleFlag ( bool isFlagged)
    {
        _cover.sprite = isFlagged ? _flagSprite : null;
    }

    /// <summary>
    /// Implementation of IPointerClick interface.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick ( PointerEventData eventData )
    {
        switch ( eventData.button )
        {
            case PointerEventData.InputButton.Left:
                LeftButtonClicked?.Invoke();
                break;

            case PointerEventData.InputButton.Right:
                RightButtonClicked?.Invoke ( );
                break;
        }
    }
}
