using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderScript : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private TextMeshProUGUI monthText, plantText;
    private Slider slider;
    [SerializeField] private Image sliderFill, plantImageSlot;

    private string[] dates = { "March 17, 2021", "March 27, 2021", "April 12, 2021", "April 23, 2021", "May 11, 2021", "May 26, 2021", "June 8, 2021", "June 23, 2021", "July 23, 2021", "August 10, 2021", "September 3, 2021", "September 30, 2021" };

    [SerializeField] private string[] plantInfo;
    [SerializeField] private Sprite[] plantImages;
    [SerializeField] private Color32[] colors;

    [Header("Info Block")]
    [SerializeField] GameObject infoBlock;
    [SerializeField] private Vector3 offScreenPosition; // Position when the block is hidden
    [SerializeField] private Vector3 onScreenPosition; // Position when the block is visible
    private bool isBlockVisible = false;
    [SerializeField] private TextMeshProUGUI iText;
    [SerializeField] private Color[] iColors;
    [SerializeField] private float moveDuration = 1.0f; // Time of block movement
    [SerializeField] private Slider infoSlider;
    [SerializeField] private float[] infoSliderValues;

    [Header("SideKey")]
    [SerializeField] GameObject key;
    [SerializeField] private Vector3 offScreenKeyPosition; // Position when the block is hidden
    [SerializeField] private Vector3 onScreenKeyPosition; // Position when the block is visible

    void Start()
    {
        slider = gameObject.GetComponent<Slider>();
        slider.maxValue = dates.Length - 1;
        monthText.text = dates[0];

        plantText.text = plantInfo[0];
        plantImageSlot.sprite = plantImages[0];

        sliderFill.color = colors[0];
        slider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
    }

    // Invoked when the value of the slider changes.
    public void ValueChangeCheck()
    {
        int value = (int)slider.value;

        monthText.text = dates[value];
        sliderFill.color = colors[value];
        plantImageSlot.sprite = plantImages[value];
        plantText.text = plantInfo[value];
        infoSlider.value = infoSliderValues[value];
    }

    public void ToggleInfoBlock()
    {
        StartCoroutine(MoveInfoBlock());
    }

    private IEnumerator MoveInfoBlock()
    {
        float timeElapsed = 0;
        Vector3 start = isBlockVisible ? onScreenPosition : offScreenPosition;
        Vector3 end = isBlockVisible ? offScreenPosition : onScreenPosition;

        Vector3 keyStart = isBlockVisible ? onScreenKeyPosition : offScreenKeyPosition;
        Vector3 keyEnd = isBlockVisible ? offScreenKeyPosition : onScreenKeyPosition;

        if (isBlockVisible) iText.color = iColors[0];
        else iText.color = iColors[1];

        while (timeElapsed < moveDuration)
        {
            infoBlock.transform.localPosition = Vector3.Lerp(start, end, timeElapsed / moveDuration);
            key.transform.localPosition = Vector3.Lerp(keyStart, keyEnd, timeElapsed / moveDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        infoBlock.transform.localPosition = end;
        isBlockVisible = !isBlockVisible;
    }
}
