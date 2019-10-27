using System;
using System.Collections;
using UnityEngine;

namespace View
{
    public abstract class AbstractMiningView : MonoBehaviour
    {
        public const float UpdateTime = 1.0f;
        
        public string Name;
        public double CryptoMiningPerSecond;
        public float ShowCoinEvery;
        private RectTransform RectTransform;
        public bool Pause;

        private void Start()
        {
            RectTransform = GetComponent<RectTransform>();
            DoStart();
        }

        protected virtual void DoStart()
        {
        }

        protected void StartMining()
        {
            StartCoroutine(UpdateMining());
            StartCoroutine(ShowCoin());
        }

        private IEnumerator ShowCoin()
        {
            yield return new WaitUntil(()=>!Pause);
            yield return new WaitForSeconds(ShowCoinEvery);
            yield return new WaitUntil(()=>!Pause);
            GameController.Instance.CoinSpawner.SpawnCoin(RectTransform);
            StartCoroutine(ShowCoin());
        }

        private IEnumerator UpdateMining()
        {
            yield return new WaitUntil(()=>!Pause);
            GameController.Instance.UpdateUserCryptoBalance(CryptoMiningPerSecond);
            yield return new WaitUntil(()=>!Pause);
            yield return new WaitForSeconds(UpdateTime);
            StartCoroutine(UpdateMining());
        }
    }
}