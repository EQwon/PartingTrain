﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StationShower : MonoBehaviour
{
    public bool man;

    private Button[] actionButtons = new Button[5];
    public Button stopButton;
    
    private Text stationNameText;
    private RectTransform rect;

    public Sprite[] rideSprites;
    public Sprite[] stopSprites;

    private int buttonLength;

    void Awake()
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
        stopButton.onClick.AddListener(() => Quit());
    }

    public void QuitAction(Station info)
    {
        if(info == null)
        {
            Debug.LogError("액션을 보여주기 위한 역 정보가 없음");
            return;
        }

        stopButton.gameObject.SetActive(false);
        stopButton.GetComponent<Image>().sprite = stopSprites[0];

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

        actionButtons[0].GetComponent<Image>().sprite = rideSprites[0];
        stopButton.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    public void GoToToilet()
    {
        GameManager.instance.Toilet(man);
        SoundManager.instance.PlaySound(SoundManager.instance.runClip, 1f);
    }

    public void Begging()
    {
        GameManager.instance.Begging(man);
        SoundManager.instance.PlaySound(SoundManager.instance.beggingClip, 0.7f);
    }

    public void Ride()
    {
        GameManager.instance.GetIn(man);

        int idx = man ? 0 : 1;

        if (GameManager.instance.players[idx].WantToGetIn)
        {
            actionButtons[0].GetComponent<Image>().sprite = rideSprites[1];
        }
        else
        {
            actionButtons[0].GetComponent<Image>().sprite = rideSprites[0];
        }
    }

    public void Quit()
    {
        GameManager.instance.GetOut(man);

        int idx = man ? 0 : 1;

        if (GameManager.instance.players[idx].WantToGetOff)
        {
            stopButton.GetComponent<Image>().sprite = stopSprites[1];
        }
        else
        {
            stopButton.GetComponent<Image>().sprite = stopSprites[0];
        }
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
