using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisBoard : MonoBehaviour
{
    //public Vector3 SpawnPoint;
    public Transform SpawnPoint;
    public TetrisBlock Prefab;

    // Start is called before the first frame update
    void Start()
    {
        //GameObject.Instantiate(Prefab, SpawnPoint, Quaternion.identity);    
        InstantiateTetrisBlock();    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InstantiateTetrisBlock() {
        var tetrisBlock = GameObject.Instantiate(Prefab, SpawnPoint.position, Quaternion.identity);
        tetrisBlock.Board = this;
    }
}
