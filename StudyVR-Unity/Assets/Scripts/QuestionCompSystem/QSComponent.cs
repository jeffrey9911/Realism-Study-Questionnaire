using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public enum ResponseMode
{
    Slider,
    ColorSlider,
    Toggle,
    Text
}

public enum ToggleMode
{
    Single,
    Multiple
}

public class QSComponent : MonoBehaviour
{
    public string StudyType;
    public string QuestionID;

    public ResponseMode ResponseType;

    #region Slider
    [SerializeField] [HideInInspector] private Slider Slider;
    [SerializeField] [HideInInspector] private TMP_Text LeftText;
    [SerializeField] [HideInInspector] private TMP_Text RightText;
    [SerializeField] [HideInInspector] private TMP_Text HandleText;

    public void SetSlider(int min, int max, string left = "", string right = "")
    {
        Slider.minValue = min;
        Slider.maxValue = max;

        Slider.value = Mathf.CeilToInt((min + max) / 2f);

        LeftText.text = left;
        RightText.text = right;
    }

    public int GetSliderValue()
    {
        return (int)Slider.value;
    }

    public void OnSliderValueChanged(float value)
    {
        HandleText.text = ((int)value).ToString(); 
    }
    #endregion



    #region Text
    [SerializeField] [HideInInspector] private TMP_InputField InputField;
    
    public void KeyOnClick(char key)
    {
        if(key == '\b')
        {
            if(InputField.text.Length > 0)
            {
                InputField.text = InputField.text.Substring(0, InputField.text.Length - 1);
            }

            return;
        }

        InputField.text += key;
    }

    public string GetText()
    {
        return InputField.text;
    }
    #endregion

    #region Toggle
    [SerializeField] [HideInInspector] private Toggle toggle;
    [SerializeField] [HideInInspector] private TMP_Text toggleText;

    //public ToggleMode toggleMode;
    public bool isToggled { get { return toggle.isOn; } }
    public string GetToggleText()
    {
        return toggleText.text;
    }
    
    #endregion

}
