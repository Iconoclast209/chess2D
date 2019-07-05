using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    #region FIELDS
    public GameObject mOutlineImage;
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
        mOutlineImage.SetActive(true);
        Debug.Log("Activating Outline.");
    }

    public void DeactivateOutline()
    {
        mOutlineImage.SetActive(false);
        Debug.Log("Deactivating Outline.");
    }

    public void RemovePiece()
    {
        if (mCurrentPiece != null)
        {
            mCurrentPiece.Kill();
        }
    }
        

    #endregion
}
