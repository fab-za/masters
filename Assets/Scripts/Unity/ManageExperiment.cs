using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManageExperiment : MonoBehaviour
{
    public int mode;
    public Text title;

    public ManageSlider slider;
    public GameObject sliderUI;

    public ManageTraining training;
    public GameObject trainingUI;

    public ManagePrelim prelim;
    public GameObject prelimUI;

    public ManageMultimodal modal;
    public GameObject modalUI;

    public ManageJND JND;
    public GameObject JNDUI;

    public bool finished = false;
    public GameObject finishedUI;

    public ManageLineGrid lineGrid;
    public ManageLineForJND lineJND;

    public SendFrequency frequencyManager;
    public SendTension tensionManager;

    public GameObject finger;

    public List<DataStruct> fullData;
    public DataStruct currentData_left;
    public DataStruct currentData_right;
    public List<ComparisonDataStruct> fullComparisonData;
    public List<MultimodalDataStruct> fullMultimodalData;

    public CsvWriter csvWriter;
    public int index;

    public List<float> variableList;
    public List<TrialParameters> allParameters;

    public bool newStart = false;
    public string experimentType;

    void Awake(){
        Debug.Log($"Initialized");
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {        
        // convertAllRoughness();
        // allParameters = new List<TrialParameters>(){train1,train2,train3,trial1,trial2,trial3,trial4,trial5,trial6};

        if(newStart){
            foreach(TrialParameters trial in allParameters){
                variableList.Add(trial.frequency_left);
                variableList.Add(trial.frequency_right);
            }
            csvWriter.initIndvCSVs(variableList, experimentType);
            newStart = false;
        }

        if(mode == 0){
            title.text = "TRAINING MODE";

            lineGrid.enabled = true;
            lineJND.enabled = false;
            frequencyManager.enabled = true;
            tensionManager.enabled = true;
            finger.SetActive(true);

            initParticipantCSVs();

            training.enabled = true;
            trainingUI.SetActive(true);

            slider.enabled = false;
            sliderUI.SetActive(false);

            prelim.enabled = false;
            prelimUI.SetActive(false);

            modal.enabled = false;
            modalUI.SetActive(false);
        } 
        else if(mode == 3){
            title.text = "SLIDER";

            lineGrid.enabled = true;
            lineJND.enabled = false;
            frequencyManager.enabled = true;
            tensionManager.enabled = true;
            finger.SetActive(true);
            
            training.enabled = false;
            trainingUI.SetActive(false);

            slider.enabled = true;
            sliderUI.SetActive(true);

            prelim.enabled = false;
            prelimUI.SetActive(false);

            modal.enabled = false;
            modalUI.SetActive(false);

            JND.enabled = false;
            JNDUI.SetActive(false);
        }
        else if(mode == 1){
            title.text = "PRELIM EXPERIMENT";

            lineGrid.enabled = true;
            lineJND.enabled = false;
            frequencyManager.enabled = true;
            tensionManager.enabled = true;
            finger.SetActive(true);
            
            training.enabled = false;
            trainingUI.SetActive(false);

            slider.enabled = true;      // prelim uses slider script as well
            sliderUI.SetActive(false);

            prelim.enabled = true;
            prelimUI.SetActive(true);

            modal.enabled = false;
            modalUI.SetActive(false);

            JND.enabled = false;
            JNDUI.SetActive(false);
        }
        else if(mode == 2){
            title.text = "MODAL EXPERIMENT";

            lineGrid.enabled = true;
            lineJND.enabled = false;
            frequencyManager.enabled = true;
            tensionManager.enabled = true;
            finger.SetActive(true);
            
            training.enabled = false;
            trainingUI.SetActive(false);

            slider.enabled = false;      
            sliderUI.SetActive(false);

            prelim.enabled = false;
            prelimUI.SetActive(false);

            modal.enabled = true;
            modalUI.SetActive(true);
            
            JND.enabled = false;
            JNDUI.SetActive(false);
        }
        else if(mode == 4){
            title.text = "JND EXPERIMENT";

            lineGrid.enabled = false;
            lineJND.enabled = true;
            frequencyManager.enabled = false;
            tensionManager.enabled = true;
            finger.SetActive(false);
            
            training.enabled = false;
            trainingUI.SetActive(false);

            slider.enabled = false;      
            sliderUI.SetActive(false);

            prelim.enabled = false;
            prelimUI.SetActive(false);

            modal.enabled = false;
            modalUI.SetActive(false);

            JND.enabled = true;
            JNDUI.SetActive(true);
        }

        if(finished){
            saveFullData();
            finishedUI.SetActive(true);
            finished = false;
        }
    }
    public void changeModes(){
        if(mode < 2){
            mode += 1;
        } else{
            mode = 0;
        }
    }
    public void initData(){
        fullData = new List<DataStruct>();
    }
    public void saveFullData(){
        if(mode == 1){
            csvWriter.storeParticipantCSV(fullData, index);
        }        
    }
    public void saveSlider(DataStruct data){
        csvWriter.addToCSV(data);
    }
    public float convertToRoughness(float amplitude, float frequency){
        // somehow based on the current grid parameters, categorise into roughness
          
        // float roughness = (weightAmplitude * amplitude) /(weightFrequency * frequency);
        float roughness = -(20f + 4f*(frequency - 30f));
        
        // return higher number = rougher
        return roughness;
    }

    public void convertAllRoughness(){
        foreach(TrialParameters trial in allParameters){
            trial.roughness_left = convertToRoughness(trial.amplitude_left, trial.frequency_left);
            trial.roughness_right = convertToRoughness(trial.amplitude_right, trial.frequency_right);
        }
    }
    public void saveComparison(ComparisonDataStruct data){
        data.experimentIndex = prelim.current;
        csvWriter.addToComparisonCSV(data);        
    }
    public void initParticipantCSVs(){
        csvWriter.initCSV(index);
    }
    public void saveMultimodal(MultimodalDataStruct data){
        data.experimentIndex = modal.current;
        csvWriter.addToMultimodalCSV(data);        
    }
    public void saveJND(JNDDataStruct data){
        data.experimentIndex = modal.current;
        csvWriter.addToJNDCSV(data);        
    }
}
