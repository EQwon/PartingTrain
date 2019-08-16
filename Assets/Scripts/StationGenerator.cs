using System.Collections.Generic;
using UnityEngine;

public class StationGenerator : MonoBehaviour
{

    [SerializeField] Train train;
    [SerializeField] Station stationRef;
    [SerializeField] Player player;

    void Awake()
    {
        List<Station> stations = new List<Station>();

        for (int i = 0; i < 10; i++)
        {
            Station station = Instantiate(stationRef, transform);
            station.transform.position = new Vector3(Random.Range(-500, 500), Random.Range(-500, 500));
            station.name = i.ToString();

            stations.Add(station);   
        }


        player.Init(stations[5]);
        train.Init(stations);
    }

    void Start()
    {
        
    }
    
}
