using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagePrelim : MonoBehaviour
{
    private ManageExperiment experiment;
    private ManageLineGrid visual;
    public ManageSlider slider;
    public TrialParameters prelim1;
    public TrialParameters prelim2;
    public TrialParameters prelim3;
    public TrialParameters prelim4;
    public TrialParameters prelim5;
    public TrialParameters prelim6;
    private int current;
    private SendTension tension;
    void Start()
    {
        visual = GameObject.Find("VisualManager").GetComponent<ManageLineGrid>();
        slider = GameObject.Find("VisualManager").GetComponent<ManageSlider>();
        tension = GameObject.Find("VisualManager").GetComponent<SendTension>();
        experiment = GameObject.Find("ExperimentManager").GetComponent<ManageExperiment>();
        current = 0;
    }

    // Update is called once per frame
    void Update()
    {
        experiment.train1 = prelim1;
        experiment.train2 = prelim2;
        experiment.train3 = prelim3;
        experiment.trial1 = prelim4;
        experiment.trial2 = prelim5;
        experiment.trial3 = prelim6;

        experiment.convertAllRoughness();

        // visual.updateParameters(experiment.allParameters[current].amplitude_left, experiment.allParameters[current].frequency_left, experiment.allParameters[current].roughness_left, experiment.allParameters[current].amplitude_right, experiment.allParameters[current].frequency_right, experiment.allParameters[current].roughness_right);
        slider.tempLeftRoughness = Mathf.Abs(experiment.allParameters[current].roughness_left)/20;
        slider.tempRightRoughness = Mathf.Abs(experiment.allParameters[current].roughness_right)/20;

        amplitudeToTension();

    }

    public void changeVisual(){
        if(current < 2){
            current += 1;
        } else{
            current = 0;
        }
    }

    public void amplitudeToTension(){
        if(experiment.allParameters[current].amplitude_left == 1){
            tension.currentState.left = "S";
        }
        else if(experiment.allParameters[current].amplitude_left == 2){
            tension.currentState.left = "T";
        }
        
        if(experiment.allParameters[current].amplitude_right == 1){
            tension.currentState.right = "S";
        }
        else if(experiment.allParameters[current].amplitude_right == 2){
            tension.currentState.right = "T";
        }
    }
}
