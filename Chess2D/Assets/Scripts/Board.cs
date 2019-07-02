using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    #region FIELDS
    public GameObject mCellPrefab;
    public Cell[,] mAllCells = new Cell[8, 8];
    #endregion

    #region METHODS
    public void Create()
    {
        for (int y = 0; y < 8; y++)
        {
            for(int x = 0;x < 8;x++)
            {
                //Create the Cell
                GameObject newCell = Instantiate(mCellPrefab, transform);
                //Position the Cell
                RectTransform rectTransform = newCell.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2((x*100) + 50,(y*100) + 50);
                //Setup the Cell
                mAllCells[x, y] = newCell.GetComponent<Cell>();
                mAllCells[x, y].Setup(new Vector2Int(x, y), this);
                Debug.Log("Created new cell at " + x + ", " + y);

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
    #endregion
}
