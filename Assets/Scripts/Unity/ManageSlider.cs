using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageSlider : MonoBehaviour
{
    private ManageExperiment experiment;
    private ManageLineGrid visual;
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
    public int tempLeftRoughness;
    public int tempRightRoughness;
    void Start()
    {
        visual = this.gameObject.GetComponent<ManageLineGrid>();
        experiment = GameObject.Find("ExperimentManager").GetComponent<ManageExperiment>();
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
    public void saveParticipantChoice(float amplitude, float frequency){
        experiment.currentData.participantAmplitude = amplitude;
        experiment.currentData.participantFrequency = frequency;

        experiment.currentData.participantRoughness = experiment.convertToRoughness(amplitude, frequency);

        experiment.saveSlider();
    }

    public void saveLeft(){
        saveParticipantChoice(slider.amplitude_left, slider.frequency_left);
    }

    public void saveRight(){
        saveParticipantChoice(slider.amplitude_right, slider.frequency_right);
    }
}
