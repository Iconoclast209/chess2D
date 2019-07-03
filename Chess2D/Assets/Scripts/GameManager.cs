using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Board mBoard;
    public PieceManager mPieceManager;
    
    // Start is called before the first frame update
    void Start()
    {
        // Create the board
        mBoard.Create();

        //Create the pieces
        mPieceManager.Setup(mBoard);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
