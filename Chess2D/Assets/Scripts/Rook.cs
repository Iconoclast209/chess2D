using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rook : BasePiece
{
    public Cell mCastleTriggerCell = null;
    public Cell mCastleCell = null;

    public override void Setup(Color newTeamColor, Color32 newSpriteColor, PieceManager newPieceManager)
    {
        base.Setup(newTeamColor, newSpriteColor, newPieceManager);

        mMovement = new Vector3Int(7, 7, 0);
        GetComponent<Image>().sprite = Resources.Load<Sprite>("T_Rook");
    }

    public override void Place(Cell newCell)
    {
        base.Place(newCell);

        //Trigger Cell
        int triggerOffset = mCurrentCell.mBoardPosition.x < 4 ? 2 : -1;
        mCastleTriggerCell = SetCell(triggerOffset);

        //Castle Cell
        int castleOffset = mCurrentCell.mBoardPosition.x < 4 ? 3 : -2;
        mCastleCell = SetCell(castleOffset);
    }

    public void Castle()
    {
        //Set target
        mTargetCell = mCastleCell;

        //Moving
        Move();
    }

    private Cell SetCell(int offset)
    {
        //Create a new position
        Vector2Int newPosition = mCurrentCell.mBoardPosition;
        newPosition.x += offset;
        //Return that Cell
        return mCurrentCell.mBoard.mAllCells[newPosition.x, newPosition.y];
    }
}
