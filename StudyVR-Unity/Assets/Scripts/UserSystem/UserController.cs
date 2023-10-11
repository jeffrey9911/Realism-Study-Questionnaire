using UnityEngine;

public class UserController : MonoBehaviour
{
    public static UserController Instance;

    private bool IsResetUser = false;
    private float ResetTimer = 0f;

    [SerializeField] private GuidePanelController RedGuide;
    [SerializeField] private GuidePanelController BlueGuide;
    [SerializeField] private GuidePanelController GrayGuide;

    public NavigOrigController naviController;
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
        if (IsResetUser) ResetingUser();

        if(OVRInput.GetDown(OVRInput.Button.Two))
        {
            OVRManager.display.RecenterPose();
        }
    }

    public void ResetUser(int NumObj)
    {
        IsResetUser = true;

        UIManager.Instance.ResetQuestionPanel();

        switch (NumObj)
        {
            case 1:
                GrayGuide.TurnOn();
                naviController.NaviArrow1.StartNavi(new Vector3(0f, 0f, 4f));
                UIManager.Instance.UISystemMessage("[Tip]: Click Left Menu (≡) to Hide/Display the survey panel. Take your time~");
                break;

            case 2:
                BlueGuide.TurnOn();
                RedGuide.TurnOn();

                naviController.NaviArrow1.StartNavi(new Vector3(-4f, 0f, 4f));
                naviController.NaviArrow2.StartNavi(new Vector3(4f, 0f, 4f));
                UIManager.Instance.UISystemMessage("[Tip]: Click Left Menu (≡) to Hide/Display the survey panel. Take your time~");
                break;

            default:
                break;
        }
    }


    private void ResetingUser()
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

    private void OnTriggerEnter(Collider other)
    {
        switch (other.name)
        {
            case "BlueBase":
                naviController.NaviArrow1.StopNavi();
                break;

            case "RedBase":
                naviController.NaviArrow2.StopNavi();
                break;
            
            case "CentreBase":
                naviController.NaviArrow1.StopNavi();
                break;

            default:
                break;
        }
    }

    [ContextMenu("TEST RUN")]
    public void TestRun()
    {
        naviController.NaviArrow1.StartNavi(new Vector3(-4f, 0f, 4f));
                naviController.NaviArrow2.StartNavi(new Vector3(4f, 0f, 4f));
    }

}
