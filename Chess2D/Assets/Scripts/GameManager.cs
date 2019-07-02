using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Board mBoard;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Creating Board");
        mBoard.Create();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
