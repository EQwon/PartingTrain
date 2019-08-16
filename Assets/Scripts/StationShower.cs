using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StationShower : MonoBehaviour
{
    public Station info;

    public GameObject ActionBubble;

    [Header("Info Bubble")]
    public GameObject InfoBubble;
    public Text stationName;
    public GameObject reversible;
    public GameObject toilet;

    private void Start()
    {
        ActionBubble.SetActive(false);
        //InfoBubble.SetActive(false);
    }

    private void InfoInitialize()
    {

    }

    public void GoToToilet()
    {
        GameManager.instance.Toilet();
        Debug.Log(info.stationName + "역에서 화장실");
    }

    public void Begging()
    {
        GameManager.instance.Begging();
        Debug.Log(info.stationName + "역에서 구걸");
    }

    public void Ride()
    {
        GameManager.instance.GetIn();
        Debug.Log(info.stationName + "역에서 탑승");
    }

    public void Quit()
    {
        GameManager.instance.GetOut();
        Debug.Log(info.stationName + "역에서 내림");
    }

    public void Beverage()
    {
        GameManager.instance.BeverageVendingMachine();
        Debug.Log(info.stationName + "역에서 음료수");
    }

    public void Snack()
    {
        GameManager.instance.SnackVendingMachine();
        Debug.Log(info.stationName + "역에서 과자");
    }

    public void Opposite()
    {
        GameManager.instance.Opposite();
        Debug.Log(info.stationName + "역에서 반대방향");
    }
}
