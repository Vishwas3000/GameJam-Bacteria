using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashBar : MonoBehaviour
{
    public static DashBar s_dashBar;
    public Slider slider;
    public Image fill;
    public Gradient gradient;

    public void Awake()
    {
        s_dashBar = this;
    }

    private void Start()
    {
        slider.maxValue = 1f;
        slider.value = 1f;

        fill.color = gradient.Evaluate(1f);
    }

    public void SetFill(float fillSize)
    {
        slider.value = fillSize;

        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
