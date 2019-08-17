using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StationInfo : MonoBehaviour
{
    private Text stationNameText;
    private Image toiletImage;
    private Image reversibleImage;
    private Image beverageImage;
    private Image snackImage;
    private RectTransform rect;

    private void Start()
    {
        rect = GetComponent<RectTransform>();

        stationNameText = transform.Find("StationName").GetComponent<Text>();
        toiletImage = transform.Find("Toilet").GetComponent<Image>();
        reversibleImage = transform.Find("Reversible").GetComponent<Image>();
        beverageImage = transform.Find("Beverage").GetComponent<Image>();
        snackImage = transform.Find("Snack").GetComponent<Image>();
        
        gameObject.SetActive(false);
    }

    public void Show(string _stationName, bool _toilet, bool _reversible, bool _beverage, bool _snack, RectTransform _transform)
    {
        stationNameText.text = _stationName;
        toiletImage.gameObject.SetActive(_toilet);
        reversibleImage.gameObject.SetActive(_reversible);
        beverageImage.gameObject.SetActive(_beverage);
        snackImage.gameObject.SetActive(_snack);

        rect.anchoredPosition = new Vector2(_transform.anchoredPosition.x, _transform.anchoredPosition.y + 60);

        gameObject.SetActive(true);

        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)transform);
    }

    private void OnDisable()
    {
        stationNameText.text = null;
        toiletImage.gameObject.SetActive(false);
        reversibleImage.gameObject.SetActive(false);
        beverageImage.gameObject.SetActive(false);
        snackImage.gameObject.SetActive(false);
    }
}
