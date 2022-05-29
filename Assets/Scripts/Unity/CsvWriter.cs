using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;

public class CsvWriter : MonoBehaviour
{
    public void initCSV(int index){
        string folderpath = Application.dataPath + "/Data/" + index + "/";

        if(!System.IO.File.Exists(folderpath)){
            Directory.CreateDirectory(folderpath);
        }

        string resultpath = folderpath + "result.csv";

        if(!System.IO.File.Exists(resultpath)){
            var result  = new StringBuilder("Trial Frequency, Trial Roughness, Trial Amplitude, Participant Frequency, Participant Roughness, Participant Amplitude, Frequency Error, Roughness Error, Amplitude Error");
            result.Append('\n');

            var writer = new StreamWriter(resultpath, false); // true for append, false for overwrite
            writer.Write(result);
            writer.Close();
            Debug.Log($"new CSV file written to \"{resultpath}\"");
        }

        string comparisonpath = folderpath + "comparison_result.csv";

        if(!System.IO.File.Exists(comparisonpath)){
            var result2  = new StringBuilder("Experiment Index, Participant Index, Trial Left Frequency, Trial Left Roughness, Trial Left Amplitude, Trial Right Frequency, Trial Right Roughness, Trial Right Amplitude, Left Frequency, Left Roughness, Left Amplitude, Right Frequency, Right Roughness, Right Amplitude, Visual Frequency Difference Between Sides, Haptic Frequency Difference Between Sides, Amplitude Difference Between Sides");
            result2.Append('\n');

            var writer2 = new StreamWriter(comparisonpath, false); // true for append, false for overwrite
            writer2.Write(result2);
            writer2.Close();
            Debug.Log($"new CSV file written to \"{comparisonpath}\"");
        }
    }
    

    public void addToCSV(DataStruct frame){
        var result = new StringBuilder("");
        result.Append(frame.trialFrequency.ToString()).Append(',').Append(frame.trialRoughness.ToString()).Append(',').Append(frame.trialAmplitude.ToString()).Append(',').Append(frame.participantFrequency.ToString()).Append(',').Append(frame.participantRoughness.ToString()).Append(',').Append(frame.participantRoughness.ToString()).Append(',').Append(frame.errorFrequency.ToString()).Append(',').Append(frame.errorRoughness.ToString()).Append('\n');
        //Debug.Log(result);

        int index = (int)frame.trialFrequency;
        result.ToString();

        string path = Application.dataPath + "/Data/frequencies/" + index + "_result.csv";
        var writer = new StreamWriter(path, true); // true for append, false for overwrite
        writer.Write(result);
        writer.Close();
        Debug.Log($"result CSV file written to \"{path}\"");
    }
    public void storeParticipantCSV(List<DataStruct> data, int index){
        string path = Application.dataPath + "/Data/" + index + "/result.csv";
        var result = new StringBuilder("");

        foreach (DataStruct frame in data)
        {
            result.Append(frame.trialFrequency.ToString()).Append(',').Append(frame.trialRoughness.ToString()).Append(',').Append(frame.trialAmplitude.ToString()).Append(',').Append(frame.participantFrequency.ToString()).Append(',').Append(frame.participantRoughness.ToString()).Append(',').Append(frame.participantRoughness.ToString()).Append(',').Append(frame.errorFrequency.ToString()).Append(',').Append(frame.errorRoughness.ToString()).Append('\n');
            //Debug.Log(result);
        }

        result.ToString();
        var writer = new StreamWriter(path, true); // true for append, false for overwrite
        writer.Write(result);
        writer.Close();
        Debug.Log($"result CSV file written to \"{path}\"");
    }
    public void initIndvCSVs(List<float> list, string type){
        // Debug.Log("hello");
        foreach(float l in list){
            string folderpath = Application.dataPath + "/Data/" + type + "/";

            if(!System.IO.File.Exists(folderpath)){
                Directory.CreateDirectory(folderpath);
            }

            string path = folderpath + (int)l + "_result.csv";
            // Debug.Log(path);

            if(!System.IO.File.Exists(path)){
                var result  = new StringBuilder("Trial Frequency, Trial Roughness, Trial Amplitude, Participant Frequency, Participant Roughness, Participant Amplitude, Frequency Error, Roughness Error, Amplitude Error");
                result.Append('\n');

                
                var writer = new StreamWriter(path, false); // true for append, false for overwrite
                writer.Write(result);
                writer.Close();
                Debug.Log($"new CSV file written to \"{path}\"");
            }
        }
        
    }

