using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageGrid : MonoBehaviour
{
    public Sprite sprite;
    public float[,] grid;
    int vertical, horizontal, columns, rows;
    public float sizeFactor;
    public int density;
    void Start()
    {
        vertical = (int) Camera.main.orthographicSize;
        horizontal = vertical * (Screen.width / Screen.height);
        columns = horizontal * (int)(3/sizeFactor);
        rows = vertical * (int)(2/sizeFactor);
        grid = new float[columns, rows];
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.N)){
            updateColourPerDensity(density);
        }
        
    }

    private void updateColourPerDensity(int skip){
        // delete old grid
        GameObject curGrid = GameObject.Find("grid");
        Destroy(curGrid);

        // make new grid
        GameObject newGrid = new GameObject("grid");
        newGrid.transform.position = this.gameObject.transform.position;

        for (int j = 0; j < rows; j+=skip){
            for (int i = 0; i < columns; i+=skip){
                SpawnTile(i,j,0, newGrid);
            }
        }

        newGrid.transform.localScale = new Vector3(sizeFactor, sizeFactor, 1.0f);
    }

    private void SpawnTile(int x, int y, float value, GameObject parentGrid){
        GameObject g = new GameObject("x: "+x+"y: "+y);
        g.transform.position = new Vector3(x-(horizontal-0.5f), y-(vertical-0.5f));
        g.transform.parent = parentGrid.transform;
        var s = g.AddComponent<SpriteRenderer>();
        s.sprite = sprite;
        s.color = new Color(value, value, value);
    }
}
