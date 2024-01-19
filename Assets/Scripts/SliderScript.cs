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

    private string[] dates = { "2021-03-17","2021-03-27","2021-04-12","2021-04-23","2021-05-11","2021-05-26","2021-06-08","2021-06-23","2021-07-23","2021-08-10","2021-09-03","2021-09-30"};
    private Color32[] colors = { new Color32(100, 20, 20, 255), new Color32(100, 50, 20, 255), new Color32(10, 100, 20, 255), new Color32(10, 150, 20, 255), new Color32(10, 255, 20, 255), new Color32(10, 150, 20, 255), new Color32(10, 100, 20, 255), new Color32(100, 50, 20, 255), new Color32(100, 20, 20, 255) };

    void Start()
    {
        slider = gameObject.GetComponent<Slider>();
        slider.maxValue = dates.Length;
        monthText.text = dates[0];
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
