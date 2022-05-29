 using System.Collections;
 using System.Collections.Generic;
 using UnityEngine;
 using UnityEngine.UI;

public class DisplayCount : MonoBehaviour
{
    public int counter;
    public Text T;

    void Start()
    {
        T = this.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        T.text = "Task: " + (counter+1);
    }
}
