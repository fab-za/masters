using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageGridSlider : MonoBehaviour
{
    public float sizeFactor;
    private int vertical, horizontal, columns, rows;
    public Sprite sprite;
    // public float x_boundary;
    // public float y_boundary;
    [System.Serializable]
    public struct GridParameters{
        public int type;
        public int panel;    // left = -1, right = +1
        public float size;
        public float spacing;

        public GridParameters(int t, int p, float si, float sp){
            type = t;
            panel = p;
            size = si;
            spacing = sp;
        }
    }
    [System.Serializable]
    public struct SliderValues{
        public float size_left;
        public float size_right;
        public float spacing_left;
        public float spacing_right;

        public SliderValues(float sil, float sir, float spl, float spr){
            size_left = sil;
            size_right = sir;
            spacing_left = spl;
            spacing_right = spr;
        }
    }
    public GridParameters leftGrid;
    public GridParameters rightGrid;
    public SliderValues slider;
    // public int trial = 0;
    // private GameObject newGrid;

    void Start()
    {
        vertical = (int) Camera.main.orthographicSize;
        horizontal = vertical * (Screen.width / Screen.height);
        columns = horizontal * (int)(3.5f/sizeFactor);
        rows = vertical * (int)(2.2f/sizeFactor);
        // grid = new float[columns, rows];

        Debug.Log(columns);
        Debug.Log(rows);

        leftGrid = new GridParameters(0,-1,1,0);
        rightGrid = new GridParameters(0,1,1,0);

        slider = new SliderValues(1,1,0,0);

        // updateGrid();
        // newGrid = new GameObject("grid");
    }

    // Update is called once per frame
    void Update()
    {
        updateGridParameters();
    }

    private void SpawnSingleTile(float x, float y, float size, GameObject parentGrid){
        GameObject g = new GameObject("x: "+x+" y: "+y);
        g.AddComponent<BoxCollider2D>();
        var s = g.AddComponent<SpriteRenderer>();
        s.sprite = sprite;

        g.transform.position = new Vector3(x,y,0);
        g.transform.parent = parentGrid.transform;
        g.transform.localScale = new Vector3(size, size, 0);
        
    }

    private void GenerateGrid(GridParameters parameters, GameObject grid){
        float total_spacing = parameters.size + parameters.spacing;

        for (int j = 0; j < rows; j++){
            for (int i = 0; i < columns; i++){
                float pos_x = this.gameObject.transform.position.x + (i * total_spacing  * parameters.panel);
                float pos_y = this.gameObject.transform.position.y + (j * total_spacing);
                SpawnSingleTile(pos_x, pos_y, parameters.size, grid);
            }
        }
    }

    public void updateGrid(){
        // delete old grid
        GameObject curGrid_left = GameObject.Find("gridLeft");
        GameObject curGrid_right = GameObject.Find("gridRight");
        Destroy(curGrid_left);
        Destroy(curGrid_right);

        // make new grid
        GameObject newGrid_left = new GameObject("gridLeft");
        GameObject newGrid_right = new GameObject("gridRight");
        newGrid_left.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, -10);
        newGrid_right.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 10);

        GenerateGrid(leftGrid, newGrid_left);
        GenerateGrid(rightGrid, newGrid_right);

        // SpawnSingleTile(0, 0, leftGrid.size, newGrid);
    }

    public void AdjustSizeLeft(float newSize){
        slider.size_left = newSize;
    }

    public void AdjustSpacingLeft(float newSpacing){
        slider.spacing_left = newSpacing;
    }

    public void AdjustSizeRight(float newSize){
        slider.size_right = newSize;
    }

    public void AdjustSpacingRight(float newSpacing){
        slider.spacing_right = newSpacing;
    }

    private void updateGridParameters(){
        leftGrid.size = slider.size_left;
        leftGrid.spacing = slider.spacing_left;

        rightGrid.size = slider.size_right;
        rightGrid.spacing = slider.spacing_right;
    }

    public void convertToType(){
        // somehow based on the current grid parameters, categorise into types
        
        // lower size + lower spacing = smoother
        // lower size + higher spacing = ?
        // higher size + lower spacing = ?
        // higher size +  higher spacing = ?
        
        // return higher number = rougher

    }

}
