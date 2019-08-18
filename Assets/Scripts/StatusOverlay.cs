using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StatusOverlay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public enum StatusType
    {
        Satiety,
        Moisture,
        Hygiene
    }

    public StatusType type;

    public bool isMan;

    private GameObject value;
    private Text valueText;

    private void Start()
    {
        value = transform.Find("Value").gameObject;
        valueText = value.GetComponentInChildren<Text>();
    }

    public void OnPointerEnter(PointerEventData _data)
    {
        int idx = isMan ? 0 : 1;

        switch (type)
        {
            case StatusType.Hygiene:
                valueText.text = "위생 : " + Mathf.FloorToInt(GameManager.instance.players[idx].Hygine).ToString();
                break;
            case StatusType.Moisture:
                valueText.text = "수분 : " + Mathf.FloorToInt(GameManager.instance.players[idx].Moisture).ToString();
                break;
            case StatusType.Satiety:
                valueText.text = "포만감 : " + Mathf.FloorToInt(GameManager.instance.players[idx].Satiety).ToString();
                break;
        }

        value.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData _data)
    {
        value.gameObject.SetActive(false);
    }
}
