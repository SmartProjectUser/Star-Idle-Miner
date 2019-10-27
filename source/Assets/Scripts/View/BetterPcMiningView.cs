using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace View
{
    public class BetterPcMiningView : AbstractMiningView
    {
        public Text PriceText;
        public Animator Animator;
        public Image Lamp;
        public Color LampColor;
        public Button Button;
        public double Price;
        public double PriceIncrement;
        public bool IsUnlocked;

        private Color LampInitColor;
        private bool IsAnimating;
        private bool IsUserCanBuy;
        
        protected override void DoStart()
        {
            base.DoStart();
            LampInitColor = Lamp.color;

            if (IsUnlocked)
            {
                Animator.SetTrigger("unlock_fast");
                StartCoroutine(ColorChange(Lamp.color, LampColor, 0.0f));
                Destroy(PriceText);
                return;
            }

            PriceText.text = Price.ToString();
            Button.onClick.AddListener(OnButtonClick);
            StartCoroutine(PriceCheckerService());
        }

        private void OnButtonClick()
        {
            if (IsAnimating)
                return;

            if (GameController.Instance.User.UserMoneyBalnce >= Price)
            {
                Animator.SetTrigger("unlock");
                GameController.Instance.UpdateUserMoneyBalance(-Price);
                StartCoroutine(ColorChange(Lamp.color, LampColor, 0.5f, callback: () =>
                {
                    LampInitColor = Lamp.color;
                }));
                StartCoroutine(WaitAndStartMining());
                Price *= PriceIncrement;
            }
            else
            {
                IsAnimating = true;
                Animator.SetTrigger("no");
                StartCoroutine(ColorChange(Lamp.color, new Color(0.75f, 0.0f, 0.0f, Lamp.color.a), 0.5f,
                    callback: () =>
                    {
                        StartCoroutine(ColorChange(Lamp.color, LampInitColor, 0.5f,
                            callback: () => { IsAnimating = false; }));
                    }));
            }
        }

        private IEnumerator WaitAndStartMining()
        {
            yield return new WaitForSeconds(1.0f);
            StartMining();
        }

        private void OnPriceUpdate()
        {
            if (PriceText == null)
                return;
            
            PriceText.text = Price.ToString();
        }


        private IEnumerator ColorChange(Color from, Color to, float duration, float time = 0, float timeWasted = 0,
            Action callback = null)
        {
            if (timeWasted >= duration)
            {
                callback?.Invoke();
                Lamp.color = to;
                yield break;
            }

            timeWasted += Time.deltaTime;
            Lamp.color = Color.Lerp(from, to, time);

            if (time < 1)
            {
                time += Time.deltaTime / duration;
            }


            yield return null;
            StartCoroutine(ColorChange(from, to, duration, time, timeWasted, callback));
        }

        private IEnumerator PriceCheckerService()
        {
            yield return new WaitForSeconds(Random.Range(0, 10));
            IsUserCanBuy = GameController.Instance.User.UserMoneyBalnce >= Price;
            if (IsUserCanBuy && !IsAnimating)
            {
                Animator.SetTrigger("ready");
            }

            StartCoroutine(PriceCheckerService());
        }

        private void OnValidate()
        {
            if (Lamp == null)
                Lamp = transform.Find("lamp").GetComponent<Image>();
            if (Button == null)
                Button = GetComponentInChildren<Button>();
        }
    }
}