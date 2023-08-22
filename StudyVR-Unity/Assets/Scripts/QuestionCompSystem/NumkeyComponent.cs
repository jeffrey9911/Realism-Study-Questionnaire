using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumkeyComponent : MonoBehaviour
{
    [SerializeField] private char key;

    [SerializeField] private QSComponent qsComponent;

    public void KeyOnClick()
    {
        qsComponent.KeyOnClick(key);
    }
}
