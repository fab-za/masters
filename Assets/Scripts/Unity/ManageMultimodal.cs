using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManageMultimodal : MonoBehaviour
{
    private ManageExperiment experiment;
    private ManageLineGrid visual;
    private SendTension tension;
    private SendFrequency frequencyManager;
    private ConnectSP sp;
    public DisplayCount display;
    public Text saved;
    public Text indicator;
    public GameObject visualblock;
    public GameObject trainingButton;
    public GameObject selectButtons;

    public TrialParameters trial1;
    public TrialParameters trial2;

    public TrialParameters trial3;
    public TrialParameters trial4;

    public TrialParameters trial5;
    public TrialParameters trial6;

    public TrialParameters trial7;
    public TrialParameters trial8;

    // public TrialParameters trial9;
    // public TrialParameters trial10;
    // public TrialParameters trial11;
    // public TrialParameters trial12;

    // public TrialParameters trial13;
    // public TrialParameters trial14;
    // public TrialParameters trial15;
    // public TrialParameters trial16;
    // public TrialParameters trial17;
    // public TrialParameters trial18;
    // public TrialParameters trial19;
    // public TrialParameters trial20;
    // public TrialParameters trial21;

    private List<TrialParameters> unimodal_haptic;
    private List<TrialParameters> unimodal_visual;
    private List<TrialParameters> multimodal;
    private List<TrialParameters> multimodal_tension;
    private TrialParameters currentTrial;

    public int selectedClass;

    public int current;
    private int cur;
    public int[] order = new int[]{0};
    private int temp;
    private int phase;
    private bool training;
    void Start()
    {
        visual = GameObject.Find("VisualManager").GetComponent<ManageLineGrid>();
        tension = GameObject.Find("VisualManager").GetComponent<SendTension>();        
        frequencyManager = GameObject.Find("VisualManager").GetComponent<SendFrequency>();
        experiment = GameObject.Find("ExperimentManager").GetComponent<ManageExperiment>();
        sp = GameObject.Find("SerialController").GetComponent<ConnectSP>();
        current = 0;
        phase = -1;

        unimodal_haptic = new List<TrialParameters>(){trial1,trial2};
        unimodal_visual = new List<TrialParameters>(){trial3,trial4};
        multimodal = new List<TrialParameters>(){trial5,trial6};
        multimodal_tension = new List<TrialParameters>(){trial7,trial8};

        Debug.Log("in start");

        changePhase();

        // experiment.experimentType = "Multimodal";
        // experiment.newStart = true;
    }

    // Update is called once per frame
    void Update()
    {
        // unimodal_haptic = new List<TrialParameters>(){trial1,trial2};
        // unimodal_visual = new List<TrialParameters>(){trial3,trial4};
        // multimodal = new List<TrialParameters>(){trial5,trial6};
        // multimodal_tension = new List<TrialParameters>(){trial7,trial8};
        // selectPhase();

        display.counter = cur;
        current = order[cur];
        currentTrial = experiment.allParameters[current];

        visual.updateParameters(1, currentTrial.frequency_left, (-currentTrial.roughness_left/20), 1, currentTrial.frequency_right, (-currentTrial.roughness_right/20));
        amplitudeToTension(currentTrial);
        frequencyManager.left_roughness = (-currentTrial.roughness_left/20);
        frequencyManager.right_roughness = (-currentTrial.roughness_right/20);
    }
    public void selectPhase(){
        if(phase == 1){
            experiment.allParameters = unimodal_haptic;
            indicator.text = "Haptic";
            visualblock.SetActive(true);
            sp.started = true;
            training = true;
        }
        else if(phase == 0){
            experiment.allParameters = unimodal_visual;
            indicator.text = "Visual";
            visualblock.SetActive(false);
            sp.started = false;
            training = true;
        }
        else if(phase == 2){
            experiment.allParameters = multimodal;
            indicator.text = "Multi 1";
            visualblock.SetActive(false);
            sp.started = true;
            training = false;
        }
        else if(phase == 3){
            experiment.allParameters = multimodal_tension;
            indicator.text = "Multi 2";
            visualblock.SetActive(false);
            sp.started = true;
            training = false;
        }
    }
    public void changePhase(){
        if(phase < 3){
            phase += 1;
        } else{
            experiment.finished = true;
            phase = 0;
        }
        Debug.Log("phase: "+phase);

        selectPhase();

        if(!training){
            order = new int[]{0,1,0,1,0,1};
            
            experiment.finishedUI.SetActive(false);
            trainingButton.SetActive(false);
            selectButtons.SetActive(true);

            Shuffle();
        }
        else{
            order = new int[]{0,1};
            
            saved.text = "";
            indicator.text = indicator.text + "Training";
            trainingButton.SetActive(true);
            selectButtons.SetActive(false);
        }
    }
    public void endTraining(){
        training = false;
    }

    public void changeVisual(){
        if(cur < (order.Length-1)){
            if(!training){
                saveCurrentMultimodal(current);
                saved.text = "Saved for Task: " + (cur+1);
            }
            cur += 1;
        } else{
            if(!training){
                experiment.finishedUI.SetActive(true);
                changePhase();
            }
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
    public void chooseLeft(){
        selectedClass = 1;
    }
    public void chooseRight(){
        selectedClass = 2;
    }
    public void saveCurrentMultimodal(int experimentIndex){
        MultimodalDataStruct multimodalFrame = new MultimodalDataStruct(
            experimentIndex, // index of experiment actually
            experiment.index, // participant index (my naming sucks sorry @ self)

            currentTrial.frequency_left,
            currentTrial.roughness_left,
            currentTrial.amplitude_left,

            selectedClass            
        );

        experiment.saveMultimodal(multimodalFrame);
    }
}
