﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendFrequency : MonoBehaviour
{
    // private ManageGrid g;
    // private ManageGridSlider visual;
    // private ManageLineGrid visual;
    private ConnectSP sp;
    public string[] alphabet;
    private Vector3 prevPosition;
    public FollowMouse mouse;
    public string message;
    public float left_roughness;
    public float right_roughness;
    void Start()
    {
        sp = GameObject.Find("SerialController").GetComponent<ConnectSP>();
        // visual = this.gameObject.GetComponent<ManageLineGrid>();
        prevPosition = mouse.mousePosition;
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(Input.GetAxis("Mouse X"));
        if(mouse.moveFinger){
            if(Input.GetAxis("Mouse X") == 0){
                // Debug.Log("stationary");
                sp.vibrationModes = "AA";
            } 
            else{
                if(mouse.position == 1){
                    message = alphabet[(int)left_roughness+1] + alphabet[(int)right_roughness+1];
                } 
                else if (mouse.position == 0){
                    message = alphabet[(int)left_roughness+1] + alphabet[(int)left_roughness+1];
                } 
                else if (mouse.position == 2){
                    message = alphabet[(int)right_roughness+1] + alphabet[(int)right_roughness+1];
                }

                // Debug.Log("message: " + message);
                sp.vibrationModes = message;
            }

            // Debug.Log(sp.vibrationModes);

        }
        else{sp.vibrationModes = "AA";}
        
        // prevPosition = mouse.mousePosition;
    }

}
