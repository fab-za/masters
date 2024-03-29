﻿using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;

public class ConnectSP : MonoBehaviour
{
    private SerialController serialController;
    public string receivedString;
    public float speed = 0.01f;
    public string tensionModes;
    public string vibrationModes;
    private int timeout = 2;
    private int elapsedFrames;
    public bool started;
    public string longDummy;
    void Start()
    {
        serialController = GameObject.Find("SerialController").GetComponent<SerialController>();
        Application.targetFrameRate = 25;

        tensionModes = "SS";
        vibrationModes = "AA";

        combineMessages();
        
    }

    // Update is called once per frame
    void Update()
    {
        // readSP();

        // Debug.Log(Time.realtimeSinceStartup);

        if(started){
            if(elapsedFrames > timeout){
            combineMessages();
            elapsedFrames = 0;
            }

            elapsedFrames += 1;
        }
    }

    public void readSP(){
        receivedString = serialController.ReadSerialMessage();
        Debug.Log(receivedString);
    }

    public void writeSP(string message){
        // Debug.Log(message);
        serialController.SendSerialMessage(message);
    }

    public void testSP(){
        // sp.Write("test");
        serialController.SendSerialMessage("TEST");
        Debug.Log("sent test");
        
    }

    public void sendLongMessage(){
        Debug.Log("sending loop of: " + longDummy);
        for(int i=0; i<100; i++){
            serialController.SendSerialMessage(longDummy);
        }
    }
    public void combineMessages(){
        string message = tensionModes + vibrationModes;
        serialController.SendSerialMessage(message);
        // Debug.Log("sending: "+message);
        // readSP();
    }
}
