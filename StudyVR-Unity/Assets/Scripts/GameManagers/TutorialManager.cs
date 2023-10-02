using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    private Vector3 LeftAnchorPosition;
    private Vector3 RightAnchorPosition;
    private Quaternion LeftAnchorRotation;
    private Quaternion RightAnchorRotation;

    [SerializeField] private Transform LeftHandTrans;
    [SerializeField] private Transform RightHandTrans;
    [SerializeField] private Transform CentreEyeTrans;

    [SerializeField] private Transform LeftHandUI;
    [SerializeField] private Transform RightHandUI;

    private float FollowSpeed = 5.0f;

    private void Update()
    {
        Vector3 LeftPosAnchor = new Vector3(LeftHandTrans.position.x - CentreEyeTrans.position.x, 0f, LeftHandTrans.position.z - CentreEyeTrans.position.z);
        LeftAnchorPosition = LeftHandTrans.position + LeftPosAnchor.normalized * 0.2f + new Vector3(0f, 0.1f, 0f);

        Vector3 RightPosAnchor = new Vector3(RightHandTrans.position.x - CentreEyeTrans.position.x, 0f, RightHandTrans.position.z - CentreEyeTrans.position.z);
        RightAnchorPosition = RightHandTrans.position + RightPosAnchor.normalized * 0.2f + new Vector3(0f, 0.1f, 0f);

        LeftAnchorRotation = LeftHandTrans.rotation * Quaternion.Euler(30f, 0f, 0f);
        RightAnchorRotation = RightHandTrans.rotation * Quaternion.Euler(30f, 0f, 0f);

        LeftHandUI.position = Vector3.Lerp(LeftHandUI.position, LeftAnchorPosition, Time.deltaTime * FollowSpeed);
        RightHandUI.position = Vector3.Lerp(RightHandUI.position, RightAnchorPosition, Time.deltaTime * FollowSpeed);

        LeftHandUI.rotation = Quaternion.Lerp(LeftHandUI.rotation, LeftAnchorRotation, Time.deltaTime * FollowSpeed);
        RightHandUI.rotation = Quaternion.Lerp(RightHandUI.rotation, RightAnchorRotation, Time.deltaTime * FollowSpeed);


    }
}
