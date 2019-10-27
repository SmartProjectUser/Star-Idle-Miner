using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User
{
    public double UserMoneyBalnce;
    public double UserCryptoBalnce;
    public BalanceData BalanceData;

    public User(BalanceData balanceData)
    {
        BalanceData = balanceData;
        UserMoneyBalnce = balanceData.UserStartDollarAmount;
        UserCryptoBalnce = balanceData.UserStartCryptoAmount;
    }
}
