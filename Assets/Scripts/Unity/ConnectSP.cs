using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;

public class ConnectSP : MonoBehaviour
{
    SerialPort sp = new SerialPort("COM3", 9600);
    // private SerialController serialController;
    public string receivedString;
    public GameObject test_data;
    public float speed = 0.01f;
    public string tensionModes;
    public string vibrationModes;
    void Start()
    {
        sp.Open();
        // serialController = GameObject.Find("SerialController").GetComponent<SerialController>();
    }

    // Update is called once per frame
    void Update()
    {
        // readSP();
        // Debug.Log(receivedString);
        combineMessages();
    }

    public void readSP(){
        receivedString = sp.ReadLine();
        // receivedString = serialController.ReadSerialMessage();
    }

    public void writeSP(string message){
        // Debug.Log(message);
        sp.Write(message);
    }

    public void testSP(){
        sp.Write("test");
        // serialController.SendSerialMessage("test");
    }
    public void combineMessages(){
        string message = tensionModes + vibrationModes;
        sp.Write(message);
        // Debug.Log(message);
    }
}
