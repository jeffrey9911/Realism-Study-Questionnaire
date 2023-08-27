using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserController : MonoBehaviour
{
    public static UserController Instance;

    public bool IsResetUser = false;

    private float ResetTimer = 0f;

    [SerializeField] Transform TrackingSpace;
    [SerializeField] Transform TrackingCenter;
    [SerializeField] Transform OVRPlaceholder;

    private CharacterController characterController;


    private bool isTracked = false;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }

        OVRManager.display.displayFrequency = 120.0f;
        OVRPlugin.systemDisplayFrequency = 120.0f;
    }


    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (IsResetUser)
        {
            this.GetComponent<CharacterController>().enabled = false;
            ResetTimer += Time.deltaTime;
            this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(0f, this.transform.position.y, 0f), ResetTimer);

            if (ResetTimer >= 0.99)
            {
                this.transform.position = new Vector3(0f, this.transform.position.y, 0f);
                ResetTimer = 0f;
                IsResetUser = false;
                this.GetComponent<CharacterController>().enabled = true;
            }
        }

        
        Vector2 RightJoystick = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);

        if(RightJoystick.x != 0)
        {
            if(!isTracked)
            {
                foreach(Transform child in TrackingSpace)
                {
                    child.SetParent(OVRPlaceholder);
                }

                this.transform.position = new Vector3(TrackingCenter.transform.position.x, this.transform.position.y, TrackingCenter.transform.position.z);

                foreach(Transform child in OVRPlaceholder)
                {
                    child.SetParent(TrackingSpace);
                }

                //this.transform.RotateAround(TrackingPlayer.transform.position, Vector3.up, 45f * (RightJoystick.x > 0 ? 1.0f : -1.0f));
                this.transform.Rotate(Vector3.up, 45f);
                isTracked = true;
            }
        }
        else
        {
            isTracked = false;
        }



        //UIManager.Instance.UISystemMessage($"{TrackingPlayer.transform.position}");

        //characterController.center = new Vector3(TrackingPlayer.localPosition.x, 0, TrackingPlayer.localPosition.z);
    }
}
