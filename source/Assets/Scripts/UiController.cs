using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
    public Counter userMoneyField;
    public Counter userCryptoField;
    public Button OpenStocksButton;
    public Button CloseStocksButton;
    public Button Clean;
    public Text CleanText;
    public double CleanPrice;
    public Animator CleanAnim;
    public Animator TrashAnim;
    public Animator DecorAnim;
    public GameObject Loader;
    public GameObject Stocks;

    private void Start()
    {
        OpenStocksButton.onClick.AddListener(OpenStocks);
        CloseStocksButton.onClick.AddListener(CloseStocks);
        Clean.onClick.AddListener(ButtonClean);
        CleanText.text = CleanPrice.ToString();
    }

    private void OpenStocks()
    {
        Loader.SetActive(true);
        StartCoroutine(WaitForLoader());
        StartCoroutine(HalfLoaderShowed(() =>
        {
            Stocks.gameObject.SetActive(true);
            GameController.Instance.SoundController.FadeMain();
            GameController.Instance.SoundController.PlayStocks();
        }));
    }

    private IEnumerator WaitForLoader()
    {
        yield return new WaitForSeconds(1.0f);
        Loader.SetActive(false);
    }

    private IEnumerator HalfLoaderShowed(Action callback)
    {
        yield return new WaitForSeconds(0.5f);
        callback.Invoke();
    }

    private void CloseStocks()
    {
        Loader.SetActive(true);
        StartCoroutine(WaitForLoader());
        StartCoroutine(HalfLoaderShowed(() =>
        {
            Stocks.gameObject.SetActive(false);
            GameController.Instance.SoundController.FadeStock();
            GameController.Instance.SoundController.PlayMain();
        }));
    }

    private void ButtonClean()
    {
        if (GameController.Instance.User.UserMoneyBalnce >= CleanPrice)
        {
            TrashAnim.enabled = true;
            DecorAnim.gameObject.SetActive(true);
            CleanAnim.enabled = true;
            GameController.Instance.UpdateUserMoneyBalance(-CleanPrice);
        }
    }
}
