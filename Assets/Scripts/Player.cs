using UnityEngine;

public class Player : MonoBehaviour, IPassenger
{
    [SerializeField] Station startStation;

    //돈
    public int Money {
        get { return money; }
        set {
            if (value <= 0)
                value = 0;
            else
                money = value;
        }
    }
    //포만감
    public int Satiety {
        get { return satiety; }
        set {
            if (value >= 100)
                satiety = value;
            else if (value <= 0)
                satiety = 0;
            else
                satiety = value;
        }
    }
    //수분
    public int Moisture {
        get { return moisture; }
        set {
            if (value >= 100)
                moisture = 100;
            else if (value <= 0)
                moisture = 0;
            else
                moisture = value;
        }
    }
    //위생
    public int Hygine {
        get { return hygiene; }
        set {
            if(value >= 100)
                hygiene = 100;
            else if (value <= 0)
                hygiene = 0;
            else
                hygiene = value;
        }
    }
    //위험도
    public int Risk {
        get { return risk; }
        set {
            if (value >= 100)
            { risk = 100; }
            else if (value <= 0)
            { risk = 0; }
            else
            { risk = value; }
        }
    }

    int money;
    int satiety;
    int moisture;
    int hygiene;
    int risk;


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
    public bool IsOpposite => isOpposite;

    private void Start()
    {
        //초기값 설정
        risk = DataInfo.playerStartRisk;
        money = DataInfo.playerStartMoney;
        moisture = DataInfo.playerStartMoisture;
        hygiene = DataInfo.playerStartHygiene;
        satiety = DataInfo.playerStartSatiety;
        meeting = DataInfo.playerStartMeeting;

        Init(startStation);
    }

    private void Update()
    {
        transform.rotation = Quaternion.identity;
    }

    public void Init(Station station)
    {
        transform.SetParent(station.transform);
        transform.localPosition = Vector3.zero;
        station.Enter(this);
        stationInfo = station;
        int idx = isMan ? 0 : 1;
        UIManager.instance.showers[idx].QuitAction(stationInfo);
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
        UIManager.instance.showers[idx].RideAction();
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
        UIManager.instance.showers[idx].QuitAction(stationInfo);
    }

    public void OnBoarding(Train train, Station station)
    {
        Debug.Log($"{name}은 {train.name}을 타고 {station}에 도착했음 (아직 내리지 않은 상태) {WantToGetIn} {WantToGetOff}");
    }

}
