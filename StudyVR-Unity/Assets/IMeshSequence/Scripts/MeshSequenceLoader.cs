using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEditor;
using UnityEngine;

public class MeshSequenceLoader : MonoBehaviour
{
    public GameObject ExampleMesh;
    private string FolderName;
    private string ExampleMeshName;
    private int Frames;

    private string FolderBasePath = "IMeshSequence/";

    private List<GameObject> ObjectSequence = new List<GameObject>();

    public bool IsUsingMaterialSequence;
    [SerializeField] [HideInInspector] public Texture2D ExampleMaterial;
    private string ExampleMaterialName;
    private List<Material> MaterialSequence = new List<Material>();

    [ContextMenu("Load Mesh Sequence")]
    public void LoadMeshSequence()
    {
#if UNITY_EDITOR
        if(ExampleMesh != null)
        {
            ExampleMeshName = ExampleMesh.name;
            FolderName = AssetDatabase.GetAssetPath(ExampleMesh).Replace("Assets/Resources/IMeshSequence/", "").Replace(System.IO.Path.GetFileName(AssetDatabase.GetAssetPath(ExampleMesh)), "");
        }
        else
        {
            Debug.LogError("Example Object Mesh is null!");
            return;
        }


        int IndexDigits = 0;

        for(int i = ExampleMeshName.Length - 1; i >= 0; i--)
        {
            if (char.IsDigit(ExampleMeshName[i])) IndexDigits++;
            else break;
        }

        string MeshName = ExampleMeshName.Substring(0, ExampleMeshName.Length - IndexDigits);

        FolderBasePath += $"{FolderName}";

        Debug.Log($"Folder Base Path: {FolderBasePath}, Mesh Name Loaded: {MeshName}, Index format: {(0).ToString($"D{IndexDigits}")}");


        while(true)
        {
            if (Resources.Load<GameObject>(FolderBasePath + MeshName + Frames.ToString($"D{IndexDigits}")) == null) break;
            Frames++;
        }

        for(int i = 0; i < Frames; i++)
        {
            ObjectSequence.Add(Instantiate(Resources.Load<GameObject>(FolderBasePath + MeshName + i.ToString($"D{IndexDigits}")), this.transform));
        }

        if(IsUsingMaterialSequence)
        {
            if (ExampleMaterial != null)
            {
                ExampleMaterialName = ExampleMaterial.name;
            }
            else
            {
                Debug.LogError("Example Object Material is null!");
                return;
            }

            int TextureIndexDigits = 0;

            for (int i = ExampleMaterialName.Length - 1; i >= 0; i--)
            {
                if (char.IsDigit(ExampleMaterialName[i])) TextureIndexDigits++;
                else break;
            }

            string TextureName = ExampleMaterialName.Substring(0, ExampleMaterialName.Length - TextureIndexDigits);

            for (int i = 0; i < Frames; i++)
            {
                Texture2D texture = Resources.Load<Texture2D>(FolderBasePath + TextureName + i.ToString($"D{TextureIndexDigits}"));

                Material material = new Material(Shader.Find("Standard"));
                material.mainTexture = texture;

                string materialName = $"mat_{i.ToString($"D{TextureIndexDigits}")}";

                AssetDatabase.CreateAsset(material, $"Assets\\Resources\\{FolderBasePath}{materialName}.mat");
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                Material loadedMaterial = Resources.Load<Material>(FolderBasePath + materialName);

                MaterialSequence.Add(loadedMaterial);
            }

            for(int i = 0; i < Frames; i++)
            {
                SwapAllMaterial(ObjectSequence[i].transform, MaterialSequence[i]);
            }
        }

        for(int i = 0; i < ObjectSequence.Count; i++)
        {
            if (i != 0) ObjectSequence[i].SetActive(false);
        }

        Debug.Log("Mesh Sequence Loaded!");
#endif
    }

    public void DeactiveAll()
    {
        foreach(GameObject obj in ObjectSequence)
        {
            obj.SetActive(false);
        }
    }

    private void SwapAllMaterial(Transform parent, Material material)
    {
        MeshRenderer meshRenderer = parent.GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            meshRenderer.sharedMaterial = material;
        }


        for(int i = 0; i < parent.childCount; i++)
        {
            SwapAllMaterial(parent.GetChild(i), material);
        }
    }

    [ContextMenu("Apply Offset")]
    public void ApplyOffset()
    {
        for(int i = 0; i < ObjectSequence.Count; i++)
        {
            Vector3 offsetPos = ObjectSequence[0].transform.localPosition;
            ObjectSequence[i].transform.localPosition = offsetPos;
        }

        Debug.Log("Offset Applied");
    }

    [ContextMenu("Show Sequence Information")]
    public void ShowSequenceInformation()
    {
        Debug.Log($"Folder Name: {FolderName}");
        Debug.Log($"Example Mesh Name: {ExampleMeshName}");
        Debug.Log($"Base path: {FolderBasePath}");
        Debug.Log($"Frames: {Frames}");
    }
}
