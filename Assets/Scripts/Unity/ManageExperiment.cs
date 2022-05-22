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
    public float[] roughnessArray;
    public float weightAmplitude;
    public float weightFrequency;
    void Start()
    {
        csvWriter.initRoughnessCSVs(roughnessArray);
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
          
        float roughness = (weightAmplitude * amplitude) /(weightFrequency * frequency);
        
        // return higher number = rougher
        return roughness;
    }
}
