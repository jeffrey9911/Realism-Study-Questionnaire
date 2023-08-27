using Oculus.Voice.Demo;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class UserController : MonoBehaviour
{
    public static UserController Instance;

    public bool IsResetUser = false;

    private float ResetTimer = 0f;

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
    }

}
