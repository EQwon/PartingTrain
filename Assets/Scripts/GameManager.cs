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

    int previousDay;
    bool playerMeeting = DataInfo.playerStartMeeting;

    public float RiskSum => players[0].Risk + players[1].Risk;
    public int MaxAgent => (int)(RiskSum / 20f) + 2;
    public int NumOfAgent => agents.Count;

    public GameObject GameOverPanel;

    [SerializeField] Agent agentPrefab;
    List<Agent> agents = new List<Agent>();
    Station[] stations;
    Coroutine dayClockCoroutine;

    protected override void Awake()
    {
        base.Awake();
        stations = FindObjectsOfType<Station>();
        
        timeSpan = new TimeSpan();
        timeSpan += TimeSpan.FromHours(5);
        previousDay = timeSpan.Days;
    }
    
    private void Start()
    {
        dayClockCoroutine = StartCoroutine(DayClockCoroutine());
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

    public void Meeting()
    {
        Debug.Log("부부가 서로 만났어요");
        playerMeeting = true;
    }

    //탑승예약
    public void GetIn(bool _isMan, Action _finishAction = null)
    {
        int idx = _isMan ? 0 : 1;

        players[idx].WantToGetIn = !players[idx].WantToGetIn;

        _finishAction?.Invoke();
    }

    //하차예약
    public void GetOut(bool _isMan, Action _finishAction = null)
    {
        int idx = _isMan ? 0 : 1;

        players[idx].WantToGetOff = !players[idx].WantToGetOff;
        //players[idx].WantToGetOff = true;

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

        players[idx].Toilet();

        SpendTime(idx, DataInfo.toiletTime);
        UIManager.instance.playerStatus.StatusRefresh("Hygine", players[idx]);
        UIManager.instance.playerStatus.StatusRefresh("Risk", players[idx]);

        _finishAction?.Invoke();
    }

    //음료수 자판기
    public void BeverageVendingMachine(bool _isMan, Action _finishAction = null)
    {
        int idx = _isMan ? 0 : 1;

        players[idx].BeverageVendingMachine();

        SpendTime(idx, DataInfo.beverageTime);
        UIManager.instance.playerStatus.StatusRefresh("Moisture", players[idx]);
        UIManager.instance.playerStatus.StatusRefresh("Risk", players[idx]);
        UIManager.instance.playerStatus.StatusRefresh("Money", players[idx]);

        _finishAction?.Invoke();
    }

    //과자 자판기
    public void SnackVendingMachine(bool _isMan, Action _finishAction = null)
    {
        int idx = _isMan ? 0 : 1;

        players[idx].SnackVendingMachine();

        SpendTime(idx, DataInfo.snackTime);
        UIManager.instance.playerStatus.StatusRefresh("Satiety", players[idx]);
        UIManager.instance.playerStatus.StatusRefresh("Risk", players[idx]);
        UIManager.instance.playerStatus.StatusRefresh("Money", players[idx]);

        _finishAction?.Invoke();
    }
    
    //구걸
    public void Begging(bool _isMan, Action _finishAction = null)
    {
        int idx = _isMan ? 0 : 1;

        players[idx].Begging();

        SpendTime(idx, DataInfo.beggingTime);

        UIManager.instance.playerStatus.StatusRefresh("Money", players[idx]);
        UIManager.instance.playerStatus.StatusRefresh("Risk", players[idx]);

        _finishAction?.Invoke();
    }

    private bool CheckGameOver()
    {
        foreach (Player player in players)
        {
            if (player.Satiety <= 0 || player.Moisture <= 0 || player.Hygine <= 0)
            {
                GameOverPanel.SetActive(true);
                return true;
            }
        }
        return false;
    }

    public void CatchPlayer()
    {
        GameEnd();
    }

    void GameEnd()
    {
        StopCoroutine(dayClockCoroutine);
        Time.timeScale = 0;
        GameOverPanel.SetActive(true);
    }

    private IEnumerator DayClockCoroutine()
    {
        while (true)
        {
            yield return null;

            if (previousDay != timeSpan.Days)
            {
                if (!playerMeeting)
                {
                    GameOverPanel.SetActive(true);
                    Debug.Log("하루가 지났지만, 부부는 한번도 만나지 못했습니다");
                    GameEnd();
                }
                else
                {
                    playerMeeting = false;
                    Debug.Log("부부가 한번이라도 만났어요");
                }
            }

            previousDay = timeSpan.Days;
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

            if (CheckGameOver())
            {
                GameEnd();
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
                if (distance >= 80)
                    continue;

                isValid = false;
            }

            if (isValid)
                return randomStation;
        }
    }
}
