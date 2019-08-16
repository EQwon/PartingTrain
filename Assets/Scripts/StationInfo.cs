using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StationInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Text stationNameText;
    private Image toiletImage;
    private Image reversibleImage;

    public string stationName;
    public bool isToilet;
    public bool isReversible;

    private void Start()
    {
        var child = transform.GetChild(0).GetComponentsInChildren<Transform>(true);
        stationNameText = child.FindInObjects("StationName").GetComponent<Text>();
        toiletImage = child.FindInObjects("Toilet").GetComponent<Image>();
        reversibleImage = child.FindInObjects("Reversible").GetComponent<Image>();

        if (transform.GetChild(0).gameObject.activeSelf)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }

        stationNameText.text = stationName;
        toiletImage.gameObject.SetActive(isToilet);
        reversibleImage.gameObject.SetActive(isReversible);
    }

    private void Show()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }

    private void Hide()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData _data)
    {
        Show();
    }

    public void OnPointerExit(PointerEventData _data)
    {
        Hide();
    }
}
