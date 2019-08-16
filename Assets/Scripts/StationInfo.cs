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

        //Vector3 vec = _transform.position;        
        //transform.position = new Vector3(vec.x, vec.y + 10, vec.z) ;
        gameObject.SetActive(true);
    }
}
