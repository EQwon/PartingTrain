
using UnityEngine;

public interface IPassenger
{
    
    bool WantToGetOff { get; set; }
    bool WantToGetIn { get; set; }
    
    bool IsOpposite { get; }
    
    Transform Transform { get; }
    
    void GetIn(Train train, Station station);
    void GetOff(Station station);
    void OnBoarding(Train train, Station station);

}