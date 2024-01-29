using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleUITrans : MonoBehaviour
{
    private Vector3 LControllerPosRecord = new Vector3(0f, 0f, 0f);
    private Vector3 RControllerPosRecord = new Vector3(0f, 0f, 0f);

    private bool IsControllerPosRecorded = false;

    private float FollowSpeed = 3f;

    [SerializeField] private GameObject GameUICanvas;

    [SerializeField] private Transform FollowTransform;

    [SerializeField] private Transform LeftHandAnchor;
    [SerializeField] private Transform RightHandAnchor;
    [SerializeField] private Transform CentreEye;
    [SerializeField] private GameObject MovePanel;
    [SerializeField] private GameObject ScalePanel;

    private Vector3 FollowUIScale = new Vector3(0.0035f, 0.0035f, 0.0035f);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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

            if (isLeftHandTrigger && isRightHandTrigger)
            {
                MovePanel.SetActive(false);
                ScalePanel.SetActive(true);

                float dDistance = Vector3.Distance(LeftHandAnchor.position, RightHandAnchor.position) - Vector3.Distance(LControllerPosRecord, RControllerPosRecord);
                dDistance *= 0.006f;

                FollowUIScale += new Vector3(dDistance, dDistance, dDistance);
            }
            else if (isLeftHandTrigger)
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

        
    }
}
