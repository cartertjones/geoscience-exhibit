using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderScript : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private TextMeshProUGUI monthText;
    private Slider slider;
    [SerializeField] private Image sliderFill;

    private string[] dates = { "March 17, 2021","March 27, 2021","April 12, 2021","April 23, 2021","May 11, 2021","May 26, 2021","June 8, 2021","June 23, 2021","July 23, 2021","August 10, 2021","September 3, 2021","September 30, 2021" };
    [SerializeField] private Color32[] colors;

    void Start()
    {
        slider = gameObject.GetComponent<Slider>();
        slider.maxValue = dates.Length;
        monthText.text = dates[0];
        sliderFill.color = colors[0];
        slider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
    }

    // Invoked when the value of the slider changes.
    public void ValueChangeCheck()
    {
        int value = (int)slider.value;
        monthText.text = dates[value - 1];
        sliderFill.color = colors[value - 1];
    }
}
