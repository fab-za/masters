using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManageTraining : MonoBehaviour
{
    public ManageExperiment experiment;
    public DisplayCount display;
    private GameObject visualManager;
    private ManageLineGrid visual;
    private SendTension tension;
    private SendFrequency frequencyManager;
    private int currentTraining;
    private TrialParameters chosen;
    public TrialParameters train1;
    public TrialParameters train2;
    public TrialParameters train3;
    

    void Start()
    {
        visualManager = GameObject.Find("VisualManager"); 
        visual = visualManager.GetComponent<ManageLineGrid>();
        tension = GameObject.Find("VisualManager").GetComponent<SendTension>();        
        frequencyManager = GameObject.Find("VisualManager").GetComponent<SendFrequency>();
        currentTraining = 0;
    }

    // Update is called once per frame
    void Update()
    {
        selectTraining(currentTraining);
        display.counter = currentTraining;
        experiment.allParameters = new List<TrialParameters>(){train1, train2, train3};
        experiment.convertAllRoughness();
        frequencyManager.left_roughness = visual.leftGrid.roughness;
        frequencyManager.right_roughness = visual.rightGrid.roughness;
    }
    public void changeTraining(){
        if(currentTraining < 1){
            currentTraining += 1;
        } else{
            currentTraining = 0;
        }
    }
    public void selectTraining(int cur){
        if(cur == 0){
            chosen = train1;
            } 
        else if(cur == 1){
            chosen = train2;
            }
        else if(cur == 2){
            chosen = train3;
            }
        visual.updateParameters(1, chosen.frequency_left, (-chosen.roughness_left/20), 1, chosen.frequency_right, (-chosen.roughness_right/20));
        amplitudeToTension(chosen);
    }
    public void amplitudeToTension(TrialParameters trial){
        if(trial.amplitude_left == 1){
            tension.currentState.left = "S";
        }
        else if(trial.amplitude_left == 2){
            tension.currentState.left = "T";
        }
        
        if(trial.amplitude_right == 1){
            tension.currentState.right = "S";
        }
        else if(trial.amplitude_right == 2){
            tension.currentState.right = "T";
        }
    }
}
