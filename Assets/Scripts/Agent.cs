﻿using UnityEngine;

public class Agent : MonoBehaviour, IPassenger
{
    public bool WantToGetOff { get; set; }
    public bool WantToGetIn { get; set; }
    
    public void GetIn(Train train, Station station)
    {
    }

    public void GetOff(Station station)
    {
    }

    public void OnBoarding(Train train, Station station)
    {
    }
    
}