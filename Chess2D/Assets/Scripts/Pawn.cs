﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pawn : BasePiece
{
    public override void Setup(Color newTeamColor, Color32 newSpriteColor, PieceManager newPieceManager)
    {
        base.Setup(newTeamColor, newSpriteColor, newPieceManager);
        mIsFirstMove = true;
        if(mColor == Color.white)
        {
            mMovement = new Vector3Int(0, 1, 1);
        }
        else
        {
            mMovement = new Vector3Int(0, -1, -1);
        }
        GetComponent<Image>().sprite = Resources.Load<Sprite>("T_Pawn");
    }

    protected override void Move()
    {
        base.Move();
        mIsFirstMove = false;
        CheckForPromotion();
    }

    private bool MatchesState(int targetX, int targetY, CellState targetState)
    {
        CellState cellState = CellState.None;
        cellState = mCurrentCell.mBoard.ValidateCell(targetX, targetY, this);

        if(cellState == targetState)
        {
            mHighlightedCells.Add(mCurrentCell.mBoard.mAllCells[targetX, targetY]);
            return true;
        }
        return false;
    }

    private void CheckForPromotion()
    {
        //Target Cell Location
        int currentX = mCurrentCell.mBoardPosition.x;
        int currentY = mCurrentCell.mBoardPosition.y;

        CellState cellState = mCurrentCell.mBoard.ValidateCell(currentX, currentY + mMovement.y, this);

        if (cellState == CellState.OutOfBounds)
        {
            Color spriteColor = GetComponent<Image>().color;
            mPieceManager.PromotePiece(this, mCurrentCell, mColor, spriteColor);
        }

    }

    protected override void CheckPathing()
    {
        //Target Cell Location
        int currentX = mCurrentCell.mBoardPosition.x;
        int currentY = mCurrentCell.mBoardPosition.y;

        MatchesState(currentX - mMovement.z, currentY + mMovement.z, CellState.Enemy);
        MatchesState(currentX + mMovement.z, currentY +  mMovement.z, CellState.Enemy);

        if (MatchesState(currentX, currentY + mMovement.y, CellState.Free))
        {
            if(mIsFirstMove)
            {
                MatchesState(currentX, currentY + (mMovement.y * 2), CellState.Free);
            }
        }
    }

}
