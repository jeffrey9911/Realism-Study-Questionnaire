using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Unity.VisualScripting;
using OVRSimpleJSON;
using AirtableUnity.PX.Model;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    private Thread MainAsyncThread;
    private SynchronizationContext MainThreadContext;

    public IAirtable ForceSetup;
    public IAirtable ConfigTable;
    public IAirtable SurveyVersionTable;
    public IAirtable PreStudyTable;
    public IAirtable PostStudyTable;
    public IAirtable QuestionTable;
    public IAirtable UABTable;
    public IAirtable ResponseTable;

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

    private void Start()
    {
        ForceSetup = this.AddComponent<IAirtable>();
        ConfigTable = this.AddComponent<IAirtable>();
        SurveyVersionTable = this.AddComponent<IAirtable>();
        PreStudyTable = this.AddComponent<IAirtable>();
        PostStudyTable = this.AddComponent<IAirtable>();
        QuestionTable = this.AddComponent<IAirtable>();
        UABTable = this.AddComponent<IAirtable>();
        ResponseTable = this.AddComponent<IAirtable>();


        ForceSetup.Initialize(EnvKey.APIVERSION, EnvKey.APPTOKEN, EnvKey.APIKEY, EnvKey.Tables.ConfigSetup, true);
        ConfigTable.Initialize(EnvKey.APIVERSION, EnvKey.APPTOKEN, EnvKey.APIKEY, EnvKey.Tables.UnityConfig, true);
        
        UABTable.Initialize(EnvKey.APIVERSION, EnvKey.APPTOKEN, EnvKey.APIKEY, EnvKey.Tables.UABList, false);
        
        MainAsyncThread = new Thread(GetUnityConfig);
        MainAsyncThread.Start();

        MainThreadContext = SynchronizationContext.Current;
    }

    private void GetUnityConfig()
    {
        MainThreadContext.Post(_ => { UIManager.Instance.UISystemMessage("[System]: Loading Unity Config..."); }, null);
        while (!ConfigTable.isLocalTableAvailable || !ForceSetup.isLocalTableAvailable)
        {
            Thread.Sleep(1000);
        }
        MainThreadContext.Post(_ => { UIManager.Instance.UISystemMessage("[System]: Unity Config Loaded!"); }, null);

        MainThreadContext.Post(
            _ =>
            {
                foreach (var record in ForceSetup.LocalTable.itable)
                {
                    int forceSetup = -1;
                    if (int.TryParse(record.Key, out forceSetup))
                    {
                        if (forceSetup >= 0)
                        {
                            DataRecorder.Instance.UnityConfigVersion = forceSetup.ToString();
                            UIManager.Instance.SetupUnityConfig(false);
                            return;
                        }

                        if(forceSetup == -2)
                        {
                            UIManager.Instance.FinishPanel.SetActive(true);
                            QuestionManager.Instance.isQuestionnaireFinished = true;
                            UIManager.Instance.UISystemMessage("[System]: Survey temporarily closed. Please await further notice.");
                            return;
                        }
                    }
                }
                UIManager.Instance.SetupUnityConfig(true);
                //JSONObject
            }, null
            );
    }

    public void LoadSurveyVersion()
    {
        SurveyVersionTable.Initialize(EnvKey.APIVERSION, EnvKey.APPTOKEN, EnvKey.APIKEY, EnvKey.Tables.SurveyVersion, true);
        MainAsyncThread = new Thread(GetSurveyVersion);
        MainAsyncThread.Start();
    }

    private void GetSurveyVersion()
    {
        MainThreadContext.Post(_ => { UIManager.Instance.UISystemMessage("[System]: Loading Survey Versions..."); }, null);
        while (!SurveyVersionTable.isLocalTableAvailable)
        {
            Thread.Sleep(1000);
        }
        MainThreadContext.Post(_ => { 
            UIManager.Instance.OnSurveyVersionLoaded();
        }, null);
    }

    public void LoadQuestionnaire()
    {
        PreStudyTable.Initialize(EnvKey.APIVERSION, EnvKey.APPTOKEN, EnvKey.APIKEY, "PreStudyV" + DataRecorder.Instance.PreStudyID, true);
        MainAsyncThread = new Thread(GetPreStudy);
        MainAsyncThread.Start();
    }

    private void GetPreStudy()
    {
        MainThreadContext.Post(_ => { UIManager.Instance.UISystemMessage("[System]: Loading Pre-Study Questions..."); }, null);
        while(!PreStudyTable.isLocalTableAvailable)
        {
            Thread.Sleep(1000);
        }

        MainThreadContext.Post(_ => { 
            LoadPostStudy();
        }, null);
    }

    private void LoadPostStudy()
    {
        PostStudyTable.Initialize(EnvKey.APIVERSION, EnvKey.APPTOKEN, EnvKey.APIKEY, "PostStudyV" + DataRecorder.Instance.PostStudyID, true);
        MainAsyncThread = new Thread(GetPostStudy);
        MainAsyncThread.Start();
    }

    private void GetPostStudy()
    {
        MainThreadContext.Post(_ => { UIManager.Instance.UISystemMessage("[System]: Loading Post-Study Questions..."); }, null);
        while (!PreStudyTable.isLocalTableAvailable)
        {
            Thread.Sleep(1000);
        }

        MainThreadContext.Post(_ =>
        {
            LoadQuestion();
        }, null);
    }

    private void LoadQuestion()
    {
        QuestionTable.Initialize(EnvKey.APIVERSION, EnvKey.APPTOKEN, EnvKey.APIKEY, "QuestionnaireV" + DataRecorder.Instance.QuestionnaireID, true);
        MainAsyncThread = new Thread(GetQuestion);
        MainAsyncThread.Start();
    }

    private void GetQuestion()
    {
        MainThreadContext.Post(_ => { UIManager.Instance.UISystemMessage("[System]: Loading Questions..."); }, null);
        while (!QuestionTable.isLocalTableAvailable)
        {
            Thread.Sleep(1000);
        }

        MainThreadContext.Post(_ => {
            UIManager.Instance.UISystemMessage("[System]: All Questions Loaded!");
            QuestionManager.Instance.OnQuestionLoaded();
            Destroy(DataManager.Instance.ConfigTable);
            Destroy(DataManager.Instance.SurveyVersionTable);
        }, null);
    }

    public void UploadResponse(string prestudyR, string questionR, string poststudyR)
    {
        UIManager.Instance.UISystemMessage("[System]: Survey Finished! Uploading Your Response... Do Not Quit");
        ResponseTable.Initialize(EnvKey.APIVERSION, EnvKey.APPTOKEN, EnvKey.APIKEY, EnvKey.Tables.ResponseVersion, false);
        string newdata = $"{{\"fields\":{{\"$Response ID\":\"{DataRecorder.Instance.PaticipantID}" +
            $"\",\"PreStudy ID\":\"{DataRecorder.Instance.PreStudyID}" +
            $"\",\"PreStudy Response\":\"{prestudyR}" +
            $"\",\"Questionnaire ID\":\"{DataRecorder.Instance.QuestionnaireID}" +
            $"\",\"Questionnaire Response\":\"{questionR}" +
            $"\",\"PostStudy ID\":\"{DataRecorder.Instance.PostStudyID}" +
            $"\",\"PostStudy Response\":\"{poststudyR}" +
            $"\"}}}}";

        ResponseTable.CreateRecord(newdata, OnCreateFinish);
    }

    private void OnCreateFinish(BaseRecord<BaseField> record)
    {
        UIManager.Instance.UISystemMessage("[System]: Response Uploaded! Thanks for participating!");
    }


    private void OnDestroy()
    {
        if(MainAsyncThread != null)
        {
            MainAsyncThread.Abort();
        }
    }
}
