using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Station : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string stationName;
    public bool isToilet;
    public bool isReversible;
    
    List<IPassenger> passengers = new List<IPassenger>();

    public List<IPassenger> GetBoardingPassengers => passengers.FindAll(p => p.WantToGetIn);

    
    // 열차를 타기 위해 Station -> Train으로 이동
    public void Out(IPassenger passenger)
    {
        Debug.Log($"{passenger}이 {name} 역에서 나감");
        passengers.Remove(passenger);
    }

    // 열차에서 사람이 내린 후 Station에 도착
    public void Enter(IPassenger passenger)
    {
        Debug.Log($"{passenger}이 {name} 역에 도착");
        passengers.Add(passenger);
    }

    void OnDrawGizmosSelected()
    {
        Handles.color = Color.yellow;
        Handles.Label(transform.position + Vector3.up * 50, name);
    }
    
    #region UI
    
    private Text stationNameText;
    private Image toiletImage;
    private Image reversibleImage;

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
    
    #endregion
}
