using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Unity.VisualScripting;
using OVRSimpleJSON;

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

        ForceSetup.Initialize(EnvKey.APIVERSION, EnvKey.APPTOKEN, EnvKey.APIKEY, EnvKey.Tables.ConfigSetup, true);
        ConfigTable.Initialize(EnvKey.APIVERSION, EnvKey.APPTOKEN, EnvKey.APIKEY, EnvKey.Tables.UnityConfig, true);
        
        
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
        PreStudyTable.Initialize(EnvKey.APIVERSION, EnvKey.APPTOKEN, EnvKey.APIKEY, EnvKey.Tables.PreStudyVersion, true);
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
        PostStudyTable.Initialize(EnvKey.APIVERSION, EnvKey.APPTOKEN, EnvKey.APIKEY, EnvKey.Tables.PostStudyVersion, true);
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
        QuestionTable.Initialize(EnvKey.APIVERSION, EnvKey.APPTOKEN, EnvKey.APIKEY, "QuestionList" + DataRecorder.Instance.QuestionnaireID, true);
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


    private void OnDestroy()
    {
        if(MainAsyncThread != null)
        {
            MainAsyncThread.Abort();
        }
    }
}
