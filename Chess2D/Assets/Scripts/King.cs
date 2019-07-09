using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class King : BasePiece
{
    private Rook mLeftRook = null;
    private Rook mRightRook = null;

    public override void Setup(Color newTeamColor, Color32 newSpriteColor, PieceManager newPieceManager)
    {
        base.Setup(newTeamColor, newSpriteColor, newPieceManager);

        mMovement = new Vector3Int(1, 1, 1);
        GetComponent<Image>().sprite = Resources.Load<Sprite>("T_King");
    }

    public override void Kill()
    {
        base.Kill();
        mPieceManager.mBothKingsAlive = false;
    }

    protected override void CheckPathing()
    {
        base.CheckPathing();
        //Set Right Rook
        mRightRook = GetRook(1, 3);
        //Set Left Rook
        mLeftRook = GetRook(-1, 4);
    }

    protected override void Move()
    {
        base.Move();
        //Additional movement for Rook
        if(CanCastle(mLeftRook))
        {
            mLeftRook.Castle();
        }

        if (CanCastle(mRightRook))
        {
            mRightRook.Castle();
        }
    }

    private bool CanCastle(Rook rook)
    {
        if(rook == null)
        {
            return false;
        }
        
        if(rook.mCastleTriggerCell != mCurrentCell)
        {
            return false;
        }

        return true;
    }

    private Rook GetRook(int direction, int count)
    {
        //Has the King moved?
        if(!mIsFirstMove)
        {
            return null;
        }

        //Get Position of King
        int currentX = mCurrentCell.mBoardPosition.x;
        int currentY = mCurrentCell.mBoardPosition.y;

        //Are there any pieces between the King and the Rook?
        for (int i = 1; i < count; i++)
        {
            int offsetX = currentX + (i * direction);
            CellState cellState = mCurrentCell.mBoard.ValidateCell(offsetX, currentY, this);
            if(cellState != CellState.Free)
            {
                return null;
            }
        }

        //Try to get a Rook
        Cell rookCell = mCurrentCell.mBoard.mAllCells[currentX + (count * direction), currentY];
        Rook rook = null;


        //Cast
        if(rookCell.mCurrentPiece != null)
        {
            if (rookCell.mCurrentPiece is Rook)
            {
                rook = (Rook)rookCell.mCurrentPiece;
            }
        }
        
        if(rook.mColor != mColor || !rook.mIsFirstMove)
        {
            return null;
        }

        if(rook == null)
        {
            return null;
        }

        //Add Castle Trigger Cell to movement list
        if(rook != null)
        {
            mHighlightedCells.Add(rook.mCastleTriggerCell);
        }

        return rook;
    }
}
