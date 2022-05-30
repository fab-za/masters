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

    string[] alphabet = new string[]{"A", "G", "B", "D", "E", "F", "H", "I", "J", "K", "M", "N", "O", "P", "Q", "R", "U", "V", "W", "X", "Y", "Z"};
    
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

    public int selectedClass;

    private int cur;
    public int rndIndex;
    
    private int phase;
    private bool training;
    private string title;
    private bool noHaptic;

    private float blockDuration;
    private float textDuration;
    private float presentDuration;

    private bool phase_complete;
    public bool task_complete;
    private int direction;

    public float correctCount;
    public float incorrectCount;
    public float totalCount;
    public float accuracy;

    void Start()
    {
        visual = GameObject.Find("VisualManager").GetComponent<ManageLineForJND>();
        tension = GameObject.Find("VisualManager").GetComponent<SendTension>();        
        experiment = GameObject.Find("ExperimentManager").GetComponent<ManageExperiment>();
        sp = GameObject.Find("SerialController").GetComponent<ConnectSP>();

        percent = 0.03f;
        phase = 0;
        phase_complete = false;
        task_complete = false;
        
        blockDuration = 0.5f;
        textDuration = 4;
        presentDuration = 2;      
        
        cur = 0;
        rndIndex = 0;
        
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

            if(correctCount > 4){
                passed.text = "PASSED 5 TIMES";
                correctCount = 0;
                incorrectCount = 0;
                if(direction == -1){
                    changePhase();
                }
                direction = 1;
                changeCurrentTrial(direction);
            }
            else if(incorrectCount > 4){
                passed.text = "FAILED 5 TIMES, INCREASING";
                correctCount = 0;
                incorrectCount = 0;
                direction = -1;
                changeCurrentTrial(direction);
            }

            if(task_complete){
                // Debug.Log("in if task complete");

                task_complete = false;

                popupSaved();
                StartCoroutine(presentPairs());
            }
        }   
        else{
            possibleTrials = new List<TrialParameters>(){baseline, JNDList[0]};

            cur = -1;
            indicator.text = title + "TRAINING";
            trainingButton.SetActive(true);
            // selectButtons.SetActive(false);

            if(correctCount > 4){
                passed.text = "PASSED 5 TIMES, DECREASING";
                correctCount = 0;
                incorrectCount = 0;
            }
            else if(incorrectCount > 4){
                passed.text = "FAILED 5 TIMES, INCREASING";
                correctCount = 0;
                incorrectCount = 0;
            }


            if(task_complete){
                task_complete = false;

                popupSaved();
                StartCoroutine(presentPairs());
            }
        }
    }

    public void initTrialList(){
        // Debug.Log("init Trials");
        for(int i=20; i>0; i--){
            TrialParameters trial = new TrialParameters(
                baseline.roughness_left + i,
                Random.Range(0.0f, 1.0f),
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
        // Debug.Log("training ended so task complete: " + task_complete);
    }

    public void changeVisual(){
        checkAccuracy(selectedClass);
        task_complete = true;

        if(phase == 1){
            noHaptic = false;
        }

        rndIndex = Random.Range(0,2);
    }


    public void changeCurrentTrial(int direction){
        saveCurrentJND(cur);

        if((cur < (JNDList.Count-1)) && (cur > 0) ){
            cur = cur + (1*direction);
        }
        else{
            if(!training){
                phase_complete = true;
                changePhase();
            }
            cur = 0;
        }

        updateCurrentTrial();
    }

    public void updateCurrentTrial(){
        currentTrial = JNDList[cur];     
        possibleTrials = new List<TrialParameters>(){baseline, currentTrial};
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

    public IEnumerator presentPairs(){

        selectButtons.SetActive(false);

        // Debug.Log("baseline 1: " + noHaptic);

        visual.updateParameters(baseline.frequency_left, baseline.amplitude_left, baseline.frequency_right, baseline.amplitude_left);

        if(!noHaptic){
            // Debug.Log("setting vibration code: "+ alphabet[(int)baseline.roughness_left]);
            sp.vibrationModes = alphabet[(int)baseline.roughness_left] + alphabet[(int)baseline.roughness_right];
        }

        yield return new WaitForSeconds(presentDuration);

        // Debug.Log("baseline shown 1");
        indicator.text = title;
        sp.vibrationModes = "AA";
        visualBlock.SetActive(true);

        yield return new WaitForSeconds(blockDuration);

        noHaptic = false;
        visualBlock.SetActive(false);

        // Debug.Log("comparing 1: " + noHaptic);

        visual.updateParameters(comparingTrial.frequency_left, comparingTrial.amplitude_left, comparingTrial.frequency_right, comparingTrial.amplitude_left);

        if(!noHaptic){
            // Debug.Log("setting vibration code: "+ alphabet[(int)comparingTrial.roughness_left]);
            sp.vibrationModes = alphabet[(int)comparingTrial.roughness_left] + alphabet[(int)comparingTrial.roughness_right];
        }

        yield return new WaitForSeconds(presentDuration);

        // Debug.Log("comparing shown 1");
        indicator.text = title;
        sp.vibrationModes = "AA";
        visualBlock.SetActive(true);

        yield return new WaitForSeconds(blockDuration);

        visualBlock.SetActive(false);

        // Debug.Log("baseline 2: " + noHaptic);

        visual.updateParameters(baseline.frequency_left, baseline.amplitude_left, baseline.frequency_right, baseline.amplitude_left);

        if(!noHaptic){
            sp.vibrationModes = alphabet[(int)baseline.roughness_left] + alphabet[(int)baseline.roughness_right];
        }

        yield return new WaitForSeconds(presentDuration);

        // Debug.Log("baseline shown 2");
        indicator.text = title;
        sp.vibrationModes = "AA";
        visualBlock.SetActive(true);

        yield return new WaitForSeconds(blockDuration);

        noHaptic = false;
        visualBlock.SetActive(false);

        // Debug.Log("comparing 2: " + noHaptic);

        visual.updateParameters(comparingTrial.frequency_left, comparingTrial.amplitude_left, comparingTrial.frequency_right, comparingTrial.amplitude_left);

        if(!noHaptic){
            sp.vibrationModes = alphabet[(int)comparingTrial.roughness_left] + alphabet[(int)comparingTrial.roughness_right];
        }

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
        if(rndIndex == 1){
            totalCount += 1;

            if(choice == rndIndex){
                correctCount += 1;
            } else{
                incorrectCount += 1;
            }

            Debug.Log(correctCount/totalCount);
            accuracy = correctCount/totalCount;
        }
    }

    public void saveCurrentJND(int experimentIndex){
        JNDDataStruct JNDFrame = new JNDDataStruct(
            phase,
            experimentIndex, // index of experiment actually, in this case the level of difference
            experiment.index, // participant index (my naming sucks sorry @ self)

            currentTrial.frequency_left,
            currentTrial.roughness_left,
            accuracy
        );

        experiment.saveJND(JNDFrame);
    }
}
