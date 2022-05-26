using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    public int position;
    public float boundary_left;
    public float boundary_right;
    public Vector3 mousePosition;
    public bool moveFinger;
    void Start()
    {
        moveFinger = false; 
    }

    // Update is called once per frame
    void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = Camera.main.transform.position.z + Camera.main.nearClipPlane;
        if(moveFinger){
            transform.position = new Vector3(mousePosition.x, transform.position.y, mousePosition.z);
        }
        findPosition();
    }

    public void findPosition(){
        if(transform.position.x < boundary_left){
            position = 0;
            // Debug.Log("left");
        }
        else if(transform.position.x > boundary_right){
            position = 2;
            // Debug.Log("right");
        }
        else{
            position = 1;
            // Debug.Log("boundary");
        }
    }

    public void toggleFinger(){
        if(moveFinger){
            moveFinger = false;
        }
        else{
            moveFinger = true;
        }
    }
}
