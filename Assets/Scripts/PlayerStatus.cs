using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerStatus : MonoBehaviour
{
    [System.Serializable]
    public class StatusImage
    {
        public Image[] images = new Image[3];
    }

    public StatusImage[] statusImages = new StatusImage[2];

    public GameObject valueObject;

    private Text valueText;

    private Button[] buttons;

    private void Start()
    {
        buttons = GetComponentsInChildren<Button>();

        buttons[0].onClick.AddListener(() => ShowStatusValue(true, "Satiety"));
        buttons[1].onClick.AddListener(() => ShowStatusValue(true, "Moisture"));
        buttons[2].onClick.AddListener(() => ShowStatusValue(true, "Hygine"));
        buttons[3].onClick.AddListener(() => ShowStatusValue(false, "Satiety"));
        buttons[4].onClick.AddListener(() => ShowStatusValue(false, "Moisture"));
        buttons[5].onClick.AddListener(() => ShowStatusValue(false, "Hygine"));
    }

    public void StatusRefresh(string _eventName, Player _player)
    {
        int idx = _player.isMan ? 0 : 1;

        switch (_eventName)
        {
            case "Satiety":
                statusImages[idx].images[0].fillAmount = _player.Satiety * 0.01f;
                break;
            case "Moisture":
                statusImages[idx].images[1].fillAmount = _player.Moisture * 0.01f;
                break;
            case "Hygine":
                statusImages[idx].images[2].fillAmount = _player.Hygine * 0.01f;
                break;
        }
    }

    public void ShowStatusValue(bool _isMan, string _eventName)
    {
        int idx = _isMan ? 0 : 1;

        switch (_eventName)
        {
            case "Satiety":
                valueText.text = GameManager.instance.players[idx].Satiety.ToString();
                break;
            case "Moisture":
                valueText.text = GameManager.instance.players[idx].Moisture.ToString();
                break;
            case "Hygine":
                valueText.text = GameManager.instance.players[idx].Hygine.ToString();
                break;
        }
    }
}
