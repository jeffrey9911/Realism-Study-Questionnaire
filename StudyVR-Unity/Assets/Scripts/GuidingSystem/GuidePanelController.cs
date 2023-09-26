using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuidePanelController : MonoBehaviour
{
    [SerializeField] private List<Image> GuideImages;

    private bool IsOn = false;
    private bool IsTriggering = false;

    private float Alpha = 0f;

    private void Start()
    {
        SetAlpha(Alpha);
    }

    private void Update()
    {
        if(IsTriggering)
        {
            if(IsOn)
            {
                Alpha += Time.deltaTime / 2;
                if (Alpha >= 1f)
                {
                    Alpha = 1f;
                    IsTriggering = false;
                }
                SetAlpha(Alpha);
            }
            else
            {
                Alpha -= Time.deltaTime / 2;
                if (Alpha <= 0f)
                {
                    Alpha = 0f;
                    IsTriggering = false;
                }
                SetAlpha(Alpha);
            }
        }
    }

    private void SetAlpha(float a)
    {
        foreach (var image in GuideImages)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, a);
        }
    }

    public void TurnOn()
    {
        IsOn = true;
        IsTriggering = true;
    }

    public void TurnOff()
    {
        IsOn = false;
        IsTriggering = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && IsOn)
        {
            TurnOff();
        }
    }

}
