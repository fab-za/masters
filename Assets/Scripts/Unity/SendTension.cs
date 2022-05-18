using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendTension : MonoBehaviour
{
    private ManageGridSlider grid;
    private ConnectSP sp;
    public GameObject trackedObject;
    private Vector3 prevPosition;
    [System.Serializable]
    public struct Relation{
        public string left;
        public string right;
        public Relation(string l, string r){
            left = l;
            right = r;
        }
    }

    private Relation rougher_left;
    private Relation rougher_right;
    private Relation rougher_equal;
    public Relation currentState;
    public string message;
    private bool onLeftPanel;
    private bool onBoundary;
    private bool onRightPanel;
    public float boundary_left;
    public float boundary_right;
    void Start()
    {
        sp = GameObject.Find("SPManager").GetComponent<ConnectSP>();
        grid = this.gameObject.GetComponent<ManageGridSlider>();
        prevPosition = trackedObject.transform.position;

        rougher_left = new Relation("T","S");
        rougher_right = new Relation("S","T");
        rougher_equal = new Relation("S","S");
    }

    // Update is called once per frame
    void Update()
    {
        // write message
        selectRelationState();
        findPosition();

        if(trackedObject.transform.position != prevPosition){
            // Debug.Log("cursor is moving");

            if(onBoundary){
                message = currentState.left + currentState.right;
            } 
            else if (onLeftPanel){
                message = currentState.left + currentState.left;
            } 
            else if (onRightPanel){
                message = currentState.right + currentState.right;
            }

            // Debug.Log("message: " + message);

            sp.writeSP(message);
        }

        prevPosition = trackedObject.transform.position;
    }

    public void selectRelationState(){
        if(grid.leftGrid.roughness > grid.rightGrid.roughness){
            currentState = rougher_left;
        } 
        else if(grid.leftGrid.roughness < grid.rightGrid.roughness){
            currentState = rougher_right;
        }
        else{
            currentState = rougher_equal; 
        }
    }

    public void findPosition(){
        if(trackedObject.transform.position.x < boundary_left){
            onLeftPanel = true;
            onBoundary = false;
            onRightPanel = false;
            // Debug.Log("left");
        }
        else if(trackedObject.transform.position.x > boundary_right){
            onLeftPanel = false;
            onBoundary = false;
            onRightPanel = true;
            // Debug.Log("right");
        }
        else{
            onLeftPanel = false;
            onBoundary = true;
            onRightPanel = false;
            // Debug.Log("boundary");
        }
    }
}
