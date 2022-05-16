using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TensionMotorManager : MonoBehaviour
{
    private ConnectSP sp;
    public GameObject trackedObject;
    private Vector3 prevPosition;
    void Start()
    {
        sp = GameObject.Find("SPManager").GetComponent<ConnectSP>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
