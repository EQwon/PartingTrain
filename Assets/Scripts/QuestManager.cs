using System;
using System.Collections.Generic;
using UnityEngine;

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
