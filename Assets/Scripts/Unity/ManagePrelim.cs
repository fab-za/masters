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
    void Start()
    {
        visual = GameObject.Find("VisualManager").GetComponent<ManageLineGrid>();
        slider = GameObject.Find("VisualManager").GetComponent<ManageSlider>();
        experiment = GameObject.Find("ExperimentManager").GetComponent<ManageExperiment>();
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
        slider.tempLeftRoughness = Mathf.Abs(experiment.allParameters[current].roughness_left) - 249;
        slider.tempRightRoughness = Mathf.Abs(experiment.allParameters[current].roughness_right) - 249;
    }

    public void changeVisual(){
        if(current < 5){
            current += 1;
        } else{
            current = 0;
        }
    }

}
