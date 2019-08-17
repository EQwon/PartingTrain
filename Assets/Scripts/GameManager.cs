using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameManager : Singleton<GameManager>
{
    public Player[] players = new Player[2];

    private float time;

    private TimeSpan timeSpan;

    public float RiskSum => players[0].Risk + players[1].Risk;
    public int MaxAgent => (int)(RiskSum / 20f) + 1;
    public int NumOfAgent => agents.Count;

    [SerializeField] Agent agentPrefab;
    List<Agent> agents = new List<Agent>();
    Station[] stations;


    protected override void Awake()
    {
        base.Awake();
        stations = FindObjectsOfType<Station>();
        
        timeSpan = new TimeSpan();
        timeSpan += TimeSpan.FromHours(5);

    }
    
    private void Start()
    {
        StartCoroutine(DayClockCoroutine());
    }

    //역 갱신 Player의 GetIn GetOut에서 관리
//    public void StationRefresh(bool _isMan, Station _info, Action _finishAction = null)
//    {
//        int idx = _isMan ? 0 : 1;
//
//        players[idx].stationInfo = _info;
//
//        _finishAction?.Invoke();
//    }

    //탑승예약
    public void GetIn(bool _isMan, Action _finishAction = null)
    {
        int idx = _isMan ? 0 : 1;

        players[idx].WantToGetIn = true;

        _finishAction?.Invoke();
    }

    //하차예약
    public void GetOut(bool _isMan, Action _finishAction = null)
    {
        int idx = _isMan ? 0 : 1;

        players[idx].WantToGetOff = true;

        _finishAction?.Invoke();
    }

    //반대방향
    public void Opposite(bool _isMan, Action _finishAction = null)
    {
        int idx = _isMan ? 0 : 1;

        players[idx].Opposite();

        SpendTime(idx, DataInfo.oppositeTime);

        _finishAction?.Invoke();
    }

    //화장실
    public void Toilet(bool _isMan, Action _finishAction = null)
    {
        int idx = _isMan ? 0 : 1;

        players[idx].Hygine += DataInfo.toiletHygiene;
        players[idx].Risk += DataInfo.toiletRisk;

        SpendTime(idx, DataInfo.toiletTime);
        UIManager.instance.playerStatus.StatusRefresh("Hygine", players[idx]);

        _finishAction?.Invoke();
    }

    //음료수 자판기
    public void BeverageVendingMachine(bool _isMan, Action _finishAction = null)
    {
        int idx = _isMan ? 0 : 1;

        players[idx].Moisture += DataInfo.beverageVendingMachineMoisture;
        players[idx].Risk += DataInfo.beverageVendingMachineRisk;

        if (CanBuy(_isMan, DataInfo.beverageVendingMachineMoney))
        {
            players[idx].Money -= DataInfo.beverageVendingMachineMoney;
        }

        SpendTime(idx, DataInfo.beverageTime);
        UIManager.instance.playerStatus.StatusRefresh("Moisture", players[idx]);

        _finishAction?.Invoke();
    }

    //과자 자판기
    public void SnackVendingMachine(bool _isMan, Action _finishAction = null)
    {
        int idx = _isMan ? 0 : 1;

        players[idx].Satiety += DataInfo.snackVendingMachineSatiety;
        players[idx].Risk += DataInfo.snackVendingMachineRisk;

        if (CanBuy(_isMan, DataInfo.snackVendingMachineMoney))
        {
            players[idx].Money -= DataInfo.snackVendingMachineMoney;
        }

        SpendTime(idx, DataInfo.snackTime);
        UIManager.instance.playerStatus.StatusRefresh("Satiety", players[idx]);

        _finishAction?.Invoke();
    }
    
    //구걸
    public void Begging(bool _isMan, Action _finishAction = null)
    {
        int idx = _isMan ? 0 : 1;

        players[idx].Money += DataInfo.beggingMoney;
        players[idx].Risk += DataInfo.beggingRisk;

        SpendTime(idx, DataInfo.beggingTime);

        _finishAction?.Invoke();
    }

    private bool CanBuy(bool _isMan, int _cost)
    {
        int idx = _isMan ? 0 : 1;

        if(players[idx].Money >= _cost)
        {
            return true;
        }

        return false;
    }

    private void CheckGameOver()
    {
        //if(satiety <= 0 || moisture <= 0 || hygiene <= 0)
        //{
        //    Debug.Log("게임 오버");
        //}
    }

    private IEnumerator DayClockCoroutine()
    {
        while (true)
        {
            yield return null;

            timeSpan += TimeSpan.FromMinutes(Time.deltaTime * 4);
            
            for(int i = 0; i <players.Length; i++)
            {
                players[i].Risk -= DataInfo.lossRiskPerTime * Time.deltaTime;
                players[i].Moisture -= DataInfo.lossMoisturePerTime* Time.deltaTime;
                players[i].Hygine -= DataInfo.lossHygienePerTime* Time.deltaTime;
                players[i].Satiety -= DataInfo.lossSatietyPerTime* Time.deltaTime;
                UIManager.instance.playerStatus.AllStatusRefresh(players[i]);
            }

            if (agents.Count < MaxAgent)
            {
                SpawnAgent();
            }

            UIManager.instance.timeText.text = string.Format("{0:00} : {1:00}", timeSpan.Hours, timeSpan.Minutes);
        }
    }

    public void SpendTime(int _idx, int _time)
    {
        StartCoroutine(SpendTimeCoroutine(_idx, _time));
        //timeSpan += TimeSpan.FromMinutes(4 * _time);
    }

    private IEnumerator SpendTimeCoroutine(int _idx, int _time)
    {
        UIManager.instance.showers[_idx].gameObject.SetActive(false);

        yield return new WaitForSeconds(_time);

        UIManager.instance.showers[_idx].gameObject.SetActive(true);
    }

    public void SpawnAgent()
    {
        Agent agent = Instantiate(agentPrefab);
        agent.Init(GetRandomStationForAgentSpawn());
        agents.Add(agent);
    }

    public void RemoveAgent(Agent agent)
    {
        agents.Remove(agent);
        Destroy(agent.gameObject);
    }

    public Station GetRandomStationForAgentSpawn()
    {
        while (true)
        {
            Station randomStation = stations[UnityEngine.Random.Range(0, stations.Length)];

            bool isValid = true;
            foreach (Player player in players)
            {
                float distance = Vector3.Distance(player.transform.position, randomStation.transform.position);
                if (distance >= 200)
                    continue;

                isValid = false;
            }

            if (isValid)
                return randomStation;
        }
    }
}
