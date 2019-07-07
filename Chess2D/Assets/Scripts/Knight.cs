﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Knight : BasePiece
{
    public override void Setup(Color newTeamColor, Color32 newSpriteColor, PieceManager newPieceManager)
    {
        base.Setup(newTeamColor, newSpriteColor, newPieceManager);

        //mMovement = new Vector3Int(7, 7, 0);
        GetComponent<Image>().sprite = Resources.Load<Sprite>("T_Knight");
    }

    private void CreateCellPath(int flipper)
    {
        //Target Position
        int currentX = mCurrentCell.mBoardPosition.x;
        int currentY = mCurrentCell.mBoardPosition.y;

        //Left
        MatchesState(currentX - 2, currentY + (1 * flipper));
        
        //Upper Left
        MatchesState(currentX - 1, currentY + (2 * flipper));
        
        //Upper Right
        MatchesState(currentX + 1, currentY + (2 * flipper));
        
        //Right
        MatchesState(currentX + 2, currentY + (1 * flipper));
    }

    protected override void CheckPathing()
    {
        CreateCellPath(1);
        CreateCellPath(-1);
    }

    private void MatchesState(int targetX, int targetY)
    {
        CellState cellState = CellState.None;
        cellState = mCurrentCell.mBoard.ValidateCell(targetX, targetY, this);
        if(cellState != CellState.Friendly && cellState != CellState.OutOfBounds)
        {
            mHighlightedCells.Add(mCurrentCell.mBoard.mAllCells[targetX, targetY]);
        }
    }
}
