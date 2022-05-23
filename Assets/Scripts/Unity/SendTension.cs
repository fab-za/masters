using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendTension : MonoBehaviour
{
    // private ManageGridSlider visual;
    private ManageLineGrid visual;
    private ConnectSP sp;
    public FollowMouse mouse;
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
    public float motorLag;
    public bool sendMessage;
    private int prevPosition;
    void Start()
    {
        sp = GameObject.Find("SerialController").GetComponent<ConnectSP>();
        visual = this.gameObject.GetComponent<ManageLineGrid>();

        rougher_left = new Relation("T","S");
        rougher_right = new Relation("S","T");
        rougher_equal = new Relation("S","S");
        
        prevPosition = 0;   
    }

    // Update is called once per frame
    void Update()
    {
        // write message
        // Debug.Log(Input.GetAxis("Mouse X"));
        selectRelationState();

        if(mouse.position != prevPosition){

            if(mouse.position == 1){
                message = currentState.left + currentState.right;
            } 
            else if (mouse.position == 0){
                message = currentState.left + currentState.left;
            } 
            else if (mouse.position == 2){
                message = currentState.right + currentState.right;
            }

            // Debug.Log("message: " + message);

            // sp.writeSP(message);

            sp.tensionModes = message;
        }

        prevPosition = mouse.position;
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
}
