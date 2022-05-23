using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendFrequency : MonoBehaviour
{
    // private ManageGrid g;
    // private ManageGridSlider visual;
    private ManageLineGrid visual;
    private ConnectSP sp;
    private string[] alphabet = new string[]{"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "X", "Y", "Z"};
    private Vector3 prevPosition;
    public FollowMouse mouse;
    public string message;
    void Start()
    {
        sp = GameObject.Find("SerialController").GetComponent<ConnectSP>();
        visual = this.gameObject.GetComponent<ManageLineGrid>();
        prevPosition = mouse.mousePosition;
    }

    // Update is called once per frame
    void Update()
    {
        if(mouse.mousePosition != prevPosition){
            if(mouse.position == 1){
                message = alphabet[(int)visual.leftGrid.roughness+1] + alphabet[(int)visual.rightGrid.roughness+1];
            } 
            else if (mouse.position == 0){
                message = alphabet[(int)visual.leftGrid.roughness+1] + alphabet[(int)visual.leftGrid.roughness+1];
            } 
            else if (mouse.position == 2){
                message = alphabet[(int)visual.rightGrid.roughness+1] + alphabet[(int)visual.rightGrid.roughness+1];
            }

            // Debug.Log("message: " + message);
            sp.vibrationModes = message;
        } 
        else{sp.vibrationModes = "AA";}

        prevPosition = mouse.mousePosition;
    }

}
