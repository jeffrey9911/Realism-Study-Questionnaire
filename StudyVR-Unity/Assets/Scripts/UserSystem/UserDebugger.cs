using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UserDebugger : MonoBehaviour
{
    [SerializeField] GameObject DebugCanvas;
    [SerializeField] TMP_InputField DebugInputField;

    [SerializeField] List<GameObject> DebugObjects = new List<GameObject>();

    private void Update()
    {
        if(OVRInput.Get(OVRInput.Button.PrimaryThumbstick) && OVRInput.GetDown(OVRInput.Button.SecondaryThumbstick))
        {
            UIManager.Instance.UISystemMessage("[Debugger]: Debug Canvas Toggled");
            DebugCanvas.SetActive(!DebugCanvas.activeSelf);
        }

        if(DebugCanvas.activeSelf)
        {
            UpdateDebugger();
        }
    }

    private void UpdateDebugger()
    {
        string debugText = "Positions:\n";
        foreach(GameObject debugObject in DebugObjects)
        {
            debugText += $"{debugObject.name} : {debugObject.transform.position}\n";
        }

        DebugInputField.text = debugText;
    }
}
