using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CellState
{
    None,
    Friendly,
    Enemy,
    Free,
    OutOfBounds
}

public class Board : MonoBehaviour
{
    #region FIELDS
    public GameObject mCellPrefab;
    [HideInInspector]
    public Cell[,] mAllCells = new Cell[8, 8];
    #endregion

    #region METHODS
    public void Create()
    {
        for (int y = 0; y < 8; y++)
        {
            for(int x = 0; x < 8; x++)
            {
                //Create the Cell
                GameObject newCell = Instantiate(mCellPrefab, transform);
                //Position the Cell
                RectTransform rectTransform = newCell.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2((x*100) + 50,(y*100) + 50);
                //Setup the Cell
                mAllCells[x, y] = newCell.GetComponent<Cell>();
                mAllCells[x, y].Setup(new Vector2Int(x, y), this);
            }
        }

        for(int x = 0; x < 8; x += 2)
        {
            for(int y = 0; y < 8; y++)
            {
                int offset = (y % 2 != 0) ? 0 : 1;
                int finalX = x + offset;
                mAllCells[finalX, y].GetComponent<Image>().color = new Color32(230, 220, 187, 255);
            }
        }
    }
    
    public CellState ValidateCell(int targetX, int targetY, BasePiece checkingPiece)
    {
        //Bounds check
        if(targetX < 0 || targetX > 7)
        {
            return CellState.OutOfBounds;
        }

        if(targetY < 0 || targetY > 7)
        {
            return CellState.OutOfBounds;
        }

        //Get Cell
        Cell targetCell = mAllCells[targetX, targetY];

        //If the Cell has a piece
        if(targetCell.mCurrentPiece != null)
        {
            if(checkingPiece.mColor == targetCell.mCurrentPiece.mColor)
            {
                return CellState.Friendly;
            }

            if(checkingPiece.mColor != targetCell.mCurrentPiece.mColor)
            {
                return CellState.Enemy;
            }
        }

        return CellState.Free;
    }
    #endregion
}
