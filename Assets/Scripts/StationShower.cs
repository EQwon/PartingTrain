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
