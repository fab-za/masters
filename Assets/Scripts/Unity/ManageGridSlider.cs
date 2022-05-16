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
    public struct GridParameters{
        public string type;
        public int panel;    // left = -1, right = +1
        public int size;
        public int spacing;

        public GridParameters(string t, int p, int si, int sp){
            type = t;
            panel = p;
            size = si;
            spacing = sp;
        }
    }

    struct SliderValues{
        public int size_left;
        public int size_right;
        public int spacing_left;
        public int spacing_right;

        public SliderValues(int sil, int sir, int spl, int spr){
            size_left = sil;
            size_right = sir;
            spacing_left = spl;
            spacing_right = spr;
        }
    }

    private GridParameters leftGrid;
    private GridParameters rightGrid;
    private SliderValues slider;
    // public int trial = 0;

    void Start()
    {
        vertical = (int) Camera.main.orthographicSize;
        horizontal = vertical * (Screen.width / Screen.height);
        columns = horizontal * (int)(3.5f/sizeFactor);
        rows = vertical * (int)(2.2f/sizeFactor);
        // grid = new float[columns, rows];

        leftGrid = new GridParameters("A",-1,0,0);
        rightGrid = new GridParameters("A",1,0,0);
    }

    // Update is called once per frame
    void Update()
    {
        updateGridParameters();
    }

    private void SpawnSingleTile(float x, float y, int size, GameObject parentGrid){
        GameObject g = new GameObject("x: "+x+" y: "+y);
        g.AddComponent<BoxCollider2D>();
        var s = g.AddComponent<SpriteRenderer>();
        s.sprite = sprite;

        g.transform.position = new Vector3(x,y,0);
        g.transform.parent = parentGrid.transform;
        g.transform.localScale += new Vector3(size, size, 0);
        
    }

    private void GenerateGrid(GridParameters parameters){
        // delete old grid
        GameObject curGrid = GameObject.Find("grid");
        if(curGrid != null){
            Destroy(curGrid);
        }

        // make new grid
        GameObject newGrid = new GameObject("grid");
        newGrid.transform.position = this.gameObject.transform.position;

        int total_spacing = parameters.size + (parameters.spacing * parameters.panel);

        for (int j = (int)this.gameObject.transform.position.y; j < rows; j+= total_spacing){
            for (int i = (int)this.gameObject.transform.position.x; i < columns; i+=total_spacing){
                SpawnSingleTile(i,j, parameters.size, newGrid);
            }
        }
    }

    public void updateGrid(){
        GenerateGrid(leftGrid);
        GenerateGrid(rightGrid);
    }

    public void AdjustSizeLeft(int newSize){
        slider.size_left = newSize;
    }

    public void AdjustSpacingLeft(int newSpacing){
        slider.spacing_left = newSpacing;
    }

    public void AdjustSizeRight(int newSize){
        slider.size_right = newSize;
    }

    public void AdjustSpacingRight(int newSpacing){
        slider.spacing_right = newSpacing;
    }

    private void updateGridParameters(){
        leftGrid.size = slider.size_left;
        leftGrid.spacing = slider.spacing_left;

        rightGrid.size = slider.size_right;
        rightGrid.spacing = slider.spacing_right;
    }

}
