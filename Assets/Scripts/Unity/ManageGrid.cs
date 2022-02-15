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
        Debug.Log(vertical+", "+horizontal);
        columns = horizontal * (int)(3/sizeFactor);
        rows = vertical * (int)(2/sizeFactor);
        grid = new float[columns, rows];

        // generateWhiteGrid();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.N)){
            updateColourPerDensity(density);
        }
        
    }

    // private void generateRandomGrid(){
    //     for (int i = 0; i < columns; i++){
    //         for (int j = 0; j < rows; j++){
    //             grid[i,j] = Random.Range(0.0f, 1.0f);
    //             SpawnTile(i,j,grid[i,j]);
    //         }
    //     }

    //     this.gameObject.transform.localScale = new Vector3(sizeFactor, sizeFactor, 1.0f);
    // }

    // private void generateWhiteGrid(){
    //     for (int i = 0; i < columns; i++){
    //         for (int j = 0; j < rows; j++){
    //             SpawnTile(i,j,255);
    //         }
    //     }

    //     this.gameObject.transform.localScale = new Vector3(sizeFactor, sizeFactor, 1.0f);
    // }

    // private void generatePerDensity(int skip){
    //     for (int i = 0; i < columns; i+=2){
    //         for (int j = 0; j < rows; j++){
    //             if ((j + 1) % (skip + 1) == 0) {
    //                 SpawnTile(i,j,0);             
    //             }
    //             else {
    //                 SpawnTile(i,j,1);  
    //             }
    //         }
    //     }

    //     this.gameObject.transform.localScale = new Vector3(sizeFactor, sizeFactor, 1.0f);
    // }

    // private void updateColourPerDensity(int skip){
    //     for (int j = 0; j < rows; j+=2){
    //         for (int i = 0; i < columns; i++){
    //             GameObject cur = GameObject.Find("x: "+i+"y: "+j);

    //             if ((i + 1) % (skip + 1) == 0) {
    //                 cur.GetComponent<SpriteRenderer>().color = new Color(0,0,0);
    //             }
    //             else {
    //                 cur.GetComponent<SpriteRenderer>().color = new Color(255,255,255);
    //             }
    //         }
    //     }
    // }

    private void updateColourPerDensity(int skip){
        GameObject curGrid = GameObject.Find("grid");
        Destroy(curGrid);

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
