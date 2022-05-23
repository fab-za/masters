using System.Collections;
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
    private int timeout = 20;
    private int elapsedFrames;
    void Start()
    {
        serialController = GameObject.Find("SerialController").GetComponent<SerialController>();
    }

    // Update is called once per frame
    void Update()
    {
        // readSP();
        if(elapsedFrames > timeout){
            combineMessages();
            elapsedFrames = 0;
        }

        elapsedFrames += 1;
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
        serialController.SendSerialMessage("test");
        Debug.Log("sent test");
        
    }
    public void combineMessages(){
        string message = tensionModes + vibrationModes;
        serialController.SendSerialMessage(message);
        // Debug.Log("sending: "+message);
    }
}
