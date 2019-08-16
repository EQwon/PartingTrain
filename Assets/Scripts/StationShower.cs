using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StationShower : MonoBehaviour
{
    public StationInfo info;

    public GameObject ActionBubble;

    [Header("Info Bubble")]
    public GameObject InfoBubble;
    public Text stationName;
    public GameObject reversible;
    public GameObject toilet;

    private void Start()
    {
        ActionBubble.SetActive(false);
        InfoBubble.SetActive(false);
    }

    private void InfoInitialize()
    {

    }

    public void GoToToilet()
    {
        Debug.Log(info.name + "역에서 화장실에 갑니다.");
    }

    public void Begging()
    {
        Debug.Log(info.name + "역에서 Action 2");
    }

    public void Action3()
    {
        Debug.Log(info.name + "역에서 Action 3");
    }

    public void Action4()
    {
        Debug.Log(info.name + "역에서 Action 4");
    }
}
