using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;

public class ConnectSP : MonoBehaviour
{
    SerialPort sp = new SerialPort("COM3", 9600);
    public string receivedString;
    public GameObject test_data;
    public float speed = 0.01f;
    void Start()
    {
        sp.Open();
    }

    // Update is called once per frame
    void Update()
    {
        // readSP();
        // Debug.Log(receivedString);
    }

    public void readSP(){
        receivedString = sp.ReadLine();
    }

    public void writeSP(string message){
        Debug.Log(message);
        sp.Write(message);
    }
}
