using System.Collections;
using UnityEngine;

public class Agent : MonoBehaviour, IPassenger
{
    //반대방향
    public bool isOpposite;
    
    public bool WantToGetOff { get; set; }
    public bool WantToGetIn { get; set; }
    public bool IsOpposite => isOpposite;
    public Transform Transform => transform;

    private void Update()
    {
        transform.rotation = Quaternion.identity;
    }

    public void Init(Station station)
    {
        transform.SetParent(station.transform);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        WantToGetIn = true;
        station.Enter(this);
    }

    public void GetIn(Train train, Station station)
    {
        Debug.Log($"{name}이 {station.name} 에서 {train} 열차를 탐");
        transform.SetParent(train.transform);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        WantToGetIn = false;
        station.Out(this);

        float randomDelay = Random.Range(8f, 10f) / (3f - GameManager.instance.RiskSum / 100f);
        Invoke(nameof(AgentGetInDelay), randomDelay);
    }

    public void GetOff(Station station)
    {
        Debug.Log($"{name}이 {station.name} 에서 열차를 내림");
        WantToGetOff = false;
        station.Enter(this);

        //(필드에 존재하는 요원 수) > (필드에 있어야 하는 요원 수)라면 Destory()
        if (GameManager.instance.NumOfAgent > GameManager.instance.MaxAgent)
            GameManager.instance.RemoveAgent(this);

        float randomDelay = Random.Range(8f, 10f) / (1f + GameManager.instance.RiskSum / 100f);
        Invoke(nameof(AgentGetOffDelay), randomDelay);

        int random = Random.Range(0, 2);
        if (random == 1)
        {
            isOpposite = !isOpposite;
            station.Refresh();
        }
    }

    public void OnBoarding(Train train, Station station)
    {
        Debug.Log($"{name}은 {train.name}을 타고 {station}에 도착했음 (아직 내리지 않은 상태) {WantToGetIn} {WantToGetOff}");
    }

    void AgentGetInDelay()
    {
        WantToGetOff = true;
    }

    void AgentGetOffDelay()
    {
        WantToGetIn = true;
    }

}
