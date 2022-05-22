using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageTraining : MonoBehaviour
{
    private GameObject visualManager;
    private ManageLineGrid visual;
    private int currentTraining;
    [System.Serializable]
    public struct TrainingParameters{
        public int roughness_left;
        public float amplitude_left;
        public float frequency_left;
        public int roughness_right;
        public float amplitude_right;
        public float frequency_right;
        public TrainingParameters(int rl, float al, float pl, int rr, float ar, float pr){
            roughness_left = rl;
            amplitude_left = al;
            frequency_left = pl;
            roughness_right = rr;
            amplitude_right = ar;
            frequency_right = pr;
        }
    }
    public TrainingParameters train1;
    public TrainingParameters train2;
    public TrainingParameters train3;

    void Start()
    {
        visualManager = GameObject.Find("VisualManager"); 
        visual = visualManager.GetComponent<ManageLineGrid>();
        currentTraining = 1;
    }

    // Update is called once per frame
    void Update()
    {
        selectTraining(currentTraining);
    }
    public void changeTraining(){
        if(currentTraining < 3){
            currentTraining += 1;
        } else{
            currentTraining = 1;
        }
    }
    public void selectTraining(int cur){
        if(cur == 1){
            visual.updateParameters(train1.amplitude_left, train1.frequency_left, train1.roughness_left, train1.amplitude_right, train1.frequency_right, train1.roughness_right);
        } 
        else if(cur == 2){
            visual.updateParameters(train2.amplitude_left, train2.frequency_left, train2.roughness_left, train2.amplitude_right, train2.frequency_right, train2.roughness_right);
        }
        else if(cur == 3){
            visual.updateParameters(train3.amplitude_left, train3.frequency_left, train3.roughness_left, train3.amplitude_right, train3.frequency_right, train3.roughness_right);
        }
    }
}
