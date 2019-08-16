﻿using UnityEngine;

public class Player : MonoBehaviour, IPassenger
{
    [SerializeField] Station startStation;
    
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
    public Station stationInfo;

    public bool WantToGetOff { get; set; }
    public bool WantToGetIn { get; set; }

    void Start()
    {
        Init(startStation);
    }
    
    public void Init(Station station)
    {
        WantToGetIn = true;
        transform.SetParent(station.transform);
        transform.localPosition = Vector3.zero;
        station.Enter(this);
    }
    
    public void GetIn(Train train, Station station)
    {
        Debug.Log($"{name}이 {station.name} 에서 {train} 열차를 탐");
        transform.SetParent(train.transform);
        transform.localPosition = Vector3.zero;
        WantToGetIn = false;
        station.Out(this);
    }

    public void GetOff(Station station)
    {
        Debug.Log($"{name}이 {station.name} 에서 열차를 내림");
        transform.SetParent(station.transform, false);
        transform.localPosition = Vector3.zero;
        WantToGetOff = false;
        station.Enter(this);
    }

    public void OnBoarding(Train train, Station station)
    {
        Debug.Log($"{name}은 {train.name}을 타고 {station}에 도착했음");

        if (station.name == "3")
            WantToGetOff = true;
    }

}
