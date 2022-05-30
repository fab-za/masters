using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManageJND : MonoBehaviour
{
    private ManageExperiment experiment;
    private ManageLineGrid visual;
    private SendTension tension;
    private SendFrequency frequencyManager;
    private ConnectSP sp;
    public DisplayCount display;
    public Text saved;
    public Text indicator;
    public GameObject visualBlock;
    public GameObject trainingButton;
    public GameObject selectButtons;
    public GameObject breakMessage;

    public TrialParameters trial1;
    public TrialParameters trial2;

    public TrialParameters trial3;
    public TrialParameters trial4;

    public TrialParameters trial5;
    public TrialParameters trial6;

    public TrialParameters trial7;
    public TrialParameters trial8;

    // public TrialParameters trial9;
    // public TrialParameters trial10;
    // public TrialParameters trial11;
    // public TrialParameters trial12;

    // public TrialParameters trial13;
    // public TrialParameters trial14;
    // public TrialParameters trial15;
    // public TrialParameters trial16;
    // public TrialParameters trial17;
    // public TrialParameters trial18;
    // public TrialParameters trial19;
    // public TrialParameters trial20;
    // public TrialParameters trial21;

    public int selectedClass;

    public int current;
    private int cur;
    public int[] order = new int[]{0};
    private int temp;
    private int phase;
    private bool training;

    private int frames;
    private int popupduration;
    private bool phase_complete;
    private bool task_complete;

    private float choiceMade;
    private float lastChoiceMade;
    private float responseTime;

    void Start()
    {
        visual = GameObject.Find("VisualManager").GetComponent<ManageLineGrid>();
        tension = GameObject.Find("VisualManager").GetComponent<SendTension>();        
        frequencyManager = GameObject.Find("VisualManager").GetComponent<SendFrequency>();
        experiment = GameObject.Find("ExperimentManager").GetComponent<ManageExperiment>();
        sp = GameObject.Find("SerialController").GetComponent<ConnectSP>();

        frequencyManager.alphabet = new string[]{"A", "G", "B", "D", "E", "F", "H", "I", "J", "K", "M", "N", "O", "P", "Q", "R", "U", "V", "W", "X", "Y", "Z"};
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
