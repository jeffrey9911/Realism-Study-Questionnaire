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
}
