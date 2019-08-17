using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Station : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string stationName;
    public bool canToilet;
    public bool canReversible;
    public bool canSnackVending;
    public bool canBeverageVending;
    public bool canBegging = true;
    public bool canRiding = true;

    List<IPassenger> passengers = new List<IPassenger>();

    public List<IPassenger> GetBoardingPassengers => passengers.FindAll(p => p.WantToGetIn);
    public bool IsPlayerInStation => passengers.Exists(p => p is Player);

    Image circle;

    void Awake()
    {
        circle = transform.GetChild(2).GetComponent<Image>();
    }
    
    // 열차를 타기 위해 Station -> Train으로 이동
    public void Out(IPassenger passenger)
    {
        Debug.Log($"{passenger}이 {name} 역에서 나감");
        passengers.Remove(passenger);
        
        Refresh();
    }

    // 열차에서 사람이 내린 후 Station에 도착
    public void Enter(IPassenger passenger)
    {
        Debug.Log($"{passenger}이 {name} 역에 도착");

        passengers.Add(passenger);

        if (passengers.FindAll(p => p is Player).Count >= 2)
        {
            GameManager.instance.Meeting();
        }
        
        Refresh();
    }

    public void Remove(IPassenger passenger)
    {
        passengers.Remove(passenger);
        Refresh();
    }

    public void Refresh()
    {
        bool player = false;
        bool agent = false;
        Color color = Color.black;
        foreach (IPassenger passenger in passengers)
        {
            if (passenger is Player p)
            {
                color += p.playerColor;
                player = true;
            }
            else
            {
                agent = true;
            }
            passenger.Transform.SetParent(transform.GetChild(passenger.IsOpposite ? 0 : 1), false);
            passenger.Transform.localPosition = Vector3.zero;
            passenger.Transform.localRotation = Quaternion.identity;
        }

        if (agent)
        {
            circle.color = Color.black;
        }
        else if(player)
        {
            circle.color = color;
        }
        else
        {
            circle.color = new Color(0,0,0,0);
        }
        
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)transform);
    }

    #region UI
    
    public void OnPointerEnter(PointerEventData _data)
    {
        UIManager.instance.ShowStationInfo(this);

    }

    public void OnPointerExit(PointerEventData _data)
    {
        UIManager.instance.HideStationInfo();
    }
    
    #endregion
}
