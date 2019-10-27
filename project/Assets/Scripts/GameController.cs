using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using View;

public class GameController : Singleton<GameController>
{
    public BalanceData UserBalanceData;
    public User User;
    public List<AbstractMiningView> MiningPrefabs;
    public UiController UiController;
    public CoinSpawner CoinSpawner;
    public SoundController SoundController;
    
    void Awake()
    {
        User = new User(UserBalanceData);
        UiController.userMoneyField.Init(User.UserMoneyBalnce);
        UiController.userCryptoField.Init(User.UserCryptoBalnce);
//        for (int i = 0; i < User.BalanceData.UserOwnedMinings.Count; i++)
//        {
//            for (int j = 0; j < MiningPrefabs.Count; j++)
//            {
//                if (User.BalanceData.UserOwnedMinings[i].Equals(MiningPrefabs[j].Id))
//                {
//                }
//            }
//        }
        
    }

    public void UpdateUserMoneyBalance(double amount)
    {
        User.UserMoneyBalnce += amount;
        UiController.userMoneyField.UpdateScore(User.UserMoneyBalnce);
    }
    
    public void UpdateUserCryptoBalance(double amount)
    {
        User.UserCryptoBalnce += amount;
        UiController.userCryptoField.UpdateScore(User.UserCryptoBalnce);
    }
    
    //public void BuyNewMining(string id)

    private void SpawnUserMining()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
