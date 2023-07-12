using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshSequencePlayer : MonoBehaviour
{
    public int PlayerFramePerSecond = 30;

    public bool isPlaying = false;

    private int CurrentFrame = 0;

    private float FrameTimer = 0;

    private List<GameObject> ObjectSequence = new List<GameObject>();

    private void Start()
    {
        foreach(Transform child in this.transform)
        {
            ObjectSequence.Add(child.gameObject);
        }

        foreach(GameObject obj in ObjectSequence)
        {
            obj.SetActive(false);
        }
    }

    private void Update()
    {
        if(isPlaying)
        {
            FrameTimer += Time.deltaTime;

            if(FrameTimer >= 1f/ PlayerFramePerSecond)
            {
                SwapFrame();
                FrameTimer = 0;
            }

        }
    }

    private void SwapFrame()
    {
        int NextFrame = (CurrentFrame + 1) >= ObjectSequence.Count ? 0 : CurrentFrame + 1;

        ObjectSequence[CurrentFrame].SetActive(false);
        ObjectSequence[NextFrame].SetActive(true);

        CurrentFrame = NextFrame;
    }
}
