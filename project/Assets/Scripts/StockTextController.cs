using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

namespace DefaultNamespace
{
    public class StockTextController : MonoBehaviour
    {
        private List<KeyValuePair<bool, Text>> Text = new List<KeyValuePair<bool, Text>>(); // is coin free
        //private List<>

        public GameObject TextPrefab;

        public float DelayBeforeStart;
        public float DelayBeforeSpawn;
        public float MinAmount;
        public float DurationOfMovement;
        public bool IsBackWards;
        public float RandomMinValue;
        public float RandomMaxValue;
        public Color RandomMinColor;
        public Color RandomMaxColor;
        private RectTransform RectTransform;

        private IEnumerator Start()
        {
            RectTransform = GetComponent<RectTransform>();
            
            yield return new WaitForSeconds(DelayBeforeStart);
            for (int i = 0; i < MinAmount; i++)
            {
                SpawnText(RectTransform);
                yield return new WaitForSeconds(DelayBeforeSpawn);
            }
            //StartCoroutine(SpawnTexts());
        }

        private void SpawnText(RectTransform parent)
        {
            Text freeText = null;
            for (int i = 0; i < Text.Count; i++)
            {
                if (Text[i].Key)
                {
                    freeText = Text[i].Value;
                    Text[i] = new KeyValuePair<bool, Text>(false, Text[i].Value);
                    break;
                }
            }

            if (freeText == null)
            {
                GameObject tempText = Instantiate(TextPrefab);
                tempText.name = Guid.NewGuid().ToString();
                Text.Add(new KeyValuePair<bool, Text>(false, tempText.GetComponent<Text>()));
                freeText = Text[Text.Count - 1].Value;
            }

            //freeText.gameObject.SetActive(true);

            Transform freeCoinTr = freeText.transform;
            freeCoinTr.SetParent(parent);
            freeCoinTr.localPosition = Vector3.zero;
            freeCoinTr.localScale = Vector3.one;

            StockTextView temp = freeCoinTr.gameObject.AddComponent<StockTextView>();
            float from = -freeText.rectTransform.rect.width / 2;
            float to = Screen.width + freeText.rectTransform.rect.width / 2;
            if (IsBackWards)
            {
                temp.fromX = to;
                temp.toX = from;
            }
            else
            {
                temp.fromX = from;
                temp.toX = to;   
            }
            temp.RectTransform = temp.GetComponent<RectTransform>();
            temp.duration = DurationOfMovement;
            temp.RandomMinValue = RandomMinValue;
            temp.RandomMaxValue = RandomMaxValue;
            temp.RandomMinColor = RandomMinColor;
            temp.RandomMaxColor = RandomMaxColor;
            temp.Text = freeText;
            temp.IsBackwards = IsBackWards;
            temp.Init();
            
            temp.gameObject.SetActive(true);
            temp.RectTransform.anchoredPosition = new Vector3(temp.fromX, temp.RectTransform.anchoredPosition.y);
            //StartCoroutine(WaitForSpawn(freeText));
        }

        private IEnumerator WaitForSpawn(Text freeText)
        {
            yield return null;
            
            freeText.gameObject.SetActive(true);
            float from = -freeText.rectTransform.rect.width / 2;
            float to = Screen.width + freeText.rectTransform.rect.width / 2;
            freeText.StartCoroutine(MoveText(freeText.rectTransform, from, to, 4.0f,
                callback: () => { OnTextMovedFinished(freeText); }));
        }

        private void OnTextMovedFinished(Text freeText)
        {
            freeText.StopAllCoroutines();
            for (int i = 0; i < Text.Count; i++)
            {
                if (Text[i].Value.name.Equals(freeText.name))
                {
                    Text[i] = new KeyValuePair<bool, Text>(true, Text[i].Value);
                    freeText.gameObject.SetActive(false);
                    break;
                }
            }
            
            freeText.StopAllCoroutines();
            SpawnText(RectTransform);
        }

        private IEnumerator MoveText(RectTransform tr, float fromX, float toX, float duration, float time = 0,
            float timeWasted = 0,
            Action callback = null)
        {
            if (timeWasted >= duration)
            {
                callback?.Invoke();
                tr.anchoredPosition = new Vector3(toX, tr.anchoredPosition.y);
                yield break;
            }

            timeWasted += Time.deltaTime;
            tr.anchoredPosition = new Vector3(Mathf.Lerp(fromX, toX, time), tr.anchoredPosition.y);

            if (time < 1)
            {
                time += Time.deltaTime / duration;
            }


            yield return new WaitForEndOfFrame();
            StartCoroutine(MoveText(tr, fromX, toX, duration, time, timeWasted, callback));
        }
    }
}