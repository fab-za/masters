using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageExperiment : MonoBehaviour
{
    public ManageSlider slider;
    public ManageTraining training;
    public bool trainingMode;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(trainingMode){
            training.enabled = true;
            slider.enabled = false;
        } else{
            training.enabled = false;
            slider.enabled = true;
        }
    }
    void changeModes(){
        if(trainingMode){
            trainingMode = false;
        } else{
            trainingMode = true;
        }
    }
}
