﻿
public interface IPassenger
{
    
    bool WantToGetOff { get; set; }
    bool WantToGetIn { get; set; }

    void GetIn(Train train, Station station);
    void GetOff(Station station);
    void OnBoarding(Train train, Station station);

}