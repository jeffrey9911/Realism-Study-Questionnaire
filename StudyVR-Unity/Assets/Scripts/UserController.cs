using System.Collections;
using System.Collections.Generic;
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
    }

    // Update is called once per frame
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
