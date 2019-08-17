using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public StationShower[] showers;
    public StationInfo stationInfo;
    public Text timeText;
    public PlayerStatus playerStatus;

    public void ShowStationInfo(Station _station)
    {
        stationInfo.Show(_station.stationName, _station.canToilet, _station.canReversible, _station.canBeverageVending, _station.canSnackVending, _station.GetComponent<RectTransform>());
    }

    public void HideStationInfo()
    {
        stationInfo.gameObject.SetActive(false);
    }
}
