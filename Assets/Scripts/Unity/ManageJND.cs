using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManageJND : MonoBehaviour
{
    private ManageExperiment experiment;
    private ManageLineForJND visual;
    private SendTension tension;
    private SendFrequency frequencyManager;
    private ConnectSP sp;
    public DisplayCount display;
    public Text saved;
    public Text indicator;
    public GameObject visualBlock;
    public GameObject leftLine;
    public GameObject rightLine;
    public GameObject trainingButton;
    public GameObject selectButtons;
    public GameObject breakMessage;

    public TrialParameters baseline;  // STANDARD
    // public TrialParameters trial2;
    // public TrialParameters trial3;
    // public TrialParameters trial4;
    // public TrialParameters trial5;
    // public TrialParameters trial6;
    // public TrialParameters trial7;
    // public TrialParameters trial8;
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

    private List<TrialParameters> JNDList;
    private List<TrialParameters> possibleTrials;
    private TrialParameters currentTrial;
    private TrialParameters comparingTrial;

    private float percent;

    public int selectedClass;

    private int cur;
    private int rndIndex;
    
    private int phase;
    private bool training;
    private string title;
    private bool noHaptic;

    private int frames;
    private int textFrames;
    private int presentFrames;
    private int popupduration;    
    private int presentDuration;

    private bool phase_complete;
    private bool task_complete;
    private int pairsShown;

    private int correctCount;
    private int totalCount;

    void Start()
    {
        visual = GameObject.Find("VisualManager").GetComponent<ManageLineForJND>();
        tension = GameObject.Find("VisualManager").GetComponent<SendTension>();        
        frequencyManager = GameObject.Find("VisualManager").GetComponent<SendFrequency>();
        experiment = GameObject.Find("ExperimentManager").GetComponent<ManageExperiment>();
        sp = GameObject.Find("SerialController").GetComponent<ConnectSP>();

        percent = 0.03f;
        phase = -1;
        phase_complete = false;
        task_complete = false;
        popupduration = 10;
        frames = 0;
        presentDuration = 100;
        presentFrames = 0;
        pairsShown = 0;

        // JNDList = new List<TrialParameters>(){trial1,trial2,trial3,trial4,trial5,trial6,trial7,trial8,trial9,trial10,trial11,trial12,trial13,trial14,trial15,trial16,trial17,trial18,trial19,trial20};

        initTrialList();
        experiment.allParameters = JNDList;
        

        frequencyManager.alphabet = new string[]{"A", "G", "B", "D", "E", "F", "H", "I", "J", "K", "M", "N", "O", "P", "Q", "R", "U", "V", "W", "X", "Y", "Z"};
    }

    // Update is called once per frame
    void Update()
    {
        popupBlock(task_complete); 
        display.counter = cur; 
        comparingTrial = possibleTrials[rndIndex];

        tension.currentState.left = "T";
        tension.currentState.right = "T"; 

        if(!training){
            trainingButton.SetActive(false);

            if(correctCount > 4){
                changeCurrentTrial();
            }

            while(task_complete){
                popupSaved();
                presentPairs();
                presentPairs();
                checkTrialProgress();
            }

            selectButtons.SetActive(true);
            noHaptic = true;
        }   
        else{
            currentTrial = baseline;

            saved.text = "";
            cur = -1;
            indicator.text = title + "TRAINING (BASELINE) PATTERN";
            trainingButton.SetActive(true);
            selectButtons.SetActive(false);
        }
    }

    public void selectPhase(){
        if(phase == 1){
            title = "Haptic ";
            leftLine.SetActive(false);
            rightLine.SetActive(false);
            training = true;
            noHaptic = false;
        }
        else if(phase == 0){
            title = "Visual ";
            leftLine.SetActive(true);
            rightLine.SetActive(true);
            training = true;
            noHaptic = true;
        }
    }
    public void changePhase(){
        if(phase < 1){
            phase += 1;
        } else{
            experiment.finished = true;
            phase = 0;
        }
        // Debug.Log("phase: "+phase);

        selectPhase();
    }

    public void endTraining(){
        training = false;
        cur = 0;
        task_complete = true;
    }

    public void changeVisual(){
        checkAccuracy(selectedClass);
        task_complete = true;

        if(phase == 1){
            noHaptic = false;
        }

        rndIndex = Random.Range(0,1); 
    }


    public void changeCurrentTrial(){
        saveCurrentJND(cur);

        if(cur < (JNDList.Count-1)){
            cur += 1;
        } else{
            if(!training){
                phase_complete = true;
                changePhase();
            }
            cur = 0;
        }

        currentTrial = experiment.allParameters[cur];     
        possibleTrials = new List<TrialParameters>(){baseline, currentTrial};
    }

    public void popupBlock(bool criteria){
        if(criteria){
            if(frames < (popupduration)){
                indicator.text = title;
                noHaptic = true;
                visualBlock.SetActive(true);
                frames += 1;
            }
            else{
                noHaptic = false;
                visualBlock.SetActive(false);
                frames = 0;
                criteria = false;
            }
        }
    }

    public void popupSaved(){
        if(textFrames < (popupduration)){
            saved.text = "Saved";
            textFrames += 1;
        }
        else{
            saved.text = "";
            textFrames = 0;
        }
    }

    public void initTrialList(){
        for(int i=20; i>0; i--){
            TrialParameters trial = new TrialParameters();

            trial.roughness_left = baseline.roughness_left + i;
            trial.amplitude_left = baseline.amplitude_left;
            trial.frequency_left = baseline.frequency_left * (1+(percent*i));

            trial.roughness_right = baseline.roughness_right + i;
            trial.amplitude_right = baseline.amplitude_right;
            trial.frequency_right = baseline.frequency_right * (1+(percent*i));

            JNDList.Add(trial);
        }
    }

    public void presentTrial(bool criteria, TrialParameters target){
        if(criteria){
            if(presentFrames < presentDuration){
                visual.updateParameters(target.frequency_left, target.frequency_right);

                if(!noHaptic){
                    frequencyManager.left_roughness = target.roughness_left;
                    frequencyManager.right_roughness = target.roughness_right;
                }
            }
            else{
                presentFrames = 0;
                criteria = false;
            }
        }
    }
    public void presentPairs(){
        indicator.text = title + "BASELINE PATTERN";
        presentTrial(task_complete, baseline);
        popupBlock(true);

        indicator.text = title + "COMPARISON PATTERN";
        presentTrial(true, comparingTrial);
        popupBlock(true);
        popupBlock(true);

        pairsShown += 1;
    }

    public void checkTrialProgress(){
        if(pairsShown > 1){
            task_complete = false;
            pairsShown = 0;
        }
    }

    public void chooseEqual(){
        selectedClass = 0;  // EQUAL
    }
    public void chooseRougher(){
        selectedClass = 1;  // ROUGHER
    }

    public void checkAccuracy(int choice){
        if(choice == rndIndex && rndIndex == 1){
            correctCount += 1;
        }
        else{
            totalCount += 1;
        }
    }

    public void saveCurrentJND(int experimentIndex){
        JNDDataStruct JNDFrame = new JNDDataStruct(
            phase,
            experimentIndex, // index of experiment actually, in this case the level of difference
            experiment.index, // participant index (my naming sucks sorry @ self)

            currentTrial.frequency_left,
            currentTrial.roughness_left,
            correctCount/totalCount
        );

        experiment.saveJND(JNDFrame);
    }
}
