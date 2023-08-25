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

    [SerializeField] public GameObject FinishPanel;
    [SerializeField] public GameObject ClosePanel;

    [SerializeField] private GameObject TutorialPanel;

    [SerializeField] private Transform FollowTransform;

    public GameObject QuestionContainer;
    public ToggleGroup QuestionToggleGroup;

    private bool IsUiFollowing = false;
    private bool IsQuestionPanelTransformed = false;
    private float FollowSpeed = 3f;

    private Vector3 LControllerPosRecord = new Vector3(0f, 0f, 0f);
    private Vector3 RControllerPosRecord = new Vector3(0f, 0f, 0f);
    //private Quaternion ControllerRotRecord = new Quaternion(0f, 0f, 0f, 0f);
    private bool IsControllerPosRecorded = false;
    private Vector3 FollowUIScale = new Vector3(0.0035f, 0.0035f, 0.0035f);
    private bool isDisplayGUI = true;

    [SerializeField] private Transform LeftHandAnchor;
    [SerializeField] private Transform RightHandAnchor;
    [SerializeField] private Transform CentreEye;
    [SerializeField] private GameObject MovePanel;
    [SerializeField] private GameObject ScalePanel;

    

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
        QuestionPanel.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 0f);
    }

    private void Update()
    {
        if(IsUiFollowing)
        {
            if (OVRInput.GetDown(OVRInput.Button.Start))
            {
                isDisplayGUI = !isDisplayGUI;
                TutorialPanel.SetActive(isDisplayGUI);
                GameUICanvas.SetActive(isDisplayGUI);
                
            }

            bool isLeftHandTrigger = OVRInput.Get(OVRInput.Button.PrimaryHandTrigger);
            bool isRightHandTrigger = OVRInput.Get(OVRInput.Button.SecondaryHandTrigger);

            if (isLeftHandTrigger || isRightHandTrigger)
            {
                MovePanel.SetActive(true);
                ScalePanel.SetActive(false);

                if (!IsControllerPosRecorded)
                {
                    LControllerPosRecord = LeftHandAnchor.position;
                    RControllerPosRecord = RightHandAnchor.position;
                    IsControllerPosRecorded = true;
                }

                if(isLeftHandTrigger && isRightHandTrigger)
                {
                    MovePanel.SetActive(false);
                    ScalePanel.SetActive(true);

                    float dDistance = Vector3.Distance(LeftHandAnchor.position, RightHandAnchor.position) - Vector3.Distance(LControllerPosRecord, RControllerPosRecord);
                    dDistance *= 0.006f;

                    FollowUIScale += new Vector3(dDistance, dDistance, dDistance);
                }
                else if(isLeftHandTrigger)
                {
                    FollowTransform.position += LeftHandAnchor.position - LControllerPosRecord;
                }
                else if (isRightHandTrigger)
                {
                    FollowTransform.position += RightHandAnchor.position - RControllerPosRecord;
                }

                LControllerPosRecord = LeftHandAnchor.position;
                RControllerPosRecord = RightHandAnchor.position;
            }
            else
            {
                MovePanel.SetActive(false);
                ScalePanel.SetActive(false);
                IsControllerPosRecorded = false;
            }

            Quaternion lookatRot = Quaternion.LookRotation(CentreEye.position - FollowTransform.position, Vector3.up);
            lookatRot *= Quaternion.Euler(0f, 180f, 0f);
            FollowTransform.rotation = lookatRot;

            GameUICanvas.transform.position = Vector3.Lerp(GameUICanvas.transform.position, FollowTransform.position, Time.deltaTime * FollowSpeed);
            GameUICanvas.transform.rotation = Quaternion.Lerp(GameUICanvas.transform.rotation, FollowTransform.rotation, Time.deltaTime * FollowSpeed);
            GameUICanvas.transform.localScale = Vector3.Lerp(GameUICanvas.transform.localScale, FollowUIScale, Time.deltaTime * FollowSpeed);

            if (!IsQuestionPanelTransformed)
            {
                QuestionPanel.GetComponent<RectTransform>().offsetMax = Vector2.Lerp(QuestionPanel.GetComponent<RectTransform>().offsetMax,
                    new Vector2(0f, 40f), Time.deltaTime * FollowSpeed);

                if(QuestionPanel.GetComponent<RectTransform>().offsetMax.y >= 39.9f)
                {
                    QuestionPanel.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 40f);
                    IsQuestionPanelTransformed = true;
                }
            }
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

        //DataRecorder.Instance.SurveyVersion = "0";
        //StartSurveyOnClick();
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

    public void ClearQuestion()
    {
        while (QuestionContainer.transform.childCount > 0)
        {
            DestroyImmediate(QuestionContainer.transform.GetChild(0).gameObject);
        }
    }

    public void RefreshLayout()
    {
        RebuildLayoutGroups(QuestionContainer.transform);
    }

    private void RebuildLayoutGroups(Transform parent)
    {
        LayoutGroup layoutGroup = parent.GetComponent<LayoutGroup>();
        if(layoutGroup != null)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(layoutGroup.GetComponent<RectTransform>());
        }

        for(int i = 0; i < parent.childCount; i++)
        {
            RebuildLayoutGroups(parent.GetChild(i));
        }
    }

    public void OnQuestionLoaded()
    {
        IsUiFollowing = true;

        SurveyConfigPanel.SetActive(false);
        FinishPanel.SetActive(false);
        ClosePanel.SetActive(false);
        QuestionPanel.SetActive(true);

        ObjectManager.Instance.StartPreload();
        QuestionManager.Instance.LoadQuestion();
    }

    [ContextMenu("Next Question")]
    public void NextQuestionOnClick()
    {
        QuestionManager.Instance.RecordCurrentQuestion();
        QuestionManager.Instance.CurrentQuestionIndex++;
        QuestionManager.Instance.LoadQuestion();
    }

    [ContextMenu("Restart Study")]
    public void RestartOnClick()
    {
        DataRecorder.Instance.ClearRecord();
        QuestionManager.Instance.OnQuestionLoaded();
        ObjectSpawner.Instance.isStudyFinished = false;
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

