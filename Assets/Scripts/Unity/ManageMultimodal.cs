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
    public GameObject visualBlock;
    public GameObject trainingButton;
    public GameObject selectButtons;
    public GameObject breakMessage;

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
    public TrialParameters currentTrial;

    public int selectedClass;

    public int current;
    private int cur;
    public int[] order = new int[]{0};
    private int temp;
    private int phase;
    private bool training;

    private int frames;
    private int popupduration;
    private bool phase_complete;
    private bool task_complete;

    private float choiceMade;
    private float lastChoiceMade;
    private float responseTime;

    void Start()
    {
        visual = GameObject.Find("VisualManager").GetComponent<ManageLineGrid>();
        tension = GameObject.Find("VisualManager").GetComponent<SendTension>();        
        frequencyManager = GameObject.Find("VisualManager").GetComponent<SendFrequency>();
        experiment = GameObject.Find("ExperimentManager").GetComponent<ManageExperiment>();
        sp = GameObject.Find("SerialController").GetComponent<ConnectSP>();

        current = 0;
        phase = -1;
        phase_complete = false;
        task_complete = false;
        popupduration = 30;
        frames = 0;

        unimodal_haptic = new List<TrialParameters>(){trial3,trial4};
        unimodal_visual = new List<TrialParameters>(){trial1,trial2};
        multimodal = new List<TrialParameters>(){trial5,trial6};
        multimodal_tension = new List<TrialParameters>(){trial7,trial8};

        // Debug.Log("in start");

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

        popupFinish();
        if(phase != 1){
            popupBlock();
        }      

        if(!training){
            display.counter = cur;
        }  
        else{
            display.T.text = "PATTERN #" + (cur+1);
        }
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
            visualBlock.SetActive(true);
            sp.started = true;
            training = true;
        }
        else if(phase == 0){
            experiment.allParameters = unimodal_visual;
            indicator.text = "Visual";
            visualBlock.SetActive(false);
            training = true;
        }
        else if(phase == 2){
            experiment.allParameters = multimodal;
            indicator.text = "Multi 1";
            visualBlock.SetActive(false);
            training = false;
        }
        else if(phase == 3){
            experiment.allParameters = multimodal_tension;
            indicator.text = "Multi 2";
            visualBlock.SetActive(false);
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
        // Debug.Log("phase: "+phase);

        selectPhase();

        checkTraining();
    }
    public void checkTraining(){
        if(!training){
            order = new int[]{0,1,0,1,0,1};

            trainingButton.SetActive(false);
            selectButtons.SetActive(true);

            Shuffle();
        }
        else{
            order = new int[]{0,1};
            
            saved.text = "";
            indicator.text = indicator.text + " Training";
            trainingButton.SetActive(true);
            selectButtons.SetActive(false);
        }
    }
    public void endTraining(){
        training = false;
        cur = 0;
        task_complete = true;
        checkTraining();
        lastChoiceMade = Time.time;
    }

    public void changeVisual(){
        task_complete = true;
        if(cur < (order.Length-1)){
            if(!training){
                saved.text = "Saved for Task: " + (cur+1);
            }
            cur += 1;
        } else{
            if(!training){
                phase_complete = true;
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
        selectedClass = 0;
        choiceMade = Time.time;
        calculateResponseTime();
        saveCurrentMultimodal(current);
    }
    public void chooseRight(){
        selectedClass = 1;
        choiceMade = Time.time;
        calculateResponseTime();
        saveCurrentMultimodal(current);
    }
    public void saveCurrentMultimodal(int experimentIndex){
        MultimodalDataStruct multimodalFrame = new MultimodalDataStruct(
            phase,
            experimentIndex, // index of experiment actually
            experiment.index, // participant index (my naming sucks sorry @ self)

            currentTrial.frequency_left,
            currentTrial.roughness_left,
            currentTrial.amplitude_left,

            selectedClass,
            responseTime           
        );

        experiment.saveMultimodal(multimodalFrame);
    }
    public void popupFinish(){
        if(phase_complete){
            if(frames < popupduration){
                breakMessage.SetActive(true);
                frames += 1;
            }
            else{
                breakMessage.SetActive(false);
                frames = 0;
                phase_complete = false;
            }
        }
    }
    public void popupBlock(){
        if(task_complete){
            if(frames < (popupduration/4)){
                visualBlock.SetActive(true);
                frames += 1;
            }
            else{
                visualBlock.SetActive(false);
                frames = 0;
                task_complete = false;
            }
        }
    }
    public void calculateResponseTime(){
        responseTime = choiceMade - lastChoiceMade;
        lastChoiceMade = choiceMade;
    }
}
