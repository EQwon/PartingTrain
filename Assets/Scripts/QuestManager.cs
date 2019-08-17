using System;
using System.Collections.Generic;
using UnityEngine;
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
    
    public void TriggerQuest(Player.PlayerAction action)
    {
        List<Quest> validActionQuests = quests.FindAll(q => q.trigger.action == action);
        
        validActionQuests.Sort((q1, q2) => q2.trigger.probability.CompareTo(q1.trigger.probability));

        foreach (Quest quest in validActionQuests)
        {
            float random = Random.value * 100f;
            
            if(quest.trigger.probability < random)
                continue;
            
            LaunchQuest(quest);
            break;
        }
    }

    public void LaunchQuest(Quest quest)
    {
        Debug.Log($"{quest}를 실행합니다");
    }
}