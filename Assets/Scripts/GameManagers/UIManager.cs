using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private TMP_Text DebugText;

    [SerializeField] private GameObject GameUICanvas;

    [SerializeField] private GameObject UnityConfigPanel;
    [SerializeField] private TMP_Dropdown UnityConfigDropdown;

    [SerializeField] private GameObject SurveyConfigPanel;
    [SerializeField] private TMP_Dropdown SurveyVersionDropdown;

    [SerializeField] private GameObject QuestionPanel;
    [SerializeField] public TMP_Text QuestionTitle;
    [SerializeField] public TMP_Text Question;

    [SerializeField] private Transform FollowTransform;

    private bool IsUiFollowing = false;
    private float FollowSpeed = 3f;

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
        UnityConfigDropdown.onValueChanged.AddListener(OnUnityConfigDDValue);
        SurveyVersionDropdown.onValueChanged.AddListener(OnSurveyVersionDDValue);
    }

    private void Update()
    {
        //OVRInput.Update();
        if(IsUiFollowing)
        {
            if (OVRInput.GetDown(OVRInput.Button.Start)) GameUICanvas.SetActive(!GameUICanvas.activeSelf);
            GameUICanvas.transform.position = Vector3.Lerp(GameUICanvas.transform.position, FollowTransform.position, Time.deltaTime * FollowSpeed);
            GameUICanvas.transform.rotation = Quaternion.Lerp(GameUICanvas.transform.rotation, FollowTransform.rotation, Time.deltaTime * FollowSpeed);
            GameUICanvas.transform.localScale = Vector3.Lerp(GameUICanvas.transform.localScale, FollowTransform.localScale, Time.deltaTime * FollowSpeed);
        }

        
    }

    private void FixedUpdate()
    {
        //OVRInput.FixedUpdate();
    }

    public void SetupUnityConfig(bool isShow)
    {
        if(isShow)
        {
            foreach (var record in DataManager.Instance.ConfigTable.LocalTable.itable)
            {
                UnityConfigDropdown.options.Add(new TMP_Dropdown.OptionData(record.Key));
            }
        }
        else
        {
            UISystemMessage($"[System]: Config Version {DataRecorder.Instance.UnityConfigVersion} is selected!");
            StartUnityConfigOnClick();
        }
        
    }

    public void StartUnityConfigOnClick()
    {
        if(DataRecorder.Instance.UnityConfigVersion == null || DataRecorder.Instance.UnityConfigVersion == "")
        {
            UISystemMessage("[Warning]: Please select a Unity Config!");
            return;
        }
        else
        {
            DataManager.Instance.LoadSurveyVersion();
        }
    }

    public void OnSurveyVersionLoaded()
    {
        UnityConfigPanel.SetActive(false);
        SurveyConfigPanel.SetActive(true);

        string[] SurveyVersions = DataManager.Instance.ConfigTable.LocalTable.itable[DataRecorder.Instance.UnityConfigVersion]["jSurveyConfigs"].Split("//");
        
        foreach(string SurveyVersionRid in SurveyVersions)
        {
            SurveyVersionDropdown.options.Add(new TMP_Dropdown.OptionData(SurveyVersionRid));
        }

        UISystemMessage("[System]: Survey Versions Loaded!");

        DataRecorder.Instance.SurveyVersion = "3";
        StartSurveyOnClick();
    }

    public void StartSurveyOnClick()
    {
        if(DataRecorder.Instance.SurveyVersion == null || DataRecorder.Instance.SurveyVersion == "")
        {
            UISystemMessage("[Warning]: Please select a Survey Version!");
            return;
        }
        else
        {
            string selectedSurveyVersion = DataRecorder.Instance.SurveyVersion;
            DataRecorder.Instance.PreStudyID = DataManager.Instance.SurveyVersionTable.LocalTable.itable[selectedSurveyVersion]["PreStudyID"];
            DataRecorder.Instance.PostStudyID = DataManager.Instance.SurveyVersionTable.LocalTable.itable[selectedSurveyVersion]["PostStudyID"];
            DataRecorder.Instance.QuestionnaireID = DataManager.Instance.SurveyVersionTable.LocalTable.itable[selectedSurveyVersion]["QuestionnaireID"];

            DataManager.Instance.LoadQuestionnaire();
        }
    }

    public void OnQuestionLoaded()
    {
        //UISystemMessage($"[System]: {QuestionManager.Instance.Questions.Count} Questions Loaded!");

        IsUiFollowing = true;

        SurveyConfigPanel.SetActive(false);
        QuestionPanel.SetActive(true);

        QuestionManager.Instance.LoadQuestion();
    }

    public void NextQuestionOnClick()
    {
        QuestionManager.Instance.CurrentQuestionIndex++;
        QuestionManager.Instance.LoadQuestion();
    }

    public void UISystemMessage(string message)
    {
        DebugText.text = message;
    }

    private void OnUnityConfigDDValue(int index)
    {
        DataRecorder.Instance.UnityConfigVersion = UnityConfigDropdown.options[index].text;
    }

    private void OnSurveyVersionDDValue(int index)
    {
        DataRecorder.Instance.SurveyVersion = SurveyVersionDropdown.options[index].text;
    }
}

