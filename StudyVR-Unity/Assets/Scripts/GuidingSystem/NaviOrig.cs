using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class NaviOrig : MonoBehaviour
{
    [SerializeField] private GameObject Rotator;

    private bool IsNavi = false;

    private Vector3 NaviPos = Vector3.zero;

    private void Update()
    {
        if(IsNavi)
        {
            NaviPos.y = 0f;
            Vector3 thisPos = new Vector3(this.transform.position.x, 0f, this.transform.position.z);

            Rotator.transform.rotation = Quaternion.LookRotation(NaviPos - thisPos);

        }
    }

    public void StartNavi(Vector3 navPos)
    {
        IsNavi = true;
        NaviPos = navPos;
        this.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
    }

    public void StopNavi()
    {
        IsNavi = false;
        this.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
    }
}