    public void addToComparisonCSV(ComparisonDataStruct frame){
        var result = new StringBuilder("");
        result.Append(frame.experimentIndex.ToString()).Append(',').Append(frame.participantIndex.ToString()).Append(',').Append(frame.trialLeftFrequency.ToString()).Append(',').Append(frame.trialLeftRoughness.ToString()).Append(',').Append(frame.trialLeftAmplitude.ToString()).Append(',').Append(frame.trialRightFrequency.ToString()).Append(',').Append(frame.trialRightRoughness.ToString()).Append(',').Append(frame.trialRightAmplitude.ToString()).Append(',').Append(frame.leftFrequency.ToString()).Append(',').Append(frame.leftRoughness.ToString()).Append(',').Append(frame.leftAmplitude.ToString()).Append(',').Append(frame.rightFrequency.ToString()).Append(',').Append(frame.rightRoughness.ToString()).Append(',').Append(frame.rightAmplitude.ToString()).Append(',').Append(frame.differenceBetweenSidesFrequency.ToString()).Append(',').Append(frame.differenceBetweenSidesRoughness.ToString()).Append(',').Append(frame.differenceBetweenSidesAmplitude.ToString()).Append('\n');
        //Debug.Log(result);

        result.ToString();

        string path = Application.dataPath + "/Data/" + frame.participantIndex + "/comparison_result.csv";
        var writer = new StreamWriter(path, true); // true for append, false for overwrite
        writer.Write(result);
        writer.Close();
        Debug.Log($"participant comparison result CSV file written to \"{path}\"");

        //---------- WRITE PER EXPERIMENT INDEX

        string experimentFolder = Application.dataPath + "/Data/Pairings/" + frame.experimentIndex;
        if(!System.IO.File.Exists(experimentFolder)){
            Directory.CreateDirectory(experimentFolder);
        }

        string experimentpath = experimentFolder + "/comparison_result.csv";

        if(!System.IO.File.Exists(experimentpath)){
            var titles  = new StringBuilder("Experiment Index, Participant Index, Trial Left Frequency, Trial Left Roughness, Trial Left Amplitude, Trial Right Frequency, Trial Right Roughness, Trial Right Amplitude, Left Frequency, Left Roughness, Left Amplitude, Right Frequency, Right Roughness, Right Amplitude, Visual Frequency Difference Between Sides, Haptic Frequency Difference Between Sides, Amplitude Difference Between Sides");
            titles.Append('\n');

            var writerTemp = new StreamWriter(experimentpath, false); // true for append, false for overwrite
            writerTemp.Write(titles);
            writerTemp.Close();
            Debug.Log($"new CSV file written to \"{experimentpath}\"");
        }
        
        var writer2 = new StreamWriter(experimentpath, true); // true for append, false for overwrite
        writer2.Write(result);
        writer2.Close();
        Debug.Log($"experiment comparison result CSV file written to \"{experimentpath}\"");
    }

    public void addToMultimodalCSV(MultimodalDataStruct frame){
        var titles  = new StringBuilder("Phase, Experiment Index, Participant Index, Trial Visual Frequency, Trial Haptic Frequency, Trial Amplitude, Participant Class, Response Time, Class Error");
        titles.Append('\n');
        
        var result = new StringBuilder("");
        result.Append(frame.phase.ToString()).Append(',').Append(frame.experimentIndex.ToString()).Append(',').Append(frame.participantIndex.ToString()).Append(',').Append(frame.trialVisualFrequency.ToString()).Append(',').Append(frame.trialHapticFrequency.ToString()).Append(',').Append(frame.trialAmplitude.ToString()).Append(',').Append(frame.participantClass.ToString()).Append(',').Append(frame.responseTime.ToString()).Append(',').Append(frame.errorClass.ToString()).Append('\n');
        result.ToString();

        //---------- WRITE PER PARTICIPANT INDEX

        string participantFolder = Application.dataPath + "/Data/Multimodal/" + frame.participantIndex;

        if(!System.IO.File.Exists(participantFolder)){
            Directory.CreateDirectory(participantFolder);
        }

        string participantpath = participantFolder + "/multimodal_result.csv";

        if(!System.IO.File.Exists(participantpath)){
            var writerTemp = new StreamWriter(participantpath, false); // true for append, false for overwrite
            writerTemp.Write(titles);
            writerTemp.Close();
            Debug.Log($"new CSV file written to \"{participantpath}\"");
        }

        var writer = new StreamWriter(participantpath, true); // true for append, false for overwrite
        writer.Write(result);
        writer.Close();
        Debug.Log($"participant multimodal result CSV file written to \"{participantpath}\"");

        //---------- WRITE PER EXPERIMENT INDEX

        string experimentFolder = Application.dataPath + "/Data/Multimodal/Pairings/" + frame.phase + frame.experimentIndex;
        if(!System.IO.File.Exists(experimentFolder)){
            Directory.CreateDirectory(experimentFolder);
        }

        string experimentpath = experimentFolder + "/multimodal_result.csv";

        if(!System.IO.File.Exists(experimentpath)){
            var writerTemp2 = new StreamWriter(experimentpath, false); // true for append, false for overwrite
            writerTemp2.Write(titles);
            writerTemp2.Close();
            Debug.Log($"new CSV file written to \"{experimentpath}\"");
        }
        
        var writer2 = new StreamWriter(experimentpath, true); // true for append, false for overwrite
        writer2.Write(result);
        writer2.Close();
        Debug.Log($"experiment multimodal result CSV file written to \"{experimentpath}\"");
    }
}
