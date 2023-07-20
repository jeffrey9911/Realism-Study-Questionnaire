using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public static ObjectSpawner Instance;

    [SerializeField] private GameObject BlueBase;
    [SerializeField] private GameObject RedBase;
    [SerializeField] private GameObject CentreBase;

    private Material BlueMaterial;
    private Material RedMaterial;
    private Material CentreMaterial;

    private Color BLUE = new Color(80/255f, 192/255f, 1.0f, 1.0f);
    private Color RED = new Color(200/255f, 97/255f, 87/255f, 1.0f);
    private Color NEUTRAL = new Color(120/255f, 104/255f, 119/255f, 1.0f);
    private Color ORIGIN = new Color(1.0f, 1.0f, 1.0f, 1.0f);

    private bool isComparison = false;
    private bool isEvaluation = false;

    private GameObject Asset0;
    private GameObject Asset1;

    private string Asset0rid;
    private string Asset1rid;

    private string CurrentMode = "";

    private float DownloadingTimer = 0f;
    public bool IsDownloading = false;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }
    }

    private void Start()
    {
        BlueMaterial = BlueBase.GetComponent<MeshRenderer>().material;
        RedMaterial = RedBase.GetComponent<MeshRenderer>().material;
        CentreMaterial = CentreBase.GetComponent<MeshRenderer>().material;

        BlueMaterial.color = ORIGIN;
        RedMaterial.color = ORIGIN;
        CentreMaterial.color = ORIGIN;

        BlueBase.transform.position = new Vector3(BlueBase.transform.position.x, 0.0f, BlueBase.transform.position.z);
        RedBase.transform.position = new Vector3(RedBase.transform.position.x, 0.0f, RedBase.transform.position.z);
        CentreBase.transform.position = new Vector3(CentreBase.transform.position.x, 0.0f, CentreBase.transform.position.z);
    }

    private void Update()
    {
        if(IsDownloading)
        {
            DownloadingTimer += Time.deltaTime;
            UIManager.Instance.UISystemMessage($"[System]: Downloading assets... Please wait... [{(int)DownloadingTimer} sec]");
        }

        if(QuestionManager.Instance.isQuestionnaireFinished)
        {
            isComparison = false;
            isEvaluation = false;

            Destroy(Asset0);
            Destroy(Asset1);

            BlueBase.transform.position = Vector3.Lerp(
                BlueBase.transform.position,
                new Vector3(BlueBase.transform.position.x, 0f, BlueBase.transform.position.z),
                Time.deltaTime
                );

            BlueBase.GetComponent<MeshRenderer>().material.color = Color.Lerp(
                 BlueBase.GetComponent<MeshRenderer>().material.color,
                 ORIGIN,
                 Time.deltaTime
                 );

            RedBase.transform.position = Vector3.Lerp(
                RedBase.transform.position,
                new Vector3(RedBase.transform.position.x, 0f, RedBase.transform.position.z),
                Time.deltaTime
                );

            RedBase.GetComponent<MeshRenderer>().material.color = Color.Lerp(
                 RedBase.GetComponent<MeshRenderer>().material.color,
                 ORIGIN,
                 Time.deltaTime
                 );

            CentreBase.transform.position = Vector3.Lerp(
                CentreBase.transform.position,
                new Vector3(CentreBase.transform.position.x, 0f, CentreBase.transform.position.z),
                Time.deltaTime
                );

            CentreBase.GetComponent<MeshRenderer>().material.color = Color.Lerp(
                 CentreBase.GetComponent<MeshRenderer>().material.color,
                 ORIGIN,
                 Time.deltaTime
                 );
        }

        if(isComparison)
        {
            BlueBase.transform.position = Vector3.Lerp(
                BlueBase.transform.position,
                new Vector3(BlueBase.transform.position.x, 0.2f, BlueBase.transform.position.z),
                Time.deltaTime
                );

            BlueBase.GetComponent<MeshRenderer>().material.color = Color.Lerp(
                 BlueBase.GetComponent<MeshRenderer>().material.color,
                 BLUE,
                 Time.deltaTime
                 );

            RedBase.transform.position = Vector3.Lerp(
                RedBase.transform.position,
                new Vector3(RedBase.transform.position.x, 0.2f, RedBase.transform.position.z),
                Time.deltaTime
                );

            RedBase.GetComponent<MeshRenderer>().material.color = Color.Lerp(
                 RedBase.GetComponent<MeshRenderer>().material.color,
                 RED,
                 Time.deltaTime
                 );

            CentreBase.transform.position = Vector3.Lerp(
                CentreBase.transform.position,
                new Vector3(CentreBase.transform.position.x, 0.0f, CentreBase.transform.position.z),
                Time.deltaTime
                );

            CentreBase.GetComponent<MeshRenderer>().material.color = Color.Lerp(
                 CentreBase.GetComponent<MeshRenderer>().material.color,
                 ORIGIN,
                 Time.deltaTime
                 );

            if(BlueBase.transform.position.y >= 0.199f)
            {
                BlueBase.transform.position = new Vector3(BlueBase.transform.position.x, 0.2f, BlueBase.transform.position.z);
                BlueBase.GetComponent<MeshRenderer>().material.color = BLUE;
                RedBase.transform.position = new Vector3(RedBase.transform.position.x, 0.2f, RedBase.transform.position.z);
                RedBase.GetComponent<MeshRenderer>().material.color = RED;
                CentreBase.transform.position = new Vector3(CentreBase.transform.position.x, 0.0f, CentreBase.transform.position.z);
                CentreBase.GetComponent<MeshRenderer>().material.color = ORIGIN;

                isComparison = false;
            }
        }

        if(isEvaluation)
        {
            CentreBase.transform.position = Vector3.Lerp(
                CentreBase.transform.position,
                new Vector3(CentreBase.transform.position.x, 0.2f, CentreBase.transform.position.z),
                Time.deltaTime
                );

            CentreBase.GetComponent<MeshRenderer>().material.color = Color.Lerp(
                CentreBase.GetComponent<MeshRenderer>().material.color,
                NEUTRAL,
                Time.deltaTime
                );

            BlueBase.transform.position = Vector3.Lerp(
                BlueBase.transform.position,
                new Vector3(BlueBase.transform.position.x, 0.0f, BlueBase.transform.position.z),
                Time.deltaTime
                );

            BlueBase.GetComponent<MeshRenderer>().material.color = Color.Lerp(
                 BlueBase.GetComponent<MeshRenderer>().material.color,
                 ORIGIN,
                 Time.deltaTime
                 );

            RedBase.transform.position = Vector3.Lerp(
                RedBase.transform.position,
                new Vector3(RedBase.transform.position.x, 0.0f, RedBase.transform.position.z),
                Time.deltaTime
                );

            RedBase.GetComponent<MeshRenderer>().material.color = Color.Lerp(
                 RedBase.GetComponent<MeshRenderer>().material.color,
                 ORIGIN,
                 Time.deltaTime
                 );

            if(CentreBase.transform.position.y >= 0.199f)
            {
                CentreBase.transform.position = new Vector3(CentreBase.transform.position.x, 0.2f, CentreBase.transform.position.z);
                CentreBase.GetComponent<MeshRenderer>().material.color = NEUTRAL;
                BlueBase.transform.position = new Vector3(BlueBase.transform.position.x, 0.0f, BlueBase.transform.position.z);
                BlueBase.GetComponent<MeshRenderer>().material.color = ORIGIN;
                RedBase.transform.position = new Vector3(RedBase.transform.position.x, 0.0f, RedBase.transform.position.z);
                RedBase.GetComponent<MeshRenderer>().material.color = ORIGIN;

                isEvaluation = false;
            }
        }
    }

    public void SpawnObject(string objrid)
    {
        if(objrid != Asset0rid || CurrentMode != "Evaluation")
        {
            DownloadingTimer = 0f;
            IsDownloading = true;
            Destroy(Asset0);
            Destroy(Asset1);

            Asset0rid = objrid;

            DataManager.Instance.UABTable.GetAsset(objrid, 
                (gobj, gname) =>
                {
                    OnSpawnObject(gobj, 0);
                }
                );

            CurrentMode = "Evaluation";
        }

        isEvaluation = true;
        isComparison = false;
    }

    public void SpawnObject(string obj0rid, string obj1rid)
    {
        bool isSwitchMode = CurrentMode != "Comparison";
        if(obj0rid != Asset0rid || isSwitchMode)
        {
            DownloadingTimer = 0f;
            IsDownloading = true;
            Destroy(Asset0);

            Asset0rid = obj0rid;

            DataManager.Instance.UABTable.GetAsset(obj0rid,
                (gobj, gname) =>
                {
                    OnSpawnObject(gobj, 1);
                }
                );

            CurrentMode = "Comparison";
        }

        if(obj1rid != Asset1rid || isSwitchMode)
        {
            DownloadingTimer = 0f;
            IsDownloading = true;
            Destroy(Asset1);

            Asset1rid = obj1rid;

            DataManager.Instance.UABTable.GetAsset(obj1rid,
                (gobj, gname) =>
                {
                    OnSpawnObject(gobj, 2);
                }
                );
        }

        isEvaluation = false;
        isComparison = true;
    }

    private void OnSpawnObject(GameObject obj, int spawnPos)
    {
        switch (spawnPos)
        {
            case 0:
                Asset0 = Instantiate(obj,
                    new Vector3(CentreBase.transform.position.x, 0.7f, CentreBase.transform.position.z),
                    Quaternion.identity);
                break;

            case 1:
                Asset0 = Instantiate(obj,
                    new Vector3(BlueBase.transform.position.x, 0.7f, BlueBase.transform.position.z),
                    Quaternion.identity);
                break;

            case 2:
                Asset1 = Instantiate(obj,
                                       new Vector3(RedBase.transform.position.x, 1f, RedBase.transform.position.z),
                                                          Quaternion.identity);
                break;

            default:
                break;
        }

        ObjectSpawner.Instance.IsDownloading = false;
        UIManager.Instance.UISystemMessage("[System]: Assets downloaded!");
    }
}
