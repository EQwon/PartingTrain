using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour
{
    [SerializeField] List<Station> stations;
    [SerializeField] bool reverse;
    [SerializeField] bool branchLine;
    [SerializeField] float trainSpeed = 1f;

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
        int count = 0;
        Queue<Station> stationQueue = new Queue<Station>(stations);

        if (stationQueue.Count <= 0)
            yield break;
        
        Station currentStation = stationQueue.Dequeue();
        transform.position = currentStation.transform.position;
        
        while (true)
        {
            if (stationQueue.Count <= 0)
            {
                count = 0;
                stationQueue = new Queue<Station>(stations);
            }

            if (branchLine && (count == 1 || count == 5))
                reverse = !reverse;
            
            Station nextStation = stationQueue.Dequeue();
            count++;

            // 열차 출발 전

            // 타고 싶은 승객들 태우기
            foreach (IPassenger passenger in currentStation.GetBoardingPassengers)
            {

                if (reverse && passenger.IsOpposite)
                {
                    passenger.GetIn(this, currentStation);
                    passengers.Add(passenger);
                }
                else if (!reverse && !passenger.IsOpposite)
                {
                    passenger.GetIn(this, currentStation);
                    passengers.Add(passenger);
                }
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
        Vector3 direction = nextStation.transform.position - transform.position;

        float dot = Vector3.Dot(transform.right, direction);
        if(dot < 0)
            transform.right = direction;
        Quaternion targetQuaternion = Quaternion.FromToRotation(Vector3.right, direction);
        float timer = 0;
        while (true)
        {
            if (timer >= 1f)
            {
                transform.position = nextStation.transform.position;
                break;
            }
            transform.position = Vector3.Lerp(currentStation.transform.position, nextStation.transform.position, timer);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetQuaternion, Time.deltaTime * 10);
            timer += Time.deltaTime * trainSpeed;
            yield return null;
        }
    }
    
    static float QuadraticEaseInOut(float p)
    {
        if(p < 0.5f)
        {
            return 2 * p * p;
        }
        else
        {
            return (-2 * p * p) + (4 * p) - 1;
        }
    }

}
