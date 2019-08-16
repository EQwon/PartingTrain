using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameManager : Singleton<GameManager>
{
    public Player[] players = new Player[2];
    public StationShower[] showers = new StationShower[2];

    private float time;

    private TimeSpan timeSpan;

    public Text text;

    private void Start()
    {
        timeSpan = new TimeSpan();
        timeSpan += TimeSpan.FromHours(5);

        StartCoroutine(DayClockCoroutine());
    }

    //역 갱신 Player의 GetIn GetOut에서 관리
//    public void StationRefresh(bool _isMan, Station _info, Action _finishAction = null)
//    {
//        int idx = _isMan ? 0 : 1;
//
//        players[idx].stationInfo = _info;
//
//        _finishAction?.Invoke();
//    }

    //탑승예약
    public void GetIn(bool _isMan, Action _finishAction = null)
    {
        int idx = _isMan ? 0 : 1;

        players[idx].WantToGetIn = true;

        _finishAction?.Invoke();
    }

    //하차예약
    public void GetOut(bool _isMan, Action _finishAction = null)
    {
        int idx = _isMan ? 0 : 1;

        players[idx].WantToGetOff = true;

        _finishAction?.Invoke();
    }

    //반대방향
    public void Opposite(bool _isMan, Action _finishAction = null)
    {
        int idx = _isMan ? 0 : 1;

        players[idx].risk += DataInfo.oppositeRisk;
        players[idx].isOpposite = !players[0].isOpposite;

        SpendTime(DataInfo.oppositeTime);

        _finishAction?.Invoke();
    }

    //화장실
    public void Toilet(bool _isMan, Action _finishAction = null)
    {
        int idx = _isMan ? 0 : 1;

        players[idx].hygiene += DataInfo.toiletHygiene;
        players[idx].risk = DataInfo.toiletRisk;

        SpendTime(DataInfo.toiletTime);

        _finishAction?.Invoke();
    }

    //음료수 자판기
    public void BeverageVendingMachine(bool _isMan, Action _finishAction = null)
    {
        int idx = _isMan ? 0 : 1;

        players[idx].moisture += DataInfo.beverageVendingMachineMoisture;
        players[idx].risk = DataInfo.beverageVendingMachineRisk;

        if (CanBuy(_isMan, DataInfo.beverageVendingMachineMoney))
        {
            players[idx].money -= DataInfo.beverageVendingMachineMoney;
        }

        SpendTime(DataInfo.beverageTime);

        _finishAction?.Invoke();
    }

    //과자 자판기
    public void SnackVendingMachine(bool _isMan, Action _finishAction = null)
    {
        int idx = _isMan ? 0 : 1;

        players[idx].satiety += DataInfo.snackVendingMachineSatiety;
        players[idx].risk = DataInfo.snackVendingMachineRisk;

        if (CanBuy(_isMan, DataInfo.snackVendingMachineMoney))
        {
            players[idx].money -= DataInfo.snackVendingMachineMoney;
        }

        SpendTime(DataInfo.snackTime);

        _finishAction?.Invoke();
    }
    
    //구걸
    public void Begging(bool _isMan, Action _finishAction = null)
    {
        int idx = _isMan ? 0 : 1;

        players[idx].money += DataInfo.beggingMoney;
        players[idx].risk = DataInfo.beggingRisk;

        SpendTime(DataInfo.beggingTime);

        _finishAction.Invoke();
    }

    private bool CanBuy(bool _isMan, int _cost)
    {
        int idx = _isMan ? 0 : 1;

        if(players[idx].money >= _cost)
        {
            return true;
        }

        return false;
    }

    private void CheckGameOver()
    {
        //if(satiety <= 0 || moisture <= 0 || hygiene <= 0)
        //{
        //    Debug.Log("게임 오버");
        //}
    }

    private IEnumerator DayClockCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            timeSpan += TimeSpan.FromMinutes(4);

            text.text = string.Format("{0:00} : {1:00}", timeSpan.Hours, timeSpan.Minutes);
        }
    }

    public void SpendTime(int _time)
    {
        timeSpan += TimeSpan.FromMinutes(4 * _time);
    }
}
