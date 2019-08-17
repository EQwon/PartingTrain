using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[Serializable]
public struct QuestReward
{
    public Player.PlayerStatus status;
    public float value;
}

[Serializable]
public struct QuestTrigger
{
    public Player.PlayerAction action;
    public float probability;
}

[Serializable]
public struct Quest
{
    public string title;
    public string description;
    
    public QuestTrigger trigger;
    public List<QuestReward> rewards;

    public static Quest RawToQuest(QuestRawData raw)
    {
        Debug.Log(raw.ToString());
        Quest quest = new Quest
        {
            title = raw.title,
            description = raw.description,
            trigger = new QuestTrigger
            {
                action = (Player.PlayerAction)Enum.Parse(typeof(Player.PlayerAction), raw.action),
                probability = raw.probability
            },
            rewards = new List<QuestReward>()
        };
        
        if(raw.hygiene != 0)
            quest.rewards.Add(new QuestReward{status = Player.PlayerStatus.Hygiene, value = raw.hygiene});
        
        if(raw.moisture != 0)
            quest.rewards.Add(new QuestReward{status = Player.PlayerStatus.Moisture, value = raw.moisture});
        
        if(raw.risk != 0)
            quest.rewards.Add(new QuestReward{status = Player.PlayerStatus.Risk, value = raw.risk});
        
        if(raw.satiety != 0)
            quest.rewards.Add(new QuestReward{status = Player.PlayerStatus.Satiety, value = raw.satiety});
        
        if(raw.money != 0)
            quest.rewards.Add(new QuestReward{status = Player.PlayerStatus.Money, value = raw.money});

        return quest;
    }
}

[Serializable]
public struct QuestRawData
{
    public string title;
    public string description;
    public string action;
    public float probability;
    public float money;
    public float risk;
    public float satiety;
    public float moisture;
    public float hygiene;

    public override string ToString()
    {
        return $"{title} {description} {action} {probability} {money} {risk} {satiety} {moisture} {hygiene}";
    }
}

[Serializable]
public class QuestRawWrapper
{
    public QuestRawData[] data;
}

public class QuestManager : Singleton<QuestManager>
{
    List<Quest> quests = new List<Quest>();
    [SerializeField] Transform questPanel;

    Text title;
    Text description;
    Text valueChanged;

    protected override void Awake()
    {
        base.Awake();
        title = questPanel.Find("Title").GetComponent<Text>();
        description = questPanel.Find("Description").GetComponent<Text>();
        valueChanged = questPanel.Find("ValueChange").GetComponent<Text>();

        questPanel.gameObject.SetActive(false);
        
        LoadQuestData();
    }

    void LoadQuestData()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("quest");

        Debug.Log(textAsset.text);
        QuestRawWrapper rawDatas = JsonUtility.FromJson<QuestRawWrapper>(textAsset.text);

        foreach (QuestRawData rawData in rawDatas.data)
        {
            quests.Add(Quest.RawToQuest(rawData));
        }
    }

    public void TriggerQuest(Player player, Player.PlayerAction action)
    {
        List<Quest> validActionQuests = quests.FindAll(q => q.trigger.action == action);

        validActionQuests.Sort((q1, q2) => q2.trigger.probability.CompareTo(q1.trigger.probability));

        foreach (Quest quest in validActionQuests)
        {
            float random = Random.value * 100f;

            if (quest.trigger.probability < random)
                continue;

            player.LaunchQuest(quest);
            LaunchUI(quest);
            break;
        }
    }

    void LaunchUI(Quest quest)
    {
        title.text = quest.title;
        description.text = quest.description;
        valueChanged.text = "";

        foreach (QuestReward reward in quest.rewards)
        {
            valueChanged.text += $"{StatusEnumToString(reward.status)} {RewardValueToString(reward.status, reward.value)} ";
        }

        StartCoroutine(nameof(UIUpdator));
    }

    IEnumerator UIUpdator()
    {
        questPanel.gameObject.SetActive(true);
        yield return new WaitForSeconds(5);
        questPanel.gameObject.SetActive(false);
    }

    static string StatusEnumToString(Player.PlayerStatus status)
    {
        switch (status)
        {
            case Player.PlayerStatus.Money:
                return "돈";
            case Player.PlayerStatus.Satiety:
                return "포만감";
            case Player.PlayerStatus.Moisture:
                return "수분";
            case Player.PlayerStatus.Hygiene:
                return "위생";
            case Player.PlayerStatus.Risk:
                return "위험도";
            default: return "";
        }
    }

    static string RewardValueToString(Player.PlayerStatus status, float value)
    {
        if (status == Player.PlayerStatus.Money)
        {
            return value >= 0 ? $"+{value}" : $"{value}";
        }
        else
        {
            return value >= 0 ? $"+{(1-value)*100f}%" : $"-{(1-value)*100f}";
        }
    }
}