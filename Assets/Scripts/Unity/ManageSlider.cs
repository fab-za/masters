using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageSlider : MonoBehaviour
{
    private ManageLineGrid visual;
    [System.Serializable]
    public struct SliderValues{
        public float amplitude_left;
        public float amplitude_right;
        public float period_left;
        public float period_right;

        public SliderValues(float sil, float sir, float spl, float spr){
            amplitude_left = sil;
            amplitude_right = sir;
            period_left = spl;
            period_right = spr;
        }
    }
    public SliderValues slider;
    public int tempLeftRoughness;
    public int tempRightRoughness;
    public float weightAmplitude;
    public float weightPeriod;
    void Start()
    {
        visual = this.gameObject.GetComponent<ManageLineGrid>();
    }

    // Update is called once per frame
    void Update()
    {
        visual.updateParameters(slider.amplitude_left, slider.period_left, tempLeftRoughness, slider.amplitude_right, slider.period_right, tempRightRoughness);
    }

    public void AdjustAmplitudeLeft(float newAmplitude){
        slider.amplitude_left = newAmplitude;
    }

    public void AdjustPeriodLeft(float newPeriod){
        slider.period_left = newPeriod;
    }

    public void AdjustAmplitudeRight(float newAmplitude){
        slider.amplitude_right = newAmplitude;
    }

    public void AdjustPeriodRight(float newPeriod){
        slider.period_right = newPeriod;
    }
    public void convertToRoughness(){
        // somehow based on the current grid parameters, categorise into roughness
          
        // roughness = (weightAmplitude * amplitude) /( weightPeriod * period);
        
        // return higher number = rougher

    }
}
