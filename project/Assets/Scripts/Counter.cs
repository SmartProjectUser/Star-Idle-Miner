using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class Counter : MonoBehaviour
{
    [SerializeField] private float duration = 2.5f;
    [SerializeField] private double score;
    [SerializeField] private Text scoreText;

    public void Init(double initScore)
    {
        score = initScore;
        scoreText.text = $"{score:0.00}";
    }

    public void UpdateScore(double to)
    {
        StartCoroutine(CountTo(to));
    }

    IEnumerator CountTo(double target)
    {
        double start = score;
        for (float timer = 0; timer < duration; timer += Time.deltaTime)
        {
            float progress = timer / duration;
            score = Lerp(start, target, progress);
            scoreText.text = $"{score:0.00}";
            yield return null;
        }

        score = target;
        scoreText.text = $"{score:0.00}";
    }
    
    private double Lerp(double firstFloat, double secondFloat, double by)
    {
        return firstFloat * (1 - by) + secondFloat * by;
    }
}
