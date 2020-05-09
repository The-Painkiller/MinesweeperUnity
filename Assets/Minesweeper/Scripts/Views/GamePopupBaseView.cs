using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Base class for GameStartView and GameEndView.
/// Shows popup.
/// </summary>
public class GamePopupBaseView : MonoBehaviour
{
    protected GameObject _popupObject { get { return this.gameObject; } private set { } }

    public void TogglePopup ( bool show )
    {
        _popupObject.SetActive ( show );
    }
}
