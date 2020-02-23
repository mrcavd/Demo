using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MazeTileHandler : MonoBehaviour
{
    public Transform icon;
    
    bool wall;
    int index;

    int[] coordinate;

    private void Awake()
    {
        coordinate = new int[2];
    }

    public void SetIndex(int i) { index = i; }

    public int GetIndex() { return index; }

    public void SetCoordinate(int row, int column) { coordinate[0] = row; coordinate[1] = column; }

    public int[] GetCoordinate() { return coordinate; }

    public int GetRow() { return coordinate[0]; }

    public int GetColumn() { return coordinate[1]; }

    public void SetWall()
    {
        wall = true;
        GetComponent<Image>().color = new Color32(128, 32, 0, 255);
    }

    public bool GetWall() { return wall; }

    public void StripWall() {
        wall = false;
        GetComponent<Image>().color = new Color32(255, 255, 255, 255);
    }
   
}
