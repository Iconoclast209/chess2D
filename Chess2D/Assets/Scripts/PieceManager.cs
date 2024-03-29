﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class PieceManager : MonoBehaviour
{
    #region FIELDS
    public GameObject mPiecePrefab;
    public bool mBothKingsAlive = true;
    private List<BasePiece> mWhitePieces = null;
    private List<BasePiece> mBlackPieces = null;
    private List<BasePiece> mPromotedPieces = new List<BasePiece>();

    private string[] mPieceOrder = new string[16]
    {
        "P", "P", "P", "P", "P", "P", "P", "P",
        "R", "KN", "B", "Q", "K", "B", "KN", "R"
    };

    private Dictionary<string, Type> mPieceLibrary = new Dictionary<string, Type>();
    #endregion

    #region METHODS
    private void Awake()
    {
        mPieceLibrary.Add("P", typeof(Pawn));
        mPieceLibrary.Add("R", typeof(Rook));
        mPieceLibrary.Add("KN", typeof(Knight));
        mPieceLibrary.Add("B", typeof(Bishop));
        mPieceLibrary.Add("K", typeof(King));
        mPieceLibrary.Add("Q", typeof(Queen));
    }

    public void Setup(Board board)
    {
        //Create the pieces
        mWhitePieces = CreatePieces(Color.white, Color.white, board);
        mBlackPieces = CreatePieces(Color.black, Color.black, board);

        //Place the pieces
        PlacePieces(1, 0, mWhitePieces, board);
        PlacePieces(6, 7, mBlackPieces, board);

        // White goes first
        SwitchSides(Color.black);
    }

    private List<BasePiece> CreatePieces(Color teamColor, Color spriteColor, Board board)
    {
        List<BasePiece> newPieces = new List<BasePiece>();

        for(int i =0; i < mPieceOrder.Length; i++)
        {
             //Get the Type, apply to new object
            string key = mPieceOrder[i];
            Type pieceType = mPieceLibrary[key];

            //Store New Piece
            BasePiece newPiece = CreatePiece(pieceType);
            newPieces.Add(newPiece);

            //Setup Piece
            newPiece.Setup(teamColor, spriteColor, this);
        }
        return newPieces;
    }

    private BasePiece CreatePiece(Type pieceType)
    {
        //Create new piece
        GameObject newPieceObject = Instantiate(mPiecePrefab);

        //Set the parent of the new piece to the Piece Manager
        newPieceObject.transform.SetParent(this.transform);

        // Set Scale and Rotation
        newPieceObject.transform.localScale = new Vector3(.75f, .75f, .75f);
        newPieceObject.transform.localRotation = Quaternion.identity;

        BasePiece newPiece = (BasePiece)newPieceObject.AddComponent(pieceType);
        return newPiece;
    }

    private void PlacePieces(int pawnRow, int royaltyRow, List<BasePiece> pieces, Board board)
    {
         for(int i = 0; i < 8; i++)
        {
            //Place Pawns
            pieces[i].Place(board.mAllCells[i, pawnRow]);

            //Place Royalty
            pieces[i + 8].Place(board.mAllCells[i, royaltyRow]);

        }
    }
  
    private void SetInteractive(List<BasePiece> allPieces, bool value)
    {
        foreach(BasePiece piece in allPieces)
        {
            piece.enabled = value;
        }
    }

    public void SwitchSides(Color color)
    {
        if(!mBothKingsAlive)
        {
            ResetPieces();
            mBothKingsAlive = true;
            color = Color.black;
        }

        bool isBlackTurn;
        if(color == Color.white)
        {
            isBlackTurn = true;
        }
        else
        {
            isBlackTurn = false;
        }

        SetInteractive(mWhitePieces, !isBlackTurn);
        SetInteractive(mBlackPieces, isBlackTurn);
        
        //Set Promoted Piece Interactivity
        foreach(BasePiece piece in mPromotedPieces)
        {
            bool isBlackPiece = piece.mColor != Color.white ? true : false;
            bool isPartOfTeam = isBlackPiece == true ? isBlackTurn : !isBlackTurn;
            piece.enabled = isPartOfTeam;
        }

    }

    public void ResetPieces()
    {
        //Destroy all promoted pieces
        foreach (BasePiece piece in mPromotedPieces)
        {
            piece.Kill();
            Destroy(piece.gameObject);
        }
        mPromotedPieces.Clear();

        foreach (BasePiece piece in mWhitePieces)
        {
            piece.Reset();
        }

        foreach(BasePiece piece in mBlackPieces)
        {
            piece.Reset();
        }
        
    }

    public void PromotePiece(Pawn pawn, Cell cell, Color teamColor, Color spriteColor)
    {
        //Kill the Pawn
        pawn.Kill();
        //Create new Queen
        BasePiece promotedPiece = CreatePiece(typeof(Queen));
        promotedPiece.Setup(teamColor, spriteColor, this);
        //Place the new Queen
        promotedPiece.Place(cell);
        //Add to list
        mPromotedPieces.Add(promotedPiece);
    }
    #endregion

}
