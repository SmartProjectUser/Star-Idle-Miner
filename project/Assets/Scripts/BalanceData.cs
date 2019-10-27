using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Main Balance", menuName = "Balance/Main Balance", order = 51)]
public class BalanceData : ScriptableObject
{
    public int UserStartDollarAmount;
    public int UserStartCryptoAmount;
    //public List<BaseMiningBalance> UserOwnedMinings;
}
