using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionManager : MonoBehaviour
{
    public static QuestionManager Instance;

    public List<string> Questions = new List<string>();

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
        for(int i = 0; i < DataManager.Instance.PreStudyTable.LocalTable.itable[DataRecorder.Instance.PreStudyID].Count; i++)
        {
            if (DataManager.Instance.PreStudyTable.LocalTable.itable[DataRecorder.Instance.PreStudyID].ContainsKey(i.ToString()))
            {
                Questions.Add(DataManager.Instance.PreStudyTable.LocalTable.itable[DataRecorder.Instance.PreStudyID][i.ToString()]);
            }
        }

        /*
        int Length = Questions.Count;

        foreach(var Question in DataManager.Instance.QuestionTable.LocalTable.itable)
        {
            int questionIndex = -1;
            if(int.TryParse(Question.Key, out questionIndex))
            {
                Questions.Insert(Length + questionIndex, Question.Value["QuestionString"]);
            }
        }
        Length = Questions.Count;
        */

        for(int i = 0; i < DataManager.Instance.QuestionTable.LocalTable.itable.Count; i++)
        {
            if(DataManager.Instance.QuestionTable.LocalTable.itable.ContainsKey(i.ToString()))
            {
                Questions.Add(DataManager.Instance.QuestionTable.LocalTable.itable[i.ToString()]["QuestionString"]);
            }
        }

        /*
        foreach(var PostStudyQuestion in DataManager.Instance.PostStudyTable.LocalTable.itable[DataRecorder.Instance.PostStudyID])
        {
            int questionIndex = -1;
            if(int.TryParse(PostStudyQuestion.Key, out questionIndex))
            {
                Questions.Insert(Length + questionIndex, PostStudyQuestion.Value);
            }
        }
        */

        for(int i = 0; i < DataManager.Instance.PostStudyTable.LocalTable.itable[DataRecorder.Instance.PostStudyID].Count; i++)
        {
            if (DataManager.Instance.PostStudyTable.LocalTable.itable[DataRecorder.Instance.PostStudyID].ContainsKey(i.ToString()))
            {
                Questions.Add(DataManager.Instance.PostStudyTable.LocalTable.itable[DataRecorder.Instance.PostStudyID][i.ToString()]);
            }
        }

        Debug.Log("===============QUESTIONS===============");
        
        foreach (var item in Questions)
        {
            Debug.Log(item);
        }
        Debug.Log("===============QUESTIONS===============");

        UIManager.Instance.OnQuestionLoaded();
    }

    public void LoadQuestion()
    {
        if(CurrentQuestionIndex < Questions.Count)
        {
            UIManager.Instance.QuestionTitle.text = $"Question{CurrentQuestionIndex}:";
            UIManager.Instance.Question.text = Questions[CurrentQuestionIndex];
        }
        else
        {
            UIManager.Instance.UISystemMessage("[System]: Questionnaire Finished! No more questions!");
        }
    }
}
