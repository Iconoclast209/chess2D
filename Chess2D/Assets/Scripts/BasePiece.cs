using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public abstract class BasePiece : EventTrigger
{
    #region FIELDS
    public Color mColor = Color.clear;
    protected Cell mOriginalCell = null;
    protected Cell mCurrentCell = null;
    protected RectTransform mRectTransform = null;
    protected PieceManager mPieceManager;
    #endregion


    #region METHODS
    public virtual void Setup(Color newTeamColor, Color32 newSpriteColor, PieceManager newPieceManager)
    {
        mPieceManager = newPieceManager;
        mColor = newTeamColor;
        GetComponent<Image>().color = newSpriteColor;
        mRectTransform = GetComponent<RectTransform>();
    }

    public void Place(Cell newCell)
    {
        //Cell
        mCurrentCell = newCell;
        mOriginalCell = newCell;
        mCurrentCell.mCurrentPiece = this;

        //Object
        transform.position = newCell.transform.position;
        gameObject.SetActive(true);
    }

    #endregion
}
