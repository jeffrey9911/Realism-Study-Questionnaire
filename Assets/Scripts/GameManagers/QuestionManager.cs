using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class QuestionManager : MonoBehaviour
{
    public static QuestionManager Instance;

    public int CurrentQuestionIndex = 0;

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

    public void OnQuestionLoaded()
    {
        Debug.Log("=============== Pre study questions ===============");
        
        foreach (var item in DataManager.Instance.PreStudyTable.LocalTable.itable)
        {
            Debug.Log(item);
        }
        Debug.Log("=============== QUESTIONS ===============");

        foreach(var item in DataManager.Instance.QuestionTable.LocalTable.itable)
        {
            Debug.Log(item);
        }
        Debug.Log("=============== Post study questions ===============");
        foreach (var item in DataManager.Instance.PostStudyTable.LocalTable.itable)
        {
            Debug.Log(item);
        }

        UIManager.Instance.OnQuestionLoaded();
    }

    public void LoadQuestion()
    {
        /*
        if(CurrentQuestionIndex < Questions.Count)
        {
            UIManager.Instance.QuestionTitle.text = $"Question{CurrentQuestionIndex}:";
            UIManager.Instance.Question.text = Questions[CurrentQuestionIndex];
        }
        else
        {
            UIManager.Instance.UISystemMessage("[System]: Questionnaire Finished! No more questions!");
        }
        */
    }
}
