using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageGrid : MonoBehaviour
{
    public Sprite sprite;
    public float[,] grid;
    int vertical, horizontal, columns, rows;
    public float sizeFactor;
    void Start()
    {
        vertical = (int) Camera.main.orthographicSize;
        horizontal = vertical * (Screen.width / Screen.height);
        Debug.Log(vertical+", "+horizontal);
        columns = horizontal * (int)(10/sizeFactor);
        rows = vertical * (int)(10/sizeFactor);
        grid = new float[columns, rows];

        generateGrid();
    }

    void Update()
    {
        
    }

    private void generateGrid(){
        for (int i = 0; i < columns; i++){
            for (int j = 0; j < rows; j++){
                grid[i,j] = Random.Range(0.0f, 1.0f);
                SpawnTile(i,j,grid[i,j]);
            }
        }

        this.gameObject.transform.localScale = new Vector3(sizeFactor, sizeFactor, 1.0f);
    }

    private void SpawnTile(int x, int y, float value){
        GameObject g = new GameObject("x: "+x+"y: "+y);
        g.transform.position = new Vector3(x-(horizontal-0.5f), y-(vertical-0.5f));
        g.transform.parent = this.gameObject.transform;
        var s = g.AddComponent<SpriteRenderer>();
        s.sprite = sprite;
        s.color = new Color(value, value, value);
    }
}
