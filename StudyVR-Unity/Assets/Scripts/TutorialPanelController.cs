using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPanelController : MonoBehaviour
{
    [SerializeField] Transform LootAtPos;

    private void Update()
    {
        Quaternion lookatRot = Quaternion.LookRotation(LootAtPos.position - this.transform.position, Vector3.up);
        lookatRot *= Quaternion.Euler(0f, 180f, 0f);
        this.transform.rotation = lookatRot;
    }
}
