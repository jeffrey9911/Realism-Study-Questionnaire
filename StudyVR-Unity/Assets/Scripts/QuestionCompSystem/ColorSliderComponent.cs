using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorSliderComponent : MonoBehaviour
{
    [SerializeField] private Image background;
    [SerializeField] private Image fill;
    [SerializeField] private Image handle;

    private Color BLUE = new Color(80 / 255f, 192 / 255f, 1.0f, 1.0f);
    private Color RED = new Color(200 / 255f, 97 / 255f, 87 / 255f, 1.0f);

    public void OnSliderValueChanged(float value)
    {
        float perc = (value - GetComponent<Slider>().minValue) / (GetComponent<Slider>().maxValue - GetComponent<Slider>().minValue);
        background.color = Color.Lerp(BLUE, RED, perc);
        fill.color = Color.Lerp(BLUE, RED, perc);
        handle.color = Color.Lerp(BLUE, RED, perc);
    }
}
