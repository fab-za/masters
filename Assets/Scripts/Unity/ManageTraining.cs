using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageTraining : MonoBehaviour
{
    public ManageExperiment experiment;
    private GameObject visualManager;
    private ManageLineGrid visual;
    private int currentTraining;

    void Start()
    {
        visualManager = GameObject.Find("VisualManager"); 
        visual = visualManager.GetComponent<ManageLineGrid>();
        currentTraining = 1;
    }

    // Update is called once per frame
    void Update()
    {
        selectTraining(currentTraining);
    }
    public void changeTraining(){
        if(currentTraining < 3){
            currentTraining += 1;
        } else{
            currentTraining = 1;
        }
    }
    public void selectTraining(int cur){
        if(cur == 1){
            visual.updateParameters(experiment.train1.amplitude_left, experiment.train1.frequency_left, experiment.train1.roughness_left, experiment.train1.amplitude_right, experiment.train1.frequency_right, experiment.train1.roughness_right);
        } 
        else if(cur == 2){
            visual.updateParameters(experiment.train2.amplitude_left, experiment.train2.frequency_left, experiment.train2.roughness_left, experiment.train2.amplitude_right, experiment.train2.frequency_right, experiment.train2.roughness_right);
        }
        else if(cur == 3){
            visual.updateParameters(experiment.train3.amplitude_left, experiment.train3.frequency_left, experiment.train3.roughness_left, experiment.train3.amplitude_right, experiment.train3.frequency_right, experiment.train3.roughness_right);
        }
    }
}
