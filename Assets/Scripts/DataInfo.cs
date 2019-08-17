using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataInfo
{
    public const int oppositeRisk = 5;

    public const int toiletRisk = 10;
    public const int toiletHygiene = 30;

    public const int beverageVendingMachineRisk = 4;
    public const int beverageVendingMachineMoisture = 20;
    public const int beverageVendingMachineMoney = 100;

    public const int snackVendingMachineRisk = 6;
    public const int snackVendingMachineSatiety = 30;
    public const int snackVendingMachineMoney = 150;

    public const int beggingRisk = 30;
    public const int beggingMoney = 300;

    public const int playerStartMoney = 300;
    public const int playerStartSatiety = 80;
    public const int playerStartMoisture = 70;
    public const int playerStartHygiene = 90;
    public const int playerStartRisk = 0;
    public const bool playerStartMeeting = false;

    public const int oppositeTime = 1;
    public const int toiletTime = 3;
    public const int beverageTime = 3;
    public const int snackTime = 3;
    public const int beggingTime = 3;

    public const float lossRiskPerTime = 0.267f;
    public const float lossSatietyPerTime = 0.4f;
    public const float lossMoisturePerTime = 0.2f;
    public const float lossHygienePerTime = 0.2f;
}
