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
}

public class QuestManager : Singleton<QuestManager>
{
    [SerializeField] List<Quest> quests;
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
        yield return new WaitForSeconds(1);
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
            return value >= 0 ? $"+{1-value}%" : $"-{1-value}";
        }
    }
}