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
        public Text text;
    }

    public StatusImage[] statusImages = new StatusImage[2];

    public GameObject valueObject;

    private Text valueText;

    public Button[] buttons;

    private bool[] buttonsCheck;

    private void Start()
    {
        valueText = valueObject.GetComponentInChildren<Text>();

        buttons = GetComponentsInChildren<Button>();
        buttonsCheck = new bool[buttons.Length];

        buttons[0].onClick.AddListener(() => QuitAction(true));
        buttons[1].onClick.AddListener(() => ShowStatusValue(0, true, "Satiety", buttons[0].GetComponent<RectTransform>()));
        buttons[2].onClick.AddListener(() => ShowStatusValue(1, true, "Moisture", buttons[1].GetComponent<RectTransform>()));
        buttons[3].onClick.AddListener(() => ShowStatusValue(2, true, "Hygine", buttons[2].GetComponent<RectTransform>()));
        buttons[4].onClick.AddListener(() => QuitAction(false));
        buttons[5].onClick.AddListener(() => ShowStatusValue(3, false, "Satiety", buttons[3].GetComponent<RectTransform>()));
        buttons[6].onClick.AddListener(() => ShowStatusValue(4, false, "Moisture", buttons[4].GetComponent<RectTransform>()));
        buttons[7].onClick.AddListener(() => ShowStatusValue(5, false, "Hygine", buttons[5].GetComponent<RectTransform>()));

        SetStart();
    }

    public void SetStart()
    {
        for(int i = 0; i < GameManager.instance.players.Length; i++)
        {
            statusImages[i].images[0].fillAmount = DataInfo.playerStartSatiety * 0.01f;
            statusImages[i].images[1].fillAmount = DataInfo.playerStartMoisture * 0.01f;
            statusImages[i].images[2].fillAmount = DataInfo.playerStartHygiene * 0.01f;
            statusImages[i].text.text = DataInfo.playerStartMoney.ToString();
        }
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
            case "Money":
                statusImages[idx].text.text = _player.Money.ToString();
                break;
        }
    }

    public void ShowStatusValue(int _idx, bool _isMan, string _eventName, RectTransform _rect)
    {
        if (buttonsCheck[_idx])
        {
            valueObject.SetActive(false);
            buttonsCheck[_idx] = false;
            return;
        }

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

        valueObject.GetComponent<RectTransform>().position = new Vector2(_rect.position.x, _rect.position.y - 30);
        buttonsCheck[_idx] = true;
        valueObject.SetActive(true);
    }

    public void QuitAction(bool _isMan)
    {
        GameManager.instance.GetOut(_isMan);
    }
}
