using UnityEngine;

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
    //성별
    public bool isMan;

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
        isRiding = true;
        stationInfo = null;
        station.Out(this);

        int idx = isMan ? 0 : 1;
        GameManager.instance.showers[idx].HideAction();
    }

    public void GetOff(Station station)
    {
        Debug.Log($"{name}이 {station.name} 에서 열차를 내림");
        transform.SetParent(station.transform, false);
        transform.localPosition = Vector3.zero;
        WantToGetOff = false;
        isRiding = false;
        stationInfo = station;
        station.Enter(this);

        int idx = isMan ? 0 : 1;
        GameManager.instance.showers[idx].ShowAction();
    }

    public void OnBoarding(Train train, Station station)
    {
        Debug.Log($"{name}은 {train.name}을 타고 {station}에 도착했음 (아직 내리지 않은 상태)");
    }

}
