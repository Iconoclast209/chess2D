using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    #region FIELDS
    public Image mOutlineImage;
    public Vector2Int mBoardPosition = Vector2Int.zero;
    public Board mBoard = null;
    public RectTransform mRectTransform = null;
    public BasePiece mCurrentPiece = null;
    #endregion

    #region METHODS
    public void Setup(Vector2Int newBoardPosition, Board newBoard)
    {
        mBoardPosition = newBoardPosition;
        mBoard = newBoard;
        mRectTransform = GetComponent<RectTransform>();
    }

    public void ActivateOutline()
    {
        mOutlineImage.enabled = true;
        Debug.Log("Activating Outline.");
    }

    public void DeactivateOutline()
    {
        mOutlineImage.enabled = false;
        Debug.Log("Deactivating Outline.");
    }


    #endregion
}
