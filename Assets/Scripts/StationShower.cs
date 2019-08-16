using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationShower : MonoBehaviour
{
    public StationInfo info;

    public GameObject ActionBubble;
    public GameObject InfoBubble;

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
        Player.instance.Toilet();
        Debug.Log(info.stationName + "역에서 화장실");
    }

    public void Begging()
    {
        Player.instance.Begging();
        Debug.Log(info.stationName + "역에서 구걸");
    }

    public void Ride()
    {
        Player.instance.GetIn();
        Debug.Log(info.stationName + "역에서 탑승");
    }

    public void Quit()
    {
        Player.instance.GetOut();
        Debug.Log(info.stationName + "역에서 내림");
    }

    public void Beverage()
    {
        Player.instance.BeverageVendingMachine();
        Debug.Log(info.stationName + "역에서 음료수");
    }

    public void Snack()
    {
        Player.instance.SnackVendingMachine();
        Debug.Log(info.stationName + "역에서 과자");
    }

    public void Opposite()
    {
        Player.instance.Opposite();
        Debug.Log(info.stationName + "역에서 반대방향");
    }
}
