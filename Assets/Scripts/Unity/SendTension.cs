using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendTension : MonoBehaviour
{
    // private ManageGridSlider visual;
    private ManageLineGrid visual;
    private ConnectSP sp;
    public GameObject trackedObject;
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
    private int position;
    private int prevPosition;
    public float boundary_left;
    public float boundary_right;
    public float motorLag;
    public bool sendMessage;
    void Start()
    {
        sp = GameObject.Find("SerialController").GetComponent<ConnectSP>();
        visual = this.gameObject.GetComponent<ManageLineGrid>();
        prevPosition = 0;

        rougher_left = new Relation("T","S");
        rougher_right = new Relation("S","T");
        rougher_equal = new Relation("S","S");
    }

    // Update is called once per frame
    void Update()
    {
        // write message
        // Debug.Log(Input.GetAxis("Mouse X"));
        selectRelationState();
        // defineBoundary();
        findPosition();

        if(position != prevPosition){
            // Debug.Log("cursor is moving");

            if(position == 1){
                message = currentState.left + currentState.right;
            } 
            else if (position == 0){
                message = currentState.left + currentState.left;
            } 
            else if (position == 2){
                message = currentState.right + currentState.right;
            }

            // Debug.Log("message: " + message);

            // sp.writeSP(message);

            sp.tensionModes = message;
        }

        prevPosition = position;
    }

    public void selectRelationState(){
        if(visual.leftGrid.roughness > visual.rightGrid.roughness){
            currentState = rougher_left;
        } 
        else if(visual.leftGrid.roughness < visual.rightGrid.roughness){
            currentState = rougher_right;
        }
        else{
            currentState = rougher_equal; 
        }
    }

    public void findPosition(){
        if(trackedObject.transform.position.x < boundary_left){
            position = 0;
            // Debug.Log("left");
        }
        else if(trackedObject.transform.position.x > boundary_right){
            position = 2;
            // Debug.Log("right");
        }
        else{
            position = 1;
            // Debug.Log("boundary");
        }
    }

    public void defineBoundary(){
        float cursorSpeed = Input.GetAxis("Mouse X");
        float offset = cursorSpeed * motorLag;
        // Debug.Log(offset);
        boundary_left = -1 -offset;
        boundary_right = 1 - offset;
    }
}
