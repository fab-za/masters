using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageTraining : MonoBehaviour
{
    private GameObject visualManager;
    private ManageLineGrid visual;
    private int currentTraining;
    void Start()
    {
        visualManager = GameObject.Find("VisualManager"); 
        visual = visualManager.GetComponent<ManageLineGrid>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void changeTraining(){
        currentTraining += 1;
    }
    void defineTraining(int currentTraining){
        if(currentTraining == 1){
            // left and right visuals etc
            // visual.updateParameters();
        } 
        else if(currentTraining == 2){
            // left and right visuals etc
        }
        else if(currentTraining == 3){
            // left and right visuals etc
        }
    }
}
