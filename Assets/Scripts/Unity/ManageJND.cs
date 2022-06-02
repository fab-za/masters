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
    public Text streakText;
    public Text question;
    public GameObject visualBlock;
    public GameObject leftLine;
    public GameObject rightLine;
    public GameObject trainingButton;
    public GameObject selectButtons;
    public GameObject breakMessage;

    public TrialParameters baseline;  // STANDARD
    public TrialParameters baselineUni;  // STANDARD UNIMODAL
    public TrialParameters baselineMulti;  // STANDARD MULTIMODAL, fv IS HIGHER THAN TRUE

    public List<TrialParameters> JNDList;
    public List<List<TrialParameters>> possibleTrials;
    private TrialParameters currentTrial;
    private List<TrialParameters> comparingTrial;
    private List<TrialParameters> notAttemptedTrials;

    private float percentInterval;

    public int phase;
    private bool training;
    private string title;
    private bool noHaptic;

    private int selectedClass;

    public int cur;
    private int[] curs;
    public int baselineLoc;
    private int highestTrial;

    private float blockDuration;
    private float textDuration;
    private float presentDuration;

    // private bool phase_complete;
    private bool task_complete;
    private int direction;

    public float correctCount;
    public float incorrectCount;
    public float totalCount;
    public float accuracy;
    private float passTextCounter;    
    private bool lastChoiceMade;
    public bool correctChoice;
    public float incorrectStreak;
    public float correctStreak;

    void Start()
    {
        visual = GameObject.Find("VisualManager").GetComponent<ManageLineForJND>();
        tension = GameObject.Find("VisualManager").GetComponent<SendTension>();        
        experiment = GameObject.Find("ExperimentManager").GetComponent<ManageExperiment>();
        sp = GameObject.Find("SerialController").GetComponent<ConnectSP>();

        percentInterval = 0.0075f;
        task_complete = false;
        // question.SetActive(false);
        
        blockDuration = 0.1f;
        textDuration = 4;
        presentDuration = 2;      
        
        curs = new int[]{0,9,9,9,9};
        cur = curs[phase];
        baselineLoc = 1;
        highestTrial = 0;
        direction = 0;
        // lastChoiceMade = 1;
        incorrectStreak = 0;
        correctStreak = 0;
        
        JNDList = new List<TrialParameters>();
        initTrialList();
        updateCurrentTrial();
        changePhase();
    }

    // Update is called once per frame
    void Update()
    {        
        display.T.text = "LEVEL: " + (cur+1);
        streakText.text = "CURRENT CORRECT STREAK: " + correctStreak;

        comparingTrial = possibleTrials[baselineLoc];

        tension.currentState.left = "T";
        tension.currentState.right = "T"; 

        if(!training){
            trainingButton.SetActive(false);

            // Debug.Log("experiment mode");

            if(correctStreak > 4){
                passed.text = "5 PASS IN A ROW, STEP DOWN";
                StartCoroutine(popupPass());

                correctCount = 0;
                incorrectCount = 0;
                incorrectStreak = 0;
                correctStreak = 0;
                direction = 1;
                changeCurrentTrial(direction);
            }
            else if(incorrectStreak > 2){
                passed.text = "3 FAIL IN A ROW, STEP UP";
                StartCoroutine(popupPass());

                correctCount = 0;
                incorrectCount = 0;
                incorrectStreak = 0;
                correctStreak = 0;
                direction = -1;
                changeCurrentTrial(direction);
            }
            else if(incorrectCount > 4){
                passed.text = "FAILED 5 TIMES, STEP UP";
                StartCoroutine(popupPass());

                correctCount = 0;
                incorrectCount = 0;
                incorrectStreak = 0;
                correctStreak = 0;
                direction = -1;
                changeCurrentTrial(direction);
            }

            if(task_complete){
                task_complete = false;
                StartCoroutine(presentPairs());
            }
        }   
        else{
            possibleTrials = new List<List<TrialParameters>>(){new List<TrialParameters>(){baseline, JNDList[0]},new List<TrialParameters>(){JNDList[0], baseline}};

            indicator.text = title + "TRAINING";

            if(correctStreak > 4){
                passed.text = "5 PASS IN A ROW, STEP DOWN";
                StartCoroutine(popupPass());

                correctCount = 0;
                incorrectCount = 0;
                incorrectStreak = 0;
                correctStreak = 0;
            }
            else if(incorrectStreak > 2){
                passed.text = "3 FAIL IN A ROW, STEP UP";
                StartCoroutine(popupPass());

                correctCount = 0;
                incorrectCount = 0;
                incorrectStreak = 0;
                correctStreak = 0;
            }
            else if(incorrectCount > 4){
                passed.text = "FAILED 5 TIMES, STEP UP";
                StartCoroutine(popupPass());

                correctCount = 0;
                incorrectCount = 0;
                incorrectStreak = 0;
                correctStreak = 0;
            }


            if(task_complete){
                task_complete = false;

                StartCoroutine(presentPairs());
            }
        }
    }

    public void initTrialList(){
        Debug.Log("init Trials");
        for(int i=20; i>0; i--){
            TrialParameters trial = new TrialParameters(
                baselineUni.roughness_left + i,
                Random.Range(0.5f, 2.5f),
                baselineUni.frequency_left * (1+(percentInterval*i)),

                baselineUni.roughness_right + i,
                baselineUni.amplitude_right,
                baselineUni.frequency_right * (1+(percentInterval*i))
            );

            Debug.Log("trial exists, attempt add");

            JNDList.Add(trial);
        }
        possibleTrials = new List<List<TrialParameters>>(){new List<TrialParameters>(){baseline, JNDList[0]},new List<TrialParameters>(){JNDList[0], baseline}};
    }

    public void selectPhase(){
        if(phase == 1){
            title = "Haptic ";
            leftLine.SetActive(false);
            rightLine.SetActive(false);
            training = true;
            noHaptic = false;
            baseline = baselineUni;
            visual.percent = 0;
        }
        else if(phase == 0){
            title = "Visual ";
            leftLine.SetActive(true);
            rightLine.SetActive(true);
            training = true;
            noHaptic = true;
            sp.vibrationModes = "AA";
            baseline = baselineUni;
            visual.percent = 0;
        }
        else if(phase == 2){
            title = "Multi ";
            leftLine.SetActive(true);
            rightLine.SetActive(true);
            training = true;
            noHaptic = false;
            baseline = baselineMulti;
            visual.percent = 0;
        }
        else if(phase == 3){
            title = "Multi 50 ";
            leftLine.SetActive(true);
            rightLine.SetActive(true);
            training = true;
            noHaptic = false;
            baseline = baselineMulti;
            visual.percent = 50;
        }
        else if(phase == 4){
            title = "Multi 100 ";
            leftLine.SetActive(true);
            rightLine.SetActive(true);
            training = true;
            noHaptic = false;
            baseline = baselineMulti;
            visual.percent = 100;
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
        cur = curs[phase];
        task_complete = true;
        correctCount = 0;
        incorrectCount = 0;
        incorrectStreak = 0;
        correctStreak = 0;
        totalCount = 0;
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

        baselineLoc = Random.Range(0,2);
    }


    public void changeCurrentTrial(int direction){
        saveCurrentJND(cur);
        totalCount = 0;
        
        trackHighestTrial();

        if((cur < (JNDList.Count-1)) && (cur > -1) ){
            cur = cur + (1*direction);
        }
        else if((cur == (JNDList.Count-1) && (direction == -1))){
            cur = cur + (1*direction);
        }
        else{
            if(!training){
                // changePhase();
                experiment.finished = true;
                if(highestTrial != 19){
                    addEmptyForNotAttempted();
                }
                this.enabled = false;
            }
            cur = curs[phase];
        }

        updateCurrentTrial();
    }

    public void updateCurrentTrial(){
        currentTrial = JNDList[cur];     
        possibleTrials = new List<List<TrialParameters>>(){new List<TrialParameters>(){baseline, currentTrial},new List<TrialParameters>(){currentTrial, baseline}};
    }

    public void trackHighestTrial(){
        if(cur > highestTrial){
            highestTrial = cur;
        }
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
        trainingButton.SetActive(false);
        visualBlock.SetActive(true);
        // question.SetActive(true);

        // Debug.Log("baseline 1: " + noHaptic);
        possibleTrials[0][1].amplitude_left = Random.Range(0.5f, 2.5f); // generate new offset for currentTrial
        possibleTrials[1][0].amplitude_left = Random.Range(0.5f, 2.5f);

        visual.updateParameters(comparingTrial[0].frequency_left, comparingTrial[0].amplitude_left, comparingTrial[0].frequency_right, comparingTrial[0].amplitude_left);

        yield return new WaitForSeconds(blockDuration);
        
        if(!noHaptic){
            // Debug.Log("setting vibration code: "+ alphabet[(int)baseline.roughness_left]);
            sp.vibrationModes = alphabet[(int)comparingTrial[0].roughness_left] + alphabet[(int)comparingTrial[0].roughness_right];
        }

        question.text = "WHICH PATTERN HAD SMALLER BUMPS? NOW SHOWING 1";
        visualBlock.SetActive(false);

        yield return new WaitForSeconds(presentDuration);

        // Debug.Log("baseline shown 1");
        indicator.text = title;
        sp.vibrationModes = "AA";
        visualBlock.SetActive(true);

        noHaptic = false;

        // Debug.Log("comparing 1: " + noHaptic);
        // comparingTrial[1].amplitude_left = Random.Range(0.5f, 2.5f);

        visual.updateParameters(comparingTrial[1].frequency_left, comparingTrial[1].amplitude_left, comparingTrial[1].frequency_right, comparingTrial[1].amplitude_left);

        yield return new WaitForSeconds(blockDuration);

        if(!noHaptic){
            // Debug.Log("setting vibration code: "+ alphabet[(int)comparingTrial.roughness_left]);
            sp.vibrationModes = alphabet[(int)comparingTrial[1].roughness_left] + alphabet[(int)comparingTrial[1].roughness_right];
        }

        question.text = "WHICH PATTERN HAD SMALLER BUMPS? NOW SHOWING 2";
        visualBlock.SetActive(false);

        yield return new WaitForSeconds(presentDuration);

        // Debug.Log("comparing shown 1");
        indicator.text = title;
        sp.vibrationModes = "AA";
        visualBlock.SetActive(true);

        // Debug.Log("baseline 2: " + noHaptic);

        visual.updateParameters(comparingTrial[0].frequency_left, comparingTrial[0].amplitude_left, comparingTrial[0].frequency_right, comparingTrial[0].amplitude_left);

        yield return new WaitForSeconds(blockDuration);

        if(!noHaptic){
            sp.vibrationModes = alphabet[(int)comparingTrial[0].roughness_left] + alphabet[(int)comparingTrial[0].roughness_right];
        }

        question.text = "WHICH PATTERN HAD SMALLER BUMPS? NOW SHOWING 1";
        visualBlock.SetActive(false);

        yield return new WaitForSeconds(presentDuration);

        // Debug.Log("baseline shown 2");
        indicator.text = title;
        sp.vibrationModes = "AA";
        visualBlock.SetActive(true);

        noHaptic = false;

        // Debug.Log("comparing 2: " + noHaptic);

        // comparingTrial[1].amplitude_left = Random.Range(0.5f, 2.5f);

        visual.updateParameters(comparingTrial[1].frequency_left, comparingTrial[1].amplitude_left, comparingTrial[1].frequency_right, comparingTrial[1].amplitude_left);

        yield return new WaitForSeconds(blockDuration);

        if(!noHaptic){
            sp.vibrationModes = alphabet[(int)comparingTrial[1].roughness_left] + alphabet[(int)comparingTrial[1].roughness_right];
        }

        question.text = "WHICH PATTERN HAD SMALLER BUMPS? NOW SHOWING 2";
        visualBlock.SetActive(false);

        yield return new WaitForSeconds(presentDuration);

        // Debug.Log("comparing shown 2");
        question.text = "WHICH PATTERN HAD SMALLER BUMPS? USE BUTTONS";
        visualBlock.SetActive(true);
        // question.SetActive(false);
        selectButtons.SetActive(true);

        if(training){
            trainingButton.SetActive(true);
        }
        
        sp.vibrationModes = "AA";
    }

    public void chooseLeft(){
        selectedClass = 0;  // EQUAL
    }
    public void chooseRight(){
        selectedClass = 1;  // SMOOTHER
    }

    public void checkAccuracy(float choice){
        if(choice != baselineLoc){
            correctChoice = true;
            correctCount += 1;
        }
        else{
            correctChoice = false;
            incorrectCount += 1;
        }

        totalCount += 1;
        // Debug.Log(correctCount/totalCount);
        accuracy = (correctCount/totalCount)*100;
    }

    public void checkStreak(float choice){
        if(correctChoice == lastChoiceMade && correctChoice){
            correctStreak += 1;
            incorrectStreak = 0;
            
        }
        else if(correctChoice == lastChoiceMade && !correctChoice){
            correctStreak = 0;
            incorrectStreak += 1;
        }
        else if(correctChoice != lastChoiceMade && correctChoice){
            correctStreak = 1;
            incorrectStreak = 0;
        }
        else if(correctChoice != lastChoiceMade && !correctChoice){
            correctStreak = 0;
            incorrectStreak = 1;
        }
        else{
            correctStreak = 0;
            incorrectStreak = 0;
        }

        lastChoiceMade = correctChoice;
    }

    public void giveUp(){
        saveCurrentJND(cur);
        if(highestTrial != 19){
            addEmptyForNotAttempted();
        }
        phase = 0;
        totalCount = 0;
        experiment.finished = true;
        this.enabled = false;
    }

    public void saveCurrentJND(int experimentIndex){
        JNDDataStruct JNDFrame = new JNDDataStruct(
            phase,
            experimentIndex, // index of experiment actually, in this case the level of difference
            experiment.index, // participant index (my naming sucks sorry @ self)

            currentTrial.frequency_left,
            currentTrial.roughness_left,
            20 + (180.0f * Mathf.Exp(-0.25f*(Mathf.Abs(currentTrial.roughness_left-21)))),
            accuracy
        );

        // Debug.Log(cur);
        // Debug.Log(experimentIndex);
        // Debug.Log(JNDFrame.experimentIndex);

        experiment.saveJND(JNDFrame);
    }

    public void addEmptyForNotAttempted(){
        for(int i=(highestTrial+1); i<(JNDList.Count); i++){
            // add to not attempted list and then write empty result so that index doesnt go whack
            // notAttemptedTrials.Add(JNDList[i]);

            JNDDataStruct JNDFrame = new JNDDataStruct(
                phase,
                i, // index of experiment actually, in this case the level of difference
                experiment.index, // participant index (my naming sucks sorry @ self)

                JNDList[i].frequency_left,
                JNDList[i].roughness_left,
                20 + (180.0f * Mathf.Exp(-0.25f*(Mathf.Abs(JNDList[i].roughness_left-21)))),
                0
            );

            experiment.saveJND(JNDFrame);
        }

    }

}
