using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageExperiment : MonoBehaviour
{
    public ManageSlider slider;
    public GameObject sliderUI;
    public ManageTraining training;
    public GameObject trainingUI;
    public bool trainingMode;
    public List<DataStruct> fullData;
    public DataStruct currentData;
    public CsvWriter csvWriter;
    public int index;
    public List<float> variableList;
    // public float weightAmplitude;
    // public float weightFrequency;
    public TrialParameters train1;
    public TrialParameters train2;
    public TrialParameters train3;
    public TrialParameters trial1;
    public TrialParameters trial2;
    public TrialParameters trial3;

    void Start()
    {
        List<TrialParameters> allParameters = new List<TrialParameters>(){train1,train2,train3,trial1,trial2,trial3};
        foreach(TrialParameters trial in allParameters){
            variableList.Add(trial.frequency_left);
            variableList.Add(trial.frequency_right);
        }
        csvWriter.initIndvCSVs(variableList, "frequencies");
    }

    // Update is called once per frame
    void Update()
    {
        if(trainingMode){
            training.enabled = true;
            trainingUI.SetActive(true);

            slider.enabled = false;
            sliderUI.SetActive(false);
        } else{
            training.enabled = false;
            trainingUI.SetActive(false);

            slider.enabled = true;
            sliderUI.SetActive(true);
        }
    }
    public void changeModes(){
        if(trainingMode){
            trainingMode = false;
        } else{
            trainingMode = true;
        }
    }
    public void initData(){
        fullData = new List<DataStruct>();
    }
    public void saveFullData(){
        csvWriter.storeParticipantCSV(fullData, index);
    }
    public void saveSlider(){
        csvWriter.addToCSV(currentData);
        fullData.Add(currentData);
    }
    public float convertToRoughness(float amplitude, float frequency){
        // somehow based on the current grid parameters, categorise into roughness
          
        // float roughness = (weightAmplitude * amplitude) /(weightFrequency * frequency);
        float roughness = -(250f + 2f*(frequency - 35f));
        
        // return higher number = rougher
        return roughness;
    }
}
