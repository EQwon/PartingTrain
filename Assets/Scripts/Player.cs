using System;
using UnityEngine;

public class Player : MonoBehaviour, IPassenger
{
    [SerializeField] Station startStation;

    #region Status
    
    //돈
    public float Money {
        get { return money; }
        set {
            if (value <= 0)
                value = 0;
            else
                money = value;
        }
    }
    //포만감
    public float Satiety {
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
    public float Moisture {
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
    public float Hygine {
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
    public float Risk {
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

    float money;
    float satiety;
    float moisture;
    float hygiene;
    float risk;
    
    #endregion

    public enum PlayerStatus
    {
        Money,
        Satiety,
        Moisture,
        Hygiene,
        Risk
    };

    public enum PlayerAction
    {
        GetIn,
        GetOut,
        Opposite,
        OnBoarding,
        Toilet,
        Beverage,
        Snack,
        Begging
    };

    
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
    public Transform Transform => transform;

    private void Start()
    {
        //초기값 설정
        risk = DataInfo.playerStartRisk;
        money = DataInfo.playerStartMoney;
        moisture = DataInfo.playerStartMoisture;
        hygiene = DataInfo.playerStartHygiene;
        satiety = DataInfo.playerStartSatiety;

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
        transform.localRotation = Quaternion.identity;
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
        transform.localRotation = Quaternion.identity;
        WantToGetIn = false;
        isRiding = true;
        stationInfo = null;
        station.Out(this);

        int idx = isMan ? 0 : 1;
        UIManager.instance.showers[idx].RideAction();
        QuestManager.instance.TriggerQuest(this, PlayerAction.GetIn);
    }

    public void GetOff(Station station)
    {
        Debug.Log($"{name}이 {station.name} 에서 열차를 내림");
        WantToGetOff = false;
        isRiding = false;
        stationInfo = station;
        station.Enter(this);

        int idx = isMan ? 0 : 1;
        UIManager.instance.showers[idx].QuitAction(stationInfo);
        QuestManager.instance.TriggerQuest(this, PlayerAction.GetOut);
    }

    public void Opposite()
    {
        Risk += DataInfo.oppositeRisk;
        isOpposite = !isOpposite;
        stationInfo.Refresh();
        QuestManager.instance.TriggerQuest(this, PlayerAction.Opposite);
    }

    public void OnBoarding(Train train, Station station)
    {
        Debug.Log($"{name}은 {train.name}을 타고 {station}에 도착했음 (아직 내리지 않은 상태) {WantToGetIn} {WantToGetOff}");

        if (station.IsPlayerInStation)
        {
            GameManager.instance.Meeting();
        }
        QuestManager.instance.TriggerQuest(this, PlayerAction.OnBoarding);
    }

    public void Toilet()
    {
        Hygine += DataInfo.toiletHygiene;
        Risk += DataInfo.toiletRisk;
        QuestManager.instance.TriggerQuest(this, PlayerAction.Toilet);
    }

    public void BeverageVendingMachine()
    {
        if (CanBuy(DataInfo.beverageVendingMachineMoney))
        {
            Money -= DataInfo.beverageVendingMachineMoney;
        }
        else
        {
            return;
        }

        Moisture += DataInfo.beverageVendingMachineMoisture;
        Risk += DataInfo.beverageVendingMachineRisk;
        QuestManager.instance.TriggerQuest(this, PlayerAction.Beverage);
    }

    public void SnackVendingMachine()
    {
        if (CanBuy( DataInfo.snackVendingMachineMoney))
        {
            Money -= DataInfo.snackVendingMachineMoney;
        }
        else
        {
            return;
        }

        Satiety += DataInfo.snackVendingMachineSatiety;
        Risk += DataInfo.snackVendingMachineRisk;
        QuestManager.instance.TriggerQuest(this, PlayerAction.Snack);
    }

    public void Begging()
    {
        Money += DataInfo.beggingMoney;
        Risk += DataInfo.beggingRisk;
        QuestManager.instance.TriggerQuest(this, PlayerAction.Begging);
    }
    
    bool CanBuy(int cost)
    {
        if(Money >= cost)
        {
            return true;
        }

        return false;
    }

    public void LaunchQuest(Quest quest)
    {
        Debug.Log($"{quest}를 실행합니다");

        foreach (QuestReward reward in quest.rewards)
        {
            switch (reward.status)
            {
                case PlayerStatus.Money:
                    Money += reward.value;
                    break;
                case PlayerStatus.Satiety:
                    Satiety *= reward.value;
                    break;
                case PlayerStatus.Moisture:
                    Moisture *= reward.value;
                    break;
                case PlayerStatus.Hygiene:
                    Hygine *= reward.value;
                    break;
                case PlayerStatus.Risk:
                    Risk *= reward.value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
    
}
