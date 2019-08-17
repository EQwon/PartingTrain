using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StationShower : MonoBehaviour
{
    public bool man;

    private Button[] actionButtons = new Button[5];
    private Text stationNameText;
    private RectTransform rect;

    private int buttonLength;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
        stationNameText = GetComponentInChildren<Text>();
        actionButtons = GetComponentsInChildren<Button>(true);

        buttonLength = actionButtons.Length;

        actionButtons[0].onClick.AddListener(() => Ride());
        actionButtons[1].onClick.AddListener(() => Opposite());
        actionButtons[2].onClick.AddListener(() => GoToToilet());
        actionButtons[3].onClick.AddListener(() => Snack());
        actionButtons[4].onClick.AddListener(() => Beverage());
        actionButtons[5].onClick.AddListener(() => Begging());
        
        actionButtons[buttonLength - 1].gameObject.SetActive(false);
    }

    public void QuitAction(Station info)
    {
        if(info == null)
        {
            Debug.LogError("액션을 보여주기 위한 역 정보가 없음");
            return;
        }

        var pos = info.GetComponent<RectTransform>().position;
        rect.position = new Vector2(pos.x, pos.y + 30);

        stationNameText.text = info.stationName;

        actionButtons[0].gameObject.SetActive(info.canRiding);
        actionButtons[1].gameObject.SetActive(info.canReversible);
        actionButtons[2].gameObject.SetActive(info.canToilet);
        actionButtons[3].gameObject.SetActive(info.canSnackVending);
        actionButtons[4].gameObject.SetActive(info.canBeverageVending);
        actionButtons[5].gameObject.SetActive(info.canBegging);

        gameObject.SetActive(true);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)transform);
    }

    public void RideAction()
    {
        for(int i = 0; i < buttonLength; i++)
        {
            actionButtons[i].gameObject.SetActive(false);
        }

        gameObject.SetActive(false);
    }

    public void GoToToilet()
    {
        GameManager.instance.Toilet(man);
    }

    public void Begging()
    {
        GameManager.instance.Begging(man);
    }

    public void Ride()
    {
        GameManager.instance.GetIn(man);
    }

    public void Quit()
    {
        GameManager.instance.GetOut(man);
    }

    public void Beverage()
    {
        GameManager.instance.BeverageVendingMachine(man);
    }

    public void Snack()
    {
        GameManager.instance.SnackVendingMachine(man);
    }

    public void Opposite()
    {
        GameManager.instance.Opposite(man);
    }
}
