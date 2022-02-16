using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibrateOnSquare : MonoBehaviour
{
    private ConnectSP sp;
    void Start()
    {
        sp = GameObject.Find("SPManager").GetComponent<ConnectSP>();
    }

    void OnMouseEnter(){
        sp.writeSP("A");
    }
}
