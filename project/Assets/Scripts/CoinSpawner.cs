using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public List<KeyValuePair<bool, AnimatorCallback>> Coins = new List<KeyValuePair<bool, AnimatorCallback>>(); // is coin free
    public int CoinsAmount;

    public GameObject CoinPrefab;

    public void SpawnCoin(RectTransform parent)
    {
        AnimatorCallback freeCoin = null;
        for (int i = 0; i < Coins.Count; i++)
        {
            if (Coins[i].Key)
            {
                freeCoin = Coins[i].Value;
                Coins[i] = new KeyValuePair<bool, AnimatorCallback>(false, Coins[i].Value);
                break;
            }
        }

        if (freeCoin == null)
        {
            GameObject tempCoin = Instantiate(CoinPrefab);
            tempCoin.name = Guid.NewGuid().ToString();
            Coins.Add(new KeyValuePair<bool, AnimatorCallback>(false, tempCoin.GetComponent<AnimatorCallback>()));
            freeCoin = Coins[Coins.Count - 1].Value;
        }

        freeCoin.Callback = () => OnCoinAnimated(freeCoin);
        freeCoin.gameObject.SetActive(true);

        Transform freeCoinTr = freeCoin.transform;
        freeCoinTr.SetParent(parent);
        freeCoinTr.localPosition = Vector3.zero;
        freeCoinTr.localScale = Vector3.one;
        freeCoinTr.SetSiblingIndex(parent.childCount);
        if (parent.localScale.x < 0)
            freeCoinTr.localScale =
                new Vector3(-freeCoinTr.localScale.x, freeCoinTr.localScale.y, freeCoinTr.localScale.z);
    }

    private void OnCoinAnimated(AnimatorCallback freeCoin)
    {
        freeCoin.Callback = null;
        for (int i = 0; i < Coins.Count; i++)
        {
            if (Coins[i].Value.name.Equals(freeCoin.name))
            {
                Coins[i] = new KeyValuePair<bool, AnimatorCallback>(true, Coins[i].Value);
                freeCoin.gameObject.SetActive(false);
                break;
            }
        }
    }
}
