using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StationShower : MonoBehaviour
{
    public bool man;

    private Button[] actionButtons = new Button[5];

    private void Start()
    {
        actionButtons = GetComponentsInChildren<Button>();

        actionButtons[0].onClick.AddListener(() => Opposite());
        actionButtons[1].onClick.AddListener(() => GoToToilet());
        actionButtons[2].onClick.AddListener(() => Beverage());
        actionButtons[3].onClick.AddListener(() => Snack());
        actionButtons[4].onClick.AddListener(() => Ride());
        actionButtons[5].onClick.AddListener(() => Quit());
    }

    public void QuitAction(Station info)
    {
        if(info == null)
        {
            Debug.LogError("액션을 보여주기 위한 역 정보가 없음");
            return;
        }

        actionButtons[0].gameObject.SetActive(info.isReversible);
        actionButtons[1].gameObject.SetActive(info.isToilet);
        actionButtons[2].gameObject.SetActive(info.isBeverageVending);
        actionButtons[3].gameObject.SetActive(info.isSnackVending);
        actionButtons[4].gameObject.SetActive(true);
    }

    public void RideAction()
    {
        for(int i = 0; i < 5; i++)
        {
            actionButtons[i].gameObject.SetActive(false);
        }

        actionButtons[5].gameObject.SetActive(true);
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
