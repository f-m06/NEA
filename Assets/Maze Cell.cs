using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell : MonoBehaviour
{
    // Forces Unity to serialize the private field
    [SerializeField]
    // Cell component
    private GameObject _leftWall;

    [SerializeField]
    private GameObject _rightWall;

    [SerializeField]
    private GameObject _frontWall;

    [SerializeField]
    private GameObject _backWall;

    [SerializeField]
    private GameObject _unvisitedCell;

    // Tracks whether the cell is visited during generation
    public bool IsVisited { get; private set; }

    // Called when a cell is visited during generation
    // unvisitedCell no longer visible
    public void Visit()
    {
        IsVisited = true;
        _unvisitedCell.SetActive(false);
    }

    public void ClearLeftWall()
    {
        _leftWall.SetActive(false);
    }
    
    public void ClearRightWall()
    { 
        _rightWall.SetActive(false); 
    }
    
    public void ClearFrontWall()
    {
        _frontWall.SetActive(false);
    }

    public void ClearBackWall()
    {
        _backWall.SetActive(false);
    }
}
