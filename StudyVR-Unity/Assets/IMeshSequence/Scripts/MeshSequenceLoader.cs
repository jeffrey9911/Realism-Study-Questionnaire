using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEditor;
using UnityEngine;

public class MeshSequenceLoader : MonoBehaviour
{
    private string FolderName;
    private string FolderBasePath = "IMeshSequence/";

    public GameObject ExampleMesh;
    private string ExampleMeshName;

    private int Frames;

    private MeshSequenceContainer meshSequenceContainer;

    private GameObject exampleObj;

    [ContextMenu("Load Mesh Sequence")]
    public void LoadMeshSequence()
    {
#if UNITY_EDITOR
        if(ExampleMesh != null)
        {
            ExampleMeshName = ExampleMesh.name;
            FolderName = AssetDatabase.GetAssetPath(ExampleMesh).Replace("Assets/Resources/IMeshSequence/", "").Replace(System.IO.Path.GetFileName(AssetDatabase.GetAssetPath(ExampleMesh)), "");
            meshSequenceContainer = this.gameObject.AddComponent<MeshSequenceContainer>();
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

        for (int i = 0; i < Frames; i++)
        {
            GameObject frame = Resources.Load<GameObject>(FolderBasePath + MeshName + i.ToString($"D{IndexDigits}"));
            
            Mesh frameMesh = GetMesh(frame.transform);
            if (frameMesh != null) { meshSequenceContainer.MeshSequence.Add(frameMesh); }

            MeshRenderer frameRenderer = GetMeshRenderer(frame.transform);
            if (frameRenderer != null) { meshSequenceContainer.MeshRendererSequence.Add(frameRenderer); }
        }

        exampleObj = Instantiate(ExampleMesh, this.transform);
        exampleObj.name = "Example Mesh for modify offset";

        Debug.Log("Mesh Sequence Loaded!");

        this.gameObject.AddComponent<MeshSequencePlayer>();

        DestroyImmediate(this.gameObject.GetComponent<MeshSequenceLoader>());
#endif
    }

    [ContextMenu("Show Sequence Information")]
    public void ShowSequenceInformation()
    {
        Debug.Log($"Folder Name: {FolderName}");
        Debug.Log($"Example Mesh Name: {ExampleMeshName}");
        Debug.Log($"Base path: {FolderBasePath}");
        Debug.Log($"Frames: {Frames}");
    }

    private MeshRenderer GetMeshRenderer(Transform parent)
    {
        MeshRenderer renderer = parent.GetComponent<MeshRenderer>();

        if (renderer != null) { return renderer; }

        foreach (Transform child in parent)
        {
            renderer = GetMeshRenderer(child);
            if (renderer != null) { return renderer; }
        }

        return null;
    }

    private Mesh GetMesh(Transform parent)
    {
        MeshFilter meshFilter;

        if(parent.TryGetComponent<MeshFilter>(out meshFilter))
        {
            return meshFilter.sharedMesh;
        }
        else
        {
            foreach (Transform child in parent)
            {
                Mesh mesh = GetMesh(child);
                if (mesh != null) { return mesh; }
            }
        }

        return null;
    }
}
