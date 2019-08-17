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

    
    // 열차를 타기 위해 Station -> Train으로 이동
    public void Out(IPassenger passenger)
    {
        Debug.Log($"{passenger}이 {name} 역에서 나감");
        passengers.Remove(passenger);
    }

    // 열차에서 사람이 내린 후 Station에 도착
    public void Enter(IPassenger passenger)
    {
        Debug.Log($"{passenger}이 {name} 역에 도착");
        passengers.Add(passenger);
    }

    void OnDrawGizmosSelected()
    {
        Handles.color = Color.yellow;
        Handles.Label(transform.position + Vector3.up * 50, name);
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
