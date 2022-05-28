using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManageSlider : MonoBehaviour
{
    public ManageExperiment experiment;
    private ManageLineGrid visual;
    public Text saved;
    public DisplayCount display;
    [System.Serializable]
    public struct SliderValues{
        public float amplitude_left;
        public float amplitude_right;
        public float frequency_left;
        public float frequency_right;

        public SliderValues(float sil, float sir, float spl, float spr){
            amplitude_left = sil;
            amplitude_right = sir;
            frequency_left = spl;
            frequency_right = spr;
        }
    }
    public SliderValues slider;
    public float tempLeftRoughness;
    public float tempRightRoughness;
    
    void Start()
    {
        visual = this.gameObject.GetComponent<ManageLineGrid>();
        slider = new SliderValues(1,1,40,40);
    }

    // Update is called once per frame
    void Update()
    {
        visual.updateParameters(slider.amplitude_left, slider.frequency_left, tempLeftRoughness, slider.amplitude_right, slider.frequency_right, tempRightRoughness);
    }

    public void AdjustAmplitudeLeft(float newAmplitude){
        slider.amplitude_left = newAmplitude;
    }

    public void AdjustFrequencyLeft(float newFrequency){
        slider.frequency_left = newFrequency;
    }

    public void AdjustAmplitudeRight(float newAmplitude){
        slider.amplitude_right = newAmplitude;
    }

    public void AdjustFrequencyRight(float newFrequency){
        slider.frequency_right = newFrequency;
    }
    public void saveParticipantChoice(float amplitude, float frequency, DataStruct tempFrame){
        tempFrame.participantAmplitude = amplitude;
        tempFrame.participantFrequency = frequency;
        tempFrame.participantRoughness = experiment.convertToRoughness(amplitude, frequency);

        experiment.fullData.Add(tempFrame);
        experiment.saveSlider(tempFrame);
    }

    public void saveLeft(){
        Debug.Log("left saved: " + slider.amplitude_left + ", " + slider.frequency_left);
        // saved.text = "Saved Left for Pattern " + (display.counter+1);

        DataStruct tempFrame = new DataStruct();

        tempFrame.trialFrequency = experiment.currentData_left.trialFrequency;
        tempFrame.trialAmplitude = experiment.currentData_left.trialAmplitude;
        tempFrame.trialRoughness = experiment.currentData_left.trialRoughness;

        saveParticipantChoice(slider.amplitude_left, slider.frequency_left, tempFrame);
    }

    public void saveRight(){
        Debug.Log("right saved: " + slider.amplitude_right + ", " + slider.frequency_right);
        // saved.text = "Saved Right for Pattern " + (display.counter+1);

        DataStruct tempFrame = new DataStruct();

        tempFrame.trialFrequency = experiment.currentData_right.trialFrequency;
        tempFrame.trialAmplitude = experiment.currentData_right.trialAmplitude;
        tempFrame.trialRoughness = experiment.currentData_right.trialRoughness;

        saveParticipantChoice(slider.amplitude_right, slider.frequency_right, tempFrame);
    }
    public void saveComparisonFile(int experimentIndex){
        ComparisonDataStruct comparisonFrame = new ComparisonDataStruct(
            experimentIndex, // index of experiment actually
            experiment.index, // participant index (my naming sucks sorry @ self)

            experiment.currentData_left.trialFrequency,
            experiment.currentData_left.trialRoughness,
            experiment.currentData_left.trialAmplitude,

            experiment.currentData_right.trialFrequency,
            experiment.currentData_right.trialRoughness,
            experiment.currentData_right.trialAmplitude,

            slider.frequency_left,
            experiment.convertToRoughness(slider.amplitude_left, slider.frequency_left),
            slider.amplitude_left,

            slider.frequency_right,
            experiment.convertToRoughness(slider.amplitude_right, slider.frequency_right),
            slider.amplitude_right
            
        );

        experiment.saveComparison(comparisonFrame);
    }

    
}
