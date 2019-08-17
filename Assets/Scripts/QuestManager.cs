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
public struct Quest
{
    public string title;
    public string description;
    public List<QuestReward> rewards;
}
