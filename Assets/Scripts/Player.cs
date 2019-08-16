using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : Singleton<Player>
{
    //돈
    public int money;
    //포만감
    public int satiety;
    //수분
    public int moisture;
    //위생
    public int hygiene;
    //위험도
    public int risk;

    //만남
    public bool meeting;
    //탑승상황
    public bool isRiding;
    //반대방향
    public bool isOpposite; 

    //역정보
    [HideInInspector]
    public StationInfo stationInfo;

    //역 갱신
    public void StationRefresh(StationInfo _info, Action _finishAction = null)
    {
        stationInfo = _info;
        _finishAction?.Invoke();
    }

    //탑승
    public void GetIn(Action _finishAction = null)
    {
        isRiding = true;

        _finishAction?.Invoke();
    }

    //하차
    public void GetOut(Action _finishAction = null)
    {
        isRiding = false;

        _finishAction.Invoke();
    }

    //반대방향
    public void Opposite(Action _finishAction = null)
    {
        risk += DataInfo.oppositeRisk;

        isOpposite = !isOpposite;

        _finishAction?.Invoke();
    }

    //화장실
    public void Toilet(Action _finishAction = null)
    {
        hygiene += DataInfo.toiletHygiene;
        risk += DataInfo.toiletRisk;

        _finishAction?.Invoke();
    }

    //음료수 자판기
    public void BeverageVendingMachine(Action _finishAction = null)
    {
        moisture += DataInfo.beverageVendingMachineMoisture;
        risk += DataInfo.beverageVendingMachineRisk;

        if (CanBuy(DataInfo.beverageVendingMachineMoney))
        {
            money -= DataInfo.beverageVendingMachineMoney;
        }

        _finishAction?.Invoke();
    }

    //과자 자판기
    public void SnackVendingMachine(Action _finishAction = null)
    {
        satiety += DataInfo.snackVendingMachineSatiety;
        risk += DataInfo.snackVendingMachineRisk;

        if (CanBuy(DataInfo.snackVendingMachineMoney))
        {
            money -= DataInfo.snackVendingMachineMoney;
        }

        _finishAction?.Invoke();
    }
    
    //구걸
    public void Begging(Action _finishAction = null)
    {
        money += DataInfo.beggingMoney;
        risk += DataInfo.beggingRisk;

        _finishAction.Invoke();
    }

    private bool CanBuy(int _cost)
    {
        if(money >= _cost)
        {
            return true;
        }

        return false;
    }
}
