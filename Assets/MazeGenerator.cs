using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    // Forces Unity to serialize the private field
    [SerializeField]
    private MazeCell _mazeCellPrefab;

    // Maze component
    [SerializeField]
    private int _mazeWidth;

    [SerializeField]
    private int _mazeDepth;

    // 2D array holds the grid of Maze Cells
    private MazeCell[,] _mazeGrid;

    // Initialises array
    IEnumerator Start()
    {
        // Uses width and depth
        _mazeGrid = new MazeCell[_mazeWidth, _mazeDepth];

        // Loops from 0 to width of maze
        for (int x = 0; x < _mazeWidth; x++)
        {
            // Loops from 0 to depth of maze
            for (int z = 0; z < _mazeDepth; z++)
            {
                // Scale of cell is 1, cells are positioned by loop, rotation is read-only (no rotation)
                _mazeGrid[x, z] = Instantiate(_mazeCellPrefab, new Vector3(x, 0, z), Quaternion.identity);
            }
        }

        yield return GenerateMaze(null, _mazeGrid[0, 0]);
    }

    // Coroutine called recursively
    private IEnumerator GenerateMaze(MazeCell previousCell, MazeCell currentCell)
    {
        // Current cell is visble
        currentCell.Visit();
        ClearWalls(previousCell, currentCell);

        // Visibly generating, 1 cell generates every 1 millisecond
        yield return new WaitForSeconds(0.001f);

        MazeCell nextCell;

        do
        {
            nextCell = GetNextUnvisitedCell(currentCell);

            if (nextCell != null)
            {
                yield return GenerateMaze(currentCell, nextCell);
            }
        } while (nextCell != null);
    }

    // Finds next unvisited adjacent cell
    private MazeCell GetNextUnvisitedCell(MazeCell currentCell)
    {
        var unvisitedCells = GetUnvisitedCells(currentCell);

        // Ordered by random
        return unvisitedCells.OrderBy(_ => Random.Range(1, 10)).FirstOrDefault();
    }

    // Returns all unvisited cells
    private IEnumerable<MazeCell> GetUnvisitedCells(MazeCell currentCell)
    {
        int x = (int)currentCell.transform.position.x;
        int z = (int)currentCell.transform.position.z;

        // Checks if within the Maze Grid
        if (x + 1 < _mazeWidth)
        {
            var cellToRight = _mazeGrid[x + 1, z];

            if (cellToRight.IsVisited == false)
            {
                // Potential return
                yield return cellToRight;
            }
        }

        if (x - 1 >= 0)
        {
            var cellToLeft = _mazeGrid[x - 1, z];

            if (cellToLeft.IsVisited == false)
            {
                yield return cellToLeft;
            }
        }

        if (z + 1 < _mazeDepth)
        {
            var cellToFront = _mazeGrid[x, z + 1];

            if (cellToFront.IsVisited == false)
            {
                yield return cellToFront;
            }
        }

        if (z - 1 >= 0)
        {
            var cellToBack = _mazeGrid[x, z - 1];

            if (cellToBack.IsVisited == false)
            {
                yield return cellToBack;
            }
        }
    }

    // Removes walls between previous cell and current cell
    private void ClearWalls(MazeCell previousCell, MazeCell currentCell)
    {
        // No previous cell at start
        if (previousCell == null)
        {
            return;
        }

        // Checks if position of previous cell is to the left of current cell
        if (previousCell.transform.position.x < currentCell.transform.position.x)
        {
            previousCell.ClearRightWall();
            currentCell.ClearLeftWall();
            return;
        }

        // Checks if position of previous cell is to the right of current cell
        if (previousCell.transform.position.x > currentCell.transform.position.x)
        {
            previousCell.ClearLeftWall();
            currentCell.ClearRightWall();
            return;
        }

        // Checks if position of previous cell is to the back of current cell
        if (previousCell.transform.position.z < currentCell.transform.position.z)
        {
            previousCell.ClearFrontWall();
            currentCell.ClearBackWall();
            return;
        }

        // Checks if position of previous cell is to the front of current cell
        if (previousCell.transform.position.z > currentCell.transform.position.z)
        {
            previousCell.ClearBackWall();
            currentCell.ClearFrontWall();
            return;
        }
    }
}