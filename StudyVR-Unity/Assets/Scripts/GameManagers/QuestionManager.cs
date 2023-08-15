using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class QuestionManager : MonoBehaviour
{
    public static QuestionManager Instance;

    private int PreStudyQuestionCount = 0;
    private int PostStudyQuestionCount = 0;
    private int QuestionnaireCount = 0;

    private string CurrentStudy = "";

    public int CurrentQuestionIndex = 0;

    private GameObject VerticalContainer;
    private GameObject advTextInput; // advanced text input
    private GameObject advToggle; // advanced toggle
    private GameObject advSlider; // advanced slider
    private GameObject advColorSlider; // advanced color slider

    public bool isQuestionnaireFinished = false;

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
        VerticalContainer = Resources.Load<GameObject>("Prefabs/VerticalContainer");
        advTextInput = Resources.Load<GameObject>("Prefabs/advTextInput");
        advToggle = Resources.Load<GameObject>("Prefabs/advToggle");
        advSlider = Resources.Load<GameObject>("Prefabs/advSlider");
        advColorSlider = Resources.Load<GameObject>("Prefabs/advColorSlider");
    }

    public void OnQuestionLoaded()
    {
        PreStudyQuestionCount = DataManager.Instance.PreStudyTable.LocalTable.itable.Count;
        PostStudyQuestionCount = DataManager.Instance.PostStudyTable.LocalTable.itable.Count;
        QuestionnaireCount = DataManager.Instance.QuestionTable.LocalTable.itable.Count;

        CurrentStudy = "PreStudy";
        UIManager.Instance.UISystemMessage($"[System]: {PreStudyQuestionCount + PostStudyQuestionCount + QuestionnaireCount} Questions Loaded!");

        UIManager.Instance.OnQuestionLoaded();
    }

    public void LoadQuestion(int counter = 0)
    {
        switch (CurrentStudy)
        {
            case "PreStudy":
                if(CurrentQuestionIndex >= PreStudyQuestionCount)
                {
                    CurrentStudy = "Questionnaire";
                    CurrentQuestionIndex = 0;
                }
                break;

            case "Questionnaire":
                if(CurrentQuestionIndex >= QuestionnaireCount)
                {
                    CurrentStudy = "PostStudy";
                    CurrentQuestionIndex = 0;
                }
                break;

            case "PostStudy":
                if(CurrentQuestionIndex >= PostStudyQuestionCount)
                {
                    isQuestionnaireFinished = true;
                    UIManager.Instance.UISystemMessage("[System]: Survey Finished!");
                    UIManager.Instance.FinishPanel.SetActive(true);
                    DataRecorder.Instance.UploadRecords();
                    return;
                }
                break;

            default:
                break;
        }

        switch (CurrentStudy)
        {
            case "PreStudy":
                SetQuestions(
                    DataManager.Instance.PreStudyTable.LocalTable.itable[CurrentQuestionIndex.ToString()]["ResponseType"],
                    DataManager.Instance.PreStudyTable.LocalTable.itable[CurrentQuestionIndex.ToString()]["ResponseConfig"],
                    DataManager.Instance.PreStudyTable.LocalTable.itable[CurrentQuestionIndex.ToString()]["QuestionString"],
                    null, null, null, 0
                    );
                break;

            case "Questionnaire":
                string responseType = DataManager.Instance.QuestionTable.LocalTable.itable[CurrentQuestionIndex.ToString()]["ResponseType"];
                string responseConfig = DataManager.Instance.QuestionTable.LocalTable.itable[CurrentQuestionIndex.ToString()]["ResponseConfig"];
                string questionString = DataManager.Instance.QuestionTable.LocalTable.itable[CurrentQuestionIndex.ToString()]["QuestionString"];
                string assetResponseType = null;
                string asset0 = null;
                string asset1 = null;
                if (DataManager.Instance.QuestionTable.LocalTable.itable[CurrentQuestionIndex.ToString()].ContainsKey("AssetResponseType"))
                {
                    assetResponseType = DataManager.Instance.QuestionTable.LocalTable.itable[CurrentQuestionIndex.ToString()]["AssetResponseType"];
                    if(assetResponseType == "Comparison")
                    {
                        asset0 = DataManager.Instance.QuestionTable.LocalTable.itable[CurrentQuestionIndex.ToString()]["Asset(0)"];
                        asset1 = DataManager.Instance.QuestionTable.LocalTable.itable[CurrentQuestionIndex.ToString()]["Asset(1)"];
                    }
                    else if(assetResponseType == "Evaluation")
                    {
                        asset0 = DataManager.Instance.QuestionTable.LocalTable.itable[CurrentQuestionIndex.ToString()]["Asset(0)"];
                    }
                }
                SetQuestions(responseType, responseConfig, questionString, assetResponseType, asset0, asset1, counter);
                break;

            case "PostStudy":
                SetQuestions(
                    DataManager.Instance.PostStudyTable.LocalTable.itable[CurrentQuestionIndex.ToString()]["ResponseType"],
                    DataManager.Instance.PostStudyTable.LocalTable.itable[CurrentQuestionIndex.ToString()]["ResponseConfig"],
                    DataManager.Instance.PostStudyTable.LocalTable.itable[CurrentQuestionIndex.ToString()]["QuestionString"],
                    null, null, null, 0
                    );
                break;

            default:
                break;
        }
    }

    private void SetQuestions(string ResponseType, string ResponseConfig, string QuestionString, string AssetResponseType = null, string Asset0 = null, string Asset1 = null, int counter = 0)
    {
        UIManager.Instance.QuestionTitle.text = $"{CurrentStudy}: Question#{CurrentQuestionIndex}";
        UIManager.Instance.Question.text = QuestionString;

        switch (ResponseType)
        {
            case "GodSpeedSlider":
                if(counter == 0) UIManager.Instance.ClearQuestion();
                if(counter < 4)
                {
                    List<string> godSpeedConfigs = GetQuestionConfig(ResponseConfig);

                    GameObject godSpeedVertical = GameObject.Find("VerticalContainer(Clone)");
                    if (godSpeedVertical == null) godSpeedVertical = Instantiate(VerticalContainer, UIManager.Instance.QuestionContainer.transform);

                    GameObject godSpeedSlider = Instantiate(advSlider, godSpeedVertical.transform);

                    QSComponent qsComponent = godSpeedSlider.GetComponent<QSComponent>();
                    qsComponent.StudyType = CurrentStudy;
                    qsComponent.QuestionID = CurrentQuestionIndex.ToString();
                    qsComponent.SetSlider(0, 5, godSpeedConfigs[0], godSpeedConfigs[1]);

                    CurrentQuestionIndex++;

                    if(CurrentQuestionIndex >= QuestionnaireCount)
                    {
                        CurrentQuestionIndex -= 1;
                        break;
                    }

                    LoadQuestion(counter + 1);
                }
                else
                {
                    CurrentQuestionIndex -= 1;
                }
                break;

            case "SingleSelect":
                UIManager.Instance.ClearQuestion();
                List<string> singleConfigs = GetQuestionConfig(ResponseConfig);

                int singleConfigIndex = 0;
                for (int i = 0; i < Mathf.CeilToInt(singleConfigs.Count / 4f); i++)
                {
                    GameObject vertical = Instantiate(VerticalContainer, UIManager.Instance.QuestionContainer.transform);

                    for (int j = 0; j < 4; j++)
                    {
                        GameObject selection = Instantiate(advToggle, vertical.transform);
                        selection.GetComponent<Toggle>().group = UIManager.Instance.QuestionToggleGroup;
                        selection.transform.Find("Label").GetComponent<TMP_Text>().text = singleConfigs[singleConfigIndex];

                        QSComponent tgQsComponent = selection.GetComponent<QSComponent>();
                        tgQsComponent.StudyType = CurrentStudy;
                        tgQsComponent.QuestionID = CurrentQuestionIndex.ToString();
                        //tgQsComponent.toggleMode = ToggleMode.Single;

                        singleConfigIndex++;

                        if (singleConfigIndex >= singleConfigs.Count) break;
                    }
                }
                break;

            case "MultiSelect":
                UIManager.Instance.ClearQuestion();
                List<string> multiConfigs = GetQuestionConfig(ResponseConfig);

                int multiConfigIndex = 0;
                for (int i = 0; i < Mathf.CeilToInt(multiConfigs.Count / 4f); i++)
                {
                    GameObject vertical = Instantiate(VerticalContainer, UIManager.Instance.QuestionContainer.transform);

                    for (int j = 0; j < 4; j++)
                    {
                        GameObject selection = Instantiate(advToggle, vertical.transform);
                        selection.transform.Find("Label").GetComponent<TMP_Text>().text = multiConfigs[multiConfigIndex];

                        QSComponent tgQsComponent = selection.GetComponent<QSComponent>();
                        tgQsComponent.StudyType = CurrentStudy;
                        tgQsComponent.QuestionID = CurrentQuestionIndex.ToString();
                        //tgQsComponent.toggleMode = ToggleMode.Multiple;

                        multiConfigIndex++;

                        if (multiConfigIndex >= multiConfigs.Count) break;
                    }
                }
                break;

            case "IntSlider":
                UIManager.Instance.ClearQuestion();
                List<string> SliderConfigs = GetQuestionConfig(ResponseConfig);

                GameObject sliderVertical = Instantiate(VerticalContainer, UIManager.Instance.QuestionContainer.transform);
                GameObject colorSlider = Instantiate(advColorSlider, sliderVertical.transform);

                QSComponent csqsComponent = colorSlider.GetComponent<QSComponent>();
                csqsComponent.StudyType = CurrentStudy;
                csqsComponent.QuestionID = CurrentQuestionIndex.ToString();

                GameObject slider = colorSlider.transform.Find("Slider").gameObject;

                foreach(string sliderConfig in SliderConfigs)
                {
                    string[] config = sliderConfig.Split(':');
                    switch (config[0])
                    {
                        case "min":
                            slider.GetComponent<Slider>().minValue = int.Parse(config[1]);
                            break;

                        case "max":
                            slider.GetComponent<Slider>().maxValue = int.Parse(config[1]);
                            break;

                        default:
                            break;
                    }
                }

                break;

            case "TextInput":
                UIManager.Instance.ClearQuestion();
                
                GameObject inputVertical = Instantiate(VerticalContainer, UIManager.Instance.QuestionContainer.transform);
                GameObject input = Instantiate(advTextInput, inputVertical.transform);
                QSComponent textQsComponent = input.GetComponent<QSComponent>();
                textQsComponent.StudyType = CurrentStudy;
                textQsComponent.QuestionID = CurrentQuestionIndex.ToString();
                break;

            default:
                break;
        }

        if(counter < 4)
        {
            switch (AssetResponseType)
            {
                case "Comparison":
                    ObjectSpawner.Instance.SpawnObject(Asset0, Asset1);
                    break;

                case "Evaluation":
                    ObjectSpawner.Instance.SpawnObject(Asset0);
                    break;

                default:
                    break;
            }
        }

        UIManager.Instance.RefreshLayout();
    }

    private List<string> GetQuestionConfig(string ResponseConfig)
    {
        List<string> QuestionConfig = new List<string>();

        string config = ResponseConfig.Replace("{", "").Replace("}", "");
        string[] configs = config.Split("\\n");

        foreach (string item in configs)
        {
            if(item != "" && item != null)
            { QuestionConfig.Add(item); }
        }

        return QuestionConfig;
    }


    public void RecordCurrentQuestion()
    {
        List<QSComponent> qsComponents = GetQSComponent(UIManager.Instance.QuestionContainer.transform);

        string responseString = "";

        bool isToggleMode = false;

        for(int i = 0; i < qsComponents.Count; i++)
        {
            switch (qsComponents[i].ResponseType)
            {
                case ResponseMode.Slider:
                    responseString += $"{qsComponents[i].QuestionID}:{qsComponents[i].GetSliderValue()}\\n";
                    break;
                case ResponseMode.ColorSlider:
                    responseString += $"{qsComponents[i].QuestionID}:{qsComponents[i].GetSliderValue()}\\n";
                    break;
                case ResponseMode.Toggle:
                    if (i == 0)
                    {
                        responseString += $"{qsComponents[i].QuestionID}:";
                    }

                    if (qsComponents[i].isToggled)
                    {
                        responseString += $"{qsComponents[i].GetToggleText()},";
                    }

                    isToggleMode = true;
                    break;
                case ResponseMode.Text:
                    if(CurrentStudy == "PreStudy" && CurrentQuestionIndex == 0)
                    {
                        DataRecorder.Instance.ParicipantID = qsComponents[i].GetText();
                    }
                    responseString += $"{qsComponents[i].QuestionID}:{qsComponents[i].GetText()}\\n";
                    break;
                default:
                    break;
            }

        }

        if(isToggleMode)
        {
            if (responseString.EndsWith(",")) responseString = responseString.Remove(responseString.Length - 1);

            responseString += "\\n";
        }

        DataRecorder.Instance.RecordQuestion(CurrentStudy, responseString);
    }

    private List<QSComponent> GetQSComponent(Transform parent, List<QSComponent> qsComponents = null)
    {
        if(qsComponents == null) qsComponents = new List<QSComponent>();

        foreach (Transform child in parent)
        {
            QSComponent qsComponent = null;

            if(child.TryGetComponent<QSComponent>(out qsComponent))
            {
                qsComponents.Add(qsComponent);
            }

            GetQSComponent(child, qsComponents);
        }

        return qsComponents;
    }
}
