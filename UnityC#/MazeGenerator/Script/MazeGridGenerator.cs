using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MazeGridGenerator : MonoBehaviour
{
    class TileObject
    {
        public Transform transform;

        public TileObject(Transform t) { transform = t; }
    }

    class QuadrantObject
    {
        List<int> topWall;
        List<int> rightWall;
        List<int> bottomWall;
        List<int> leftWall;
        int topNeighbor = -1;
        int rightNeighbor = -1;
        int bottomNeighbor = -1;
        int leftNeighbor = -1;
        int rowWidth;
        int[] coordinate;
        int index;

        public bool visited;
        
        public void init(int i, int row, int column, int quadrantPerRow)
        {
            topWall = new List<int>();
            rightWall = new List<int>();
            bottomWall = new List<int>();
            leftWall = new List<int>();
            coordinate = new int[2];
            coordinate[0] = row;
            coordinate[1] = column;
            index = i;
            rowWidth = quadrantPerRow;
            visited = false;
            SetNeighbors();
        }
        public List<int> VisitTop() { return GetTopWall(); }

        public List<int> VisitRight() { return GetRightWall(); }

        public List<int> VisitBottom() { return GetBottomWall(); }

        public List<int> VisitLeft() { return GetLeftWall(); }

        // direction is the order of neighbors
        // 0 - top, 1 - right, 2 - bottom, 3 - left
        public int GetNeighbor(int direction){
            if(direction == 0) { return topNeighbor; }
            else if(direction == 1) { return rightNeighbor; }
            else if(direction == 2) { return bottomNeighbor; }
            else if(direction == 3) { return leftNeighbor; }
            else { Debug.LogWarning("Incorrect Direction Parameter In GetNeighbor"); return -1; }
        }

        public void SetTopWall(int index)
        {
            topWall.Add(index);
        }

        public List<int> GetTopWall() { return topWall; }

        public void SetRightWall(int index)
        {
            rightWall.Add(index);
        }

        public List<int> GetRightWall() { return rightWall; }

        public void SetBottomWall(int index)
        {
            bottomWall.Add(index);
        }

        public List<int> GetBottomWall() { return bottomWall; }

        public void SetLeftWall(int index)
        {
            leftWall.Add(index);
        }

        public List<int> GetLeftWall() { return leftWall; }

        public void SetNeighbors()
        {
            if(coordinate[1] < rowWidth - 1 )
            {
                rightNeighbor = index + 1;
            }
            if(coordinate[1] > 0)
            {
                leftNeighbor = index - 1;
            }
            if(coordinate[0] > 0)
            {
                topNeighbor = index - rowWidth;
            }
            if(coordinate[0] < rowWidth - 1)
            {
                bottomNeighbor = index + rowWidth;
            }
            
        }
    }

    public GameObject tile;
    public int tileWidth;
    public int tileCount;
    public int gridWidth;
    public int mazeWidth;
    public int quadrantPerRow;
    

    TileObject[] tileObjects;
    // for tidy code, return <MazeTileHandler> directly
    MazeTileHandler GetTileObject(int index) { return tileObjects[index].transform.GetComponent<MazeTileHandler>(); }

    QuadrantObject[] quadrantObjects;

    private void Start()
    {
        // make sure cellSize and RectTransform size is correct given
        // tileWidth & mazeWidth
        float sideLength = tileWidth * mazeWidth;
        GetComponent<GridLayoutGroup>().cellSize = new Vector2(tileWidth, tileWidth);
        GetComponent<RectTransform>().sizeDelta = new Vector2(sideLength, sideLength);

        // quadrantWidth is how many quadrant are there in a row
        // Square of it gives us the total number of quadrant on a grid
        int quadrantWidth = (mazeWidth - 1) / (gridWidth - 1);

        quadrantObjects = new QuadrantObject[quadrantWidth * quadrantWidth];
        for(int i = 0; i < quadrantWidth * quadrantWidth; i++)
        {
            int row = i / quadrantPerRow;
            int column = i - row * quadrantPerRow;
            quadrantObjects[i] = new QuadrantObject();
            quadrantObjects[i].init(i, row, column, quadrantPerRow);
        }

        Configure();
        AssignWalls();

        int totalQuadrant = quadrantPerRow * quadrantPerRow;
        GenerateMaze(0, Random.Range(0, totalQuadrant));
        
        /* visualize wall 
        for (int k = 0; k < 9; k++)
        {
            for (int j = 0; j < 5; j++)
            {
                int index = quadrantObjects[k].GetTopWall()[j];
                tileObjects[index].transform.GetComponent<Image>().color = new Color32(128, 32, 0, 200);
                index = quadrantObjects[k].GetRightWall()[j];
                tileObjects[index].transform.GetComponent<Image>().color = new Color32(128, 32, 0, 200);
                index = quadrantObjects[k].GetBottomWall()[j];
                tileObjects[index].transform.GetComponent<Image>().color = new Color32(128, 32, 0, 200);
                index = quadrantObjects[k].GetLeftWall()[j];
                tileObjects[index].transform.GetComponent<Image>().color = new Color32(128, 32, 0, 200);

            }
        }
        */
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            ResetWalls();
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            int totalQuadrant = quadrantPerRow * quadrantPerRow;
            GenerateMaze(0, Random.Range(0, totalQuadrant));
        }
    }

    void GenerateMaze(int QuadrantVisited, int currentIndex)
    {
        //if(QuadrantVisited == quadrantPerRow * quadrantPerRow) { return; }

        // set the current quadrant's visited record to true
        quadrantObjects[currentIndex].visited = true;
        QuadrantVisited++;

        // --------------------------------
        // indicate direction of travelling
        // 0 - 3 in random order
        List<int> directionIndexes = new List<int>();
        while(directionIndexes.Count != 4)
        {
            int index = Random.Range(0, 4);
            if (!directionIndexes.Contains(index)){
                directionIndexes.Add(index);
            }
        }
        //---------------------------------

        //---------------------------------
        // recursively visit neighbor quadrant associate to
        // current Quadrant
        // directionIndex[i] gets the direction of travel
        // GetNeighbor() gets the actual index of neighbor

        // for loop goes over all direction of a Quadrant
        for (int i = 0; i < directionIndexes.Count; i++)
        {
            int neighbor = quadrantObjects[currentIndex].GetNeighbor(directionIndexes[i]);
            // only proceed if neighbor hasn't been visited
            if (neighbor != -1 && !quadrantObjects[neighbor].visited)
            {
                // get the wall indexes according to direction of travel
                // store as temp List<int>
                List<int> temp;
                if(directionIndexes[i] == 0) { temp = quadrantObjects[currentIndex].GetTopWall(); }
                else if(directionIndexes[i] == 1) { temp = quadrantObjects[currentIndex].GetRightWall(); }
                else if (directionIndexes[i] == 2) { temp = quadrantObjects[currentIndex].GetBottomWall(); }
                else { temp = quadrantObjects[currentIndex].GetLeftWall(); }

                // call StripWall at TileHandler to turn a tile back to
                // normal state
                for (int j = 0; j < temp.Count; j++)
                {
                    GetTileObject(temp[j]).StripWall();
                }

                // recursively call Generate maze with
                // update QuadrantVisited count
                // neighbor becomes the current Quadrant
                GenerateMaze(QuadrantVisited, neighbor);
                //Debug.Log(QuadrantVisited);
                //if (QuadrantVisited == quadrantPerRow * quadrantPerRow) { return; }
            }
        }
        
    }

    void Configure()
    {
        tileObjects = new TileObject[tileCount];

        for (int i = 0; i < tileCount; i++)
        {
            GameObject go = Instantiate(tile) as GameObject;

            Transform t = go.transform;

            int row = i / mazeWidth;
            int column = i - row * mazeWidth;
            t.GetComponent<MazeTileHandler>().SetCoordinate(row, column);

            // left-most, right-most & row dividing tiles
            // Set horizontal walls and vertical by grid Width
            if (t.GetComponent<MazeTileHandler>().GetRow() % (gridWidth - 1) == 0
                || t.GetComponent<MazeTileHandler>().GetColumn() % (gridWidth - 1) == 0)
            {
                t.GetComponent<MazeTileHandler>().SetWall();
            }

            // horizontal walls of quadrant 0
            if(column > 0 && column < gridWidth - 1)
            {
                // top wall
                if(row == 0)
                {
                    quadrantObjects[0].SetTopWall(i);
                }
                else if(row == gridWidth - 1)
                {
                    quadrantObjects[0].SetBottomWall(i);
                }
            }

            if(row > 0 && row < gridWidth - 1)
            {
                if(column == 0)
                {
                    quadrantObjects[0].SetLeftWall(i);
                }
                else if(column == gridWidth - 1)
                {
                    quadrantObjects[0].SetRightWall(i);
                }
            }

            t.GetComponent<MazeTileHandler>().SetIndex(i);
            t.SetParent(transform);
            tileObjects[i] = new TileObject(t);
        }
    }

    void ResetWalls()
    {
        for(int i = 0; i < tileCount; i++)
        {
            if (GetTileObject(i).GetRow() % (gridWidth - 1) == 0
                || GetTileObject(i).GetColumn() % (gridWidth - 1) == 0)
            {
                GetTileObject(i).SetWall();
            }
        }

        for(int q = 0; q < quadrantObjects.Length; q++)
        {
            quadrantObjects[q].visited = false;
        }
    }

    // re-calculate wall index according to Quadrant[i]
    // then apply to Quadrant[i + 1], hence Quadrant[0]'s
    // wall is assigned from configure() method
    void AssignWalls()
    {
        // loop over all the quadrants
        // until the second last one to set wall index within the
        // object
        for(int i = 0; i < quadrantPerRow * quadrantPerRow - 1; i++)
        {
            RelocateWall(i, i + 1);
        }
    }

    // calculate indexes of 4 walls of given quadrant 
    // based on previous Quadrant passed in as parameter
    void RelocateWall(int prev, int next)
    {
        List<int> tempTop = quadrantObjects[prev].GetTopWall();
        List<int> tempRight = quadrantObjects[prev].GetRightWall();
        List<int> tempBottom = quadrantObjects[prev].GetBottomWall();
        List<int> tempLeft = quadrantObjects[prev].GetLeftWall();

        int offset;

        // next quadrant is beginning of a row
        // offset is shifted by number of rows 
        if(next % quadrantPerRow == 0) {
            // gridWidth - 2 is the interactable region
            // the rows until next quadrant
            // *(gridWidth - 1) * mazeWidth*
            // plus width of a grid 
            offset = (gridWidth - 2) * mazeWidth + gridWidth;
        }
        else
        {
            offset = gridWidth - 1;
        }
        
        for (int i = 0; i < gridWidth - 2; i++)
        {
            int updatedIndex = tempTop[i] + offset;
            quadrantObjects[next].SetTopWall(updatedIndex);
            updatedIndex = tempRight[i] + offset;
            quadrantObjects[next].SetRightWall(updatedIndex);
            updatedIndex = tempBottom[i] + offset;
            quadrantObjects[next].SetBottomWall(updatedIndex);
            updatedIndex = tempLeft[i] + offset;
            quadrantObjects[next].SetLeftWall(updatedIndex);
        }
    }
}
