using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour
{
    [SerializeField] List<Station> stations;
    [SerializeField] bool reverse;
    [SerializeField] bool branchLine;

    List<IPassenger> passengers = new List<IPassenger>();

    private void Start()
    {
        if (reverse)
        {
            stations.Reverse();
        }
        
        StartCoroutine(nameof(TrainUpdator));
    }

    IEnumerator TrainUpdator()
    {
        Queue<Station> stationQueue = new Queue<Station>(stations);

        if (stationQueue.Count <= 0)
            yield break;
        
        Station currentStation = stationQueue.Dequeue();
        transform.position = currentStation.transform.position;
        
        while (true)
        {
            if (stationQueue.Count <= 0)
            {
                if(branchLine)
                    stations.Reverse();
                stationQueue = new Queue<Station>(stations);
            }
            
            Station nextStation = stationQueue.Dequeue();

            // 열차 출발 전
            
            // 타고 싶은 승객들 태우기
            foreach (IPassenger passenger in currentStation.GetBoardingPassengers)
            {
                passenger.GetIn(this, currentStation);
                passengers.Add(passenger);
            }

            yield return StartCoroutine(TrainMovementUpdator(currentStation, nextStation)); // 열차 출발
            
            // 열차 도착
            currentStation = nextStation;

            // 내리고 싶은 승객들 내리기
            foreach (IPassenger passenger in new List<IPassenger>(passengers)) // 루프 내에서 remove 사용 때문에 임시로 사용
            {
                passenger.OnBoarding(this, currentStation);
                if (passenger.WantToGetOff)
                {
                    passenger.GetOff(currentStation);
                    passengers.Remove(passenger);
                }
            }
        }

    }

    IEnumerator TrainMovementUpdator(Station currentStation, Station nextStation)
    {
        Vector3 direction = nextStation.transform.position - currentStation.transform.position;
        transform.right = direction;

        float timer = 0;
        while (true)
        {
            if (timer >= 1f)
            {
                transform.position = nextStation.transform.position;
                break;
            }

            transform.position = Vector3.Lerp(currentStation.transform.position, nextStation.transform.position, timer);
            timer += Time.deltaTime;
            yield return null;
        }
    }
    
}
