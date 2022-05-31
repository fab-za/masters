using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManageJND : MonoBehaviour
{
    private ManageExperiment experiment;
    private ManageLineForJND visual;
    private SendTension tension;
    private ConnectSP sp;

    string[] alphabet = new string[]{"A", "C", "B", "D", "E", "F", "H", "I", "J", "K", "M", "N", "O", "P", "Q", "R", "U", "V", "W", "X", "Y", "Z"};
    
    public DisplayCount display;
    public Text saved;
    public Text indicator;
    public Text passed;
    public GameObject visualBlock;
    public GameObject leftLine;
    public GameObject rightLine;
    public GameObject trainingButton;
    public GameObject selectButtons;
    public GameObject breakMessage;

    public TrialParameters baseline;  // STANDARD
    public List<TrialParameters> JNDList;
    public List<TrialParameters> possibleTrials;
    private TrialParameters currentTrial;
    private TrialParameters comparingTrial;

    private float percent;

    public int phase;
    private bool training;
    private string title;
    private bool noHaptic;

    private int selectedClass;

    public int cur;
    private int rndIndex;

    private float blockDuration;
    private float textDuration;
    private float presentDuration;

    private bool phase_complete;
    private bool task_complete;
    private int direction;

    public float correctCount;
    public float incorrectCount;
    public float totalCount;
    public float accuracy;
    private float lastChoiceMade;
    public float streak;

    void Start()
    {
        visual = GameObject.Find("VisualManager").GetComponent<ManageLineForJND>();
        tension = GameObject.Find("VisualManager").GetComponent<SendTension>();        
        experiment = GameObject.Find("ExperimentManager").GetComponent<ManageExperiment>();
        sp = GameObject.Find("SerialController").GetComponent<ConnectSP>();

        percent = 0.01f;
        phase_complete = false;
        task_complete = false;
        passed.enabled = false;
        
        blockDuration = 0.1f;
        textDuration = 4;
        presentDuration = 2;      
        
        cur = 0;
        rndIndex = 1;
        direction = 0;
        
        JNDList = new List<TrialParameters>();
        initTrialList();
        updateCurrentTrial();
        changePhase();
    }

    // Update is called once per frame
    void Update()
    {
        display.counter = cur;
        comparingTrial = possibleTrials[rndIndex];

        tension.currentState.left = "T";
        tension.currentState.right = "T"; 

        if(!training){
            trainingButton.SetActive(false);

            // Debug.Log("experiment mode");

            if((correctCount > 4) && (streak > 4)){
                passed.text = "PASSED 5 TIMES, DECREASING";
                StartCoroutine(popupPass());

                correctCount = 0;
                incorrectCount = 0;
                streak = 0;
                direction = 1;
                changeCurrentTrial(direction);
            }
            else if((incorrectCount > 4) && (streak > 4)){
                passed.text = "FAILED 5 TIMES, INCREASING";
                StartCoroutine(popupPass());

                correctCount = 0;
                incorrectCount = 0;
                streak = 0;
                direction = -1;
                changeCurrentTrial(direction);
            }

            if(task_complete){
                task_complete = false;
                StartCoroutine(presentPairs());
            }
        }   
        else{
            possibleTrials = new List<TrialParameters>(){baseline, JNDList[0], JNDList[0], JNDList[0], JNDList[0], JNDList[0]};

            indicator.text = title + "TRAINING";
            trainingButton.SetActive(true);

            if((correctCount > 4) && (streak > 4)){
                passed.text = "PASSED 5 TIMES, DECREASING";
                StartCoroutine(popupPass());

                correctCount = 0;
                incorrectCount = 0;
                streak = 0;
            }
            else if((incorrectCount > 4) && (streak > 4)){
                passed.text = "FAILED 5 TIMES, INCREASING";
                StartCoroutine(popupPass());

                correctCount = 0;
                incorrectCount = 0;
                streak = 0;
            }


            if(task_complete){
                task_complete = false;

                StartCoroutine(presentPairs());
            }
        }
    }

    public void initTrialList(){
        // Debug.Log("init Trials");
        for(int i=20; i>0; i--){
            TrialParameters trial = new TrialParameters(
                baseline.roughness_left + i,
                Random.Range(0.5f, 2.5f),
                baseline.frequency_left * (1+(percent*i)),

                baseline.roughness_right + i,
                baseline.amplitude_right,
                baseline.frequency_right * (1+(percent*i))
            );

            // Debug.Log("trial exists, attempt add");

            JNDList.Add(trial);
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
            sp.vibrationModes = "AA";
        }
    }
    public void changePhase(){
        // if(phase < 0){
        //     phase += 1;
        // } else{
        //     experiment.finished = true;
        //     phase = 0;
        // }
        // Debug.Log("phase: "+phase);
        selectPhase();
    }

    public void endTraining(){
        training = false;
        cur = 0;
        task_complete = true;
        correctCount = 0;
        incorrectCount = 0;
        // Debug.Log("training ended so task complete: " + task_complete);
    }

    public void changeVisual(){
        checkAccuracy(selectedClass);
        checkStreak(selectedClass);
        StartCoroutine(popupSaved());

        task_complete = true;

        if(phase == 1){
            noHaptic = false;
        }
        
        // StartCoroutine(popupBlock());

        // rndIndex = Random.Range(0,6);
    }


    public void changeCurrentTrial(int direction){
        saveCurrentJND(cur);
        totalCount = 0;

        if((cur < (JNDList.Count-1)) && (cur > -1) ){
            cur = cur + (1*direction);
        }
        else{
            if(!training){
                phase_complete = true;
                // changePhase();
                experiment.finished = true;
            }
            cur = 0;
        }

        if(cur == -1){
            phase_complete = true;
            changePhase();
        }

        updateCurrentTrial();
    }

    public void updateCurrentTrial(){
        currentTrial = JNDList[cur];     
        possibleTrials = new List<TrialParameters>(){baseline, currentTrial, currentTrial, currentTrial, currentTrial, currentTrial};
    }

    public IEnumerator popupBlock(){  
        indicator.text = title;
        // sp.vibrationModes = "AA";
        visualBlock.SetActive(true);

        yield return new WaitForSeconds(blockDuration);

        visualBlock.SetActive(false);
    }

    public IEnumerator popupSaved(){
        saved.text = "Saved";

        yield return new WaitForSeconds(textDuration);

        saved.text = "";
    }
    public IEnumerator popupPass(){
        passed.enabled = true;

        yield return new WaitForSeconds(textDuration);

        passed.enabled = false;
    }

    public IEnumerator presentPairs(){

        selectButtons.SetActive(false);
        visualBlock.SetActive(true);

        // Debug.Log("baseline 1: " + noHaptic);

        visual.updateParameters(baseline.frequency_left, baseline.amplitude_left, baseline.frequency_right, baseline.amplitude_left);

        yield return new WaitForSeconds(blockDuration);
        
        if(!noHaptic){
            // Debug.Log("setting vibration code: "+ alphabet[(int)baseline.roughness_left]);
            sp.vibrationModes = alphabet[(int)baseline.roughness_left] + alphabet[(int)baseline.roughness_right];
        }

        visualBlock.SetActive(false);

        yield return new WaitForSeconds(presentDuration);

        // Debug.Log("baseline shown 1");
        indicator.text = title;
        sp.vibrationModes = "AA";
        visualBlock.SetActive(true);

        noHaptic = false;

        // Debug.Log("comparing 1: " + noHaptic);
        comparingTrial.amplitude_left = Random.Range(0.5f, 2.5f);

        visual.updateParameters(comparingTrial.frequency_left, comparingTrial.amplitude_left, comparingTrial.frequency_right, comparingTrial.amplitude_left);

        yield return new WaitForSeconds(blockDuration);

        if(!noHaptic){
            // Debug.Log("setting vibration code: "+ alphabet[(int)comparingTrial.roughness_left]);
            sp.vibrationModes = alphabet[(int)comparingTrial.roughness_left] + alphabet[(int)comparingTrial.roughness_right];
        }

        visualBlock.SetActive(false);

        yield return new WaitForSeconds(presentDuration);

        // Debug.Log("comparing shown 1");
        indicator.text = title;
        sp.vibrationModes = "AA";
        visualBlock.SetActive(true);

        // Debug.Log("baseline 2: " + noHaptic);

        visual.updateParameters(baseline.frequency_left, baseline.amplitude_left, baseline.frequency_right, baseline.amplitude_left);

        yield return new WaitForSeconds(blockDuration);

        if(!noHaptic){
            sp.vibrationModes = alphabet[(int)baseline.roughness_left] + alphabet[(int)baseline.roughness_right];
        }

        visualBlock.SetActive(false);

        yield return new WaitForSeconds(presentDuration);

        // Debug.Log("baseline shown 2");
        indicator.text = title;
        sp.vibrationModes = "AA";
        visualBlock.SetActive(true);

        noHaptic = false;

        // Debug.Log("comparing 2: " + noHaptic);

        comparingTrial.amplitude_left = Random.Range(0.5f, 2.5f);

        visual.updateParameters(comparingTrial.frequency_left, comparingTrial.amplitude_left, comparingTrial.frequency_right, comparingTrial.amplitude_left);

        yield return new WaitForSeconds(blockDuration);

        if(!noHaptic){
            sp.vibrationModes = alphabet[(int)comparingTrial.roughness_left] + alphabet[(int)comparingTrial.roughness_right];
        }

        visualBlock.SetActive(false);

        yield return new WaitForSeconds(presentDuration);

        // Debug.Log("comparing shown 2");
        visualBlock.SetActive(true);
        selectButtons.SetActive(true);
        sp.vibrationModes = "AA";
    }

    public void chooseEqual(){
        selectedClass = 0;  // EQUAL
    }
    public void chooseDifferent(){
        selectedClass = 1;  // SMOOTHER
    }

    public void checkAccuracy(float choice){
        lastChoiceMade = choice;
        if(rndIndex == 1){
            totalCount += 1;

            if(choice == rndIndex){
                correctCount += 1;
            } else{
                incorrectCount += 1;
            }

            // Debug.Log(correctCount/totalCount);
            accuracy = correctCount/totalCount;
        }
    }

    public void checkStreak(float choice){
        if(choice == lastChoiceMade){
            streak += 1;
        }
        else{
            streak = 0;
        }
    }

    public void giveUp(){
        phase = 0;
        saveCurrentJND(cur);
        totalCount = 0;
        experiment.finished = true;
    }

    public void saveCurrentJND(int experimentIndex){
        JNDDataStruct JNDFrame = new JNDDataStruct(
            phase,
            experimentIndex, // index of experiment actually, in this case the level of difference
            experiment.index, // participant index (my naming sucks sorry @ self)

            currentTrial.frequency_left,
            currentTrial.roughness_left,
            180.0f * Mathf.Exp(-0.25f*(currentTrial.roughness_left-1)),
            accuracy
        );

        // Debug.Log(cur);
        // Debug.Log(experimentIndex);
        // Debug.Log(JNDFrame.experimentIndex);

        experiment.saveJND(JNDFrame);
    }
}
