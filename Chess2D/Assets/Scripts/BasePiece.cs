﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public abstract class BasePiece : EventTrigger
{
    #region FIELDS
    public Color mColor = Color.clear;
    public bool mIsFirstMove = true;
    protected Cell mOriginalCell = null;
    protected Cell mCurrentCell = null;
    protected RectTransform mRectTransform = null;
    protected PieceManager mPieceManager;
    protected Vector3Int mMovement = Vector3Int.one;
    protected List<Cell> mHighlightedCells = new List<Cell>();
    protected Cell mTargetCell = null;
    #endregion


    #region METHODS
    public virtual void Setup(Color newTeamColor, Color32 newSpriteColor, PieceManager newPieceManager)
    {
        mPieceManager = newPieceManager;
        mColor = newTeamColor;
        GetComponent<Image>().color = newSpriteColor;
        mRectTransform = GetComponent<RectTransform>();
    }

    public virtual void Place(Cell newCell)
    {
        //Cell
        mCurrentCell = newCell;
        mOriginalCell = newCell;
        mCurrentCell.mCurrentPiece = this;

        //Object
        transform.position = newCell.transform.position;
        gameObject.SetActive(true);
    }

    private void CreateCellPath(int xDirection, int yDirection, int movement)
    {
        //Target Position
        int currentX = mCurrentCell.mBoardPosition.x;
        int currentY = mCurrentCell.mBoardPosition.y;


        //Check Each Cell
        for(int i=1; i <= movement; i++)
        {
            currentX += xDirection;
            currentY += yDirection;

            CellState cellState = CellState.None;
            cellState = mCurrentCell.mBoard.ValidateCell(currentX, currentY, this);

            if (cellState == CellState.Enemy)
            {
                mHighlightedCells.Add(mCurrentCell.mBoard.mAllCells[currentX, currentY]);
                break;
            }
            if(cellState != CellState.Free)
            {
                break;
            }

            mHighlightedCells.Add(mCurrentCell.mBoard.mAllCells[currentX, currentY]);
        }
        
        
    }

    protected virtual void CheckPathing()
    {
        //Horizontal
        CreateCellPath(1, 0, mMovement.x);
        CreateCellPath(-1, 0, mMovement.x);

        //Vertical
        CreateCellPath(0, 1, mMovement.y);
        CreateCellPath(0, -1, mMovement.y);

        //Upper Diagonal
        CreateCellPath(1, 1, mMovement.z);
        CreateCellPath(-1, 1, mMovement.z);

        //Lower Diagonal
        CreateCellPath(-1, -1, mMovement.z);
        CreateCellPath(1, -1, mMovement.z);

    }

    protected void ShowCells()
    {
        foreach(Cell cell in mHighlightedCells)
        {
            cell.ActivateOutline();
        }
    }

    protected void ClearCells()
    {
        foreach(Cell cell in mHighlightedCells)
        {
            cell.DeactivateOutline();
        }
        mHighlightedCells.Clear();
    }
    #endregion

    #region EVENTS

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        //Test for allowed movement to cells
        CheckPathing();
        //Show valid cells
        ShowCells();
    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);

        //Follow Pointer
        transform.position += (Vector3)eventData.delta;

        foreach(Cell cell in mHighlightedCells)
        {
            if(RectTransformUtility.RectangleContainsScreenPoint(cell.mRectTransform, Input.mousePosition))
            {
                //If the mouse is within a valid cell, get it, and break
                mTargetCell = cell;
                break;
            }
            mTargetCell = null;
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);

        ClearCells();

        //Return to Original Position
        if(!mTargetCell)
        {
            transform.position = mCurrentCell.gameObject.transform.position;
            return;
        }
        Move();

        mPieceManager.SwitchSides(mColor);
    }

    public void Reset()
    {
        Kill();
        Place(mOriginalCell);
        mIsFirstMove = true;
    }

    public virtual void Kill()
    {
        //Clear Current Cell
        mCurrentCell.mCurrentPiece = null;
        //Remove Piece
        gameObject.SetActive(false);
    }

    protected virtual void Move()
    {
        //If there is an enemy piece, remove it
        mTargetCell.RemovePiece();

        //Clear Current Cell
        mCurrentCell.mCurrentPiece = null;

        //Switch Cells
        mCurrentCell = mTargetCell;
        mCurrentCell.mCurrentPiece = this;

        //Move on Board
        transform.position = mCurrentCell.transform.position;
        mTargetCell = null;
        mIsFirstMove = false;
    }

    #endregion
}
