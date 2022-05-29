using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManagePrelim : MonoBehaviour
{
    private ManageExperiment experiment;
    private ManageLineGrid visual;
    private SendTension tension;
    private SendFrequency frequencyManager;
    public ManageSlider slider;    
    public DisplayCount display;
    public Text saved;
    public TrialParameters prelim1;
    public TrialParameters prelim2;
    public TrialParameters prelim3;
    public TrialParameters prelim4;
    public TrialParameters prelim5;
    public TrialParameters prelim6;
    public TrialParameters prelim7;
    public TrialParameters prelim8;
    public TrialParameters prelim9;
    public int current;
    private int cur;
    public int[] order = new int[]{0,1,2,3,4,5};
    private int temp;
    
    void Start()
    {
        visual = GameObject.Find("VisualManager").GetComponent<ManageLineGrid>();
        slider = GameObject.Find("VisualManager").GetComponent<ManageSlider>();
        tension = GameObject.Find("VisualManager").GetComponent<SendTension>();
        frequencyManager = GameObject.Find("VisualManager").GetComponent<SendFrequency>();
        experiment = GameObject.Find("ExperimentManager").GetComponent<ManageExperiment>();
        current = 0;

        // experiment.train1 = prelim1;
        // experiment.train2 = prelim2;
        // experiment.train3 = prelim3;
        // experiment.trial1 = prelim4;
        // experiment.trial2 = prelim5;
        // experiment.trial3 = prelim6;
        // experiment.trial4 = prelim7;
        // experiment.trial5 = prelim8;
        // experiment.trial6 = prelim9;

        experiment.experimentType = "frequencies";
        experiment.newStart = true;

        Shuffle();

        // Debug.Log(experiment.allParameters.Count);
    }

    // Update is called once per frame
    void Update()
    {
        // experiment.train1 = prelim1;
        // experiment.train2 = prelim2;
        // experiment.train3 = prelim3;
        // experiment.trial1 = prelim4;
        // experiment.trial2 = prelim5;
        // experiment.trial3 = prelim6;
        // experiment.trial4 = prelim7;
        // experiment.trial5 = prelim8;
        // experiment.trial6 = prelim9;

        experiment.convertAllRoughness();

        experiment.allParameters = new List<TrialParameters>(){prelim1, prelim2, prelim3, prelim4, prelim5, prelim6, prelim7, prelim8, prelim9};

        display.counter = cur;
        current = order[cur];

        slider.tempLeftRoughness = Mathf.Abs(experiment.allParameters[current].roughness_left)/20;
        slider.tempRightRoughness = Mathf.Abs(experiment.allParameters[current].roughness_right)/20;

        amplitudeToTension();
        frequencyManager.left_roughness = visual.leftGrid.roughness;
        frequencyManager.right_roughness = visual.rightGrid.roughness;

        updateCurrentDataFrame();

    }

    public void changeVisual(){
        slider.saveLeft();
        slider.saveRight();
        slider.saveComparisonFile(current);
        saved.text = "Saved for Task: " + (cur+1);

        if(cur < 5){
            cur += 1;
        } else{
            experiment.finished = true;
            cur = 0;
        }
    }

    public void Shuffle() 
    {
        for (int i = 0; i < order.Length - 1; i++) 
        {
            int rnd = Random.Range(i, order.Length);
            temp = order[rnd];
            order[rnd] = order[i];
            order[i] = temp;
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
    public void updateCurrentDataFrame(){
        experiment.currentData_left.trialFrequency = experiment.allParameters[current].frequency_left;
        experiment.currentData_left.trialAmplitude = experiment.allParameters[current].amplitude_left;
        experiment.currentData_left.trialRoughness = experiment.allParameters[current].roughness_left;

        experiment.currentData_right.trialFrequency = experiment.allParameters[current].frequency_right;
        experiment.currentData_right.trialAmplitude = experiment.allParameters[current].amplitude_right;
        experiment.currentData_right.trialRoughness = experiment.allParameters[current].roughness_right;

    }
}
