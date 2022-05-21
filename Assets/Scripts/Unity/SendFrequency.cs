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
    public GameObject trackedObject;
    private Vector3 prevPosition;
    void Start()
    {
        sp = GameObject.Find("SPManager").GetComponent<ConnectSP>();
        // g = this.gameObject.GetComponent<ManageGrid>();
        visual = this.gameObject.GetComponent<ManageLineGrid>();
        prevPosition = trackedObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(trackedObject.transform.position != prevPosition){
            // Debug.Log("cursor is moving: "+ g.density);
            // sp.writeSP(alphabet[g.curPair]);

            sp.vibrationModes = alphabet[visual.leftGrid.roughness+1] + alphabet[visual.rightGrid.roughness+1];
        } else{sp.vibrationModes = "AA";}

        prevPosition = trackedObject.transform.position;
    }

}
