using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshSequenceContainer : MonoBehaviour
{
    public Vector3 PositionOffset;
    public Vector3 RotationOffset;
    public Vector3 ScaleOffset;
    public List<Mesh> MeshSequence = new List<Mesh>();
    public List<MeshRenderer> MeshRendererSequence = new List<MeshRenderer>();
}
