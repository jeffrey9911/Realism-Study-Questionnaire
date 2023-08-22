using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataRecorder : MonoBehaviour
{
    public static DataRecorder Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }
    }

    public string UnityConfigVersion;
    public string SurveyVersion;
    public string PreStudyID;
    public string PostStudyID;
    public string QuestionnaireID;

    public string PaticipantID;

    public List<string> PreStudyResponse = new List<string>();
    public List<string> QuestionnaireResponse = new List<string>();
    public List<string> PostStudyResponse = new List<string>();

    public void RecordQuestion(string studyName, string responseString)
    {
        switch (studyName)
        {
            case "PreStudy":
                PreStudyResponse.Add(responseString);
                break;

            case "Questionnaire":
                QuestionnaireResponse.Add(responseString);
                break;

            case "PostStudy":
                PostStudyResponse.Add(responseString);
                break;

            default:
                break;
        }
    }

    public void DisplayRecords()
    {
        Debug.Log($"Unity Config Version: {UnityConfigVersion}\n" +
            $"Survey Version: {SurveyVersion}\n" +
            $"PreStudyID: {PreStudyID}\n" +
            $"QuestionnaireID: {QuestionnaireID}\n" +
            $"PostStudyID: {PostStudyID}\n" +
            $"================== Pre-Study ==================\n");
        foreach (string response in PreStudyResponse)
        {
            Debug.Log(response);
        }

        Debug.Log($"================== Questionnaire ==================\n");
        foreach (string response in QuestionnaireResponse)
        {
            Debug.Log(response);
        }

        Debug.Log($"================== Post-Study ==================\n");
        foreach (string response in PostStudyResponse)
        {
            Debug.Log(response);
        }
    }

    public void UploadRecords()
    {
        string preR = "{\\n";
        string quesR = "{\\n";
        string postR = "{\\n";

        int randomNum = Random.Range(1000, 10000);

        string currentDateTimeStr = $"{(System.DateTime.Now.Year % 100):00}" +
            $"{(System.DateTime.Now.Month):00}" +
            $"{(System.DateTime.Now.Day):00}" +
            $"{(System.DateTime.Now.Hour):00}" +
            $"{(System.DateTime.Now.Minute):00}" +
            $"{(System.DateTime.Now.Second):00}";

        PaticipantID = $"{randomNum}{currentDateTimeStr}";
        
        foreach (string response in PreStudyResponse)
        {
            preR += response;
        }
        preR += "}";

        foreach (string response in QuestionnaireResponse)
        {
            quesR += response;
        }
        quesR += "}";

        foreach (string response in PostStudyResponse)
        {
            postR += response;
        }
        postR += "}";

        Debug.Log(preR);
        Debug.Log(quesR);
        Debug.Log(postR);

        DataManager.Instance.UploadResponse(preR, quesR, postR);
    }
}
