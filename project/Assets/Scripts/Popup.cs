using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    public Animator PopupAnimator;

    public Button Minus;
    public Button Plus;
    public Button Close;
    public Button Accept;

    public Text Info;
    public Text Btc;
    public Text Dollars;
    public AnimatorCallback AnimatorCallback;
    public event Action PopupClose;

    private float Course;
    private double CurrentBtcValue;
    private double Increaser = 0.01;
    private double IncreaserInitialValue;

    void Start()
    {
        IncreaserInitialValue = Increaser;
        Minus.onClick.AddListener(OnMinusClick);
        Plus.onClick.AddListener(OnPlusClick);
        Accept.onClick.AddListener(OnAcceptClick);
        Close.onClick.AddListener(OnCloseClick);
    }

    public void OpenPopup(float course)
    {
        Info.text = $"1 биткоин = {course:0.00}$";
        Course = course;
        CurrentBtcValue = GameController.Instance.User.UserCryptoBalnce;
        UpdateInfo();
        for (int i = 0; i < GameController.Instance.MiningPrefabs.Count; i++)
        {
            GameController.Instance.MiningPrefabs[i].Pause = true;
        }
    }

    private void OnAcceptClick()
    {
        GameController.Instance.UpdateUserCryptoBalance(-CurrentBtcValue);
        GameController.Instance.UpdateUserMoneyBalance(CurrentBtcValue * Course);
        OpenPopup(Course);
    }

    private void OnCloseClick()
    {
        PopupAnimator.SetTrigger("hide");
        AnimatorCallback.Callback = AnimatorHided;
    }

    private void AnimatorHided()
    {
        for (int i = 0; i < GameController.Instance.MiningPrefabs.Count; i++)
        {
            GameController.Instance.MiningPrefabs[i].Pause = false;
        }
        AnimatorCallback.Callback = null;
        PopupClose?.Invoke();
    }

    private void OnMinusClick()
    {
        if (CurrentBtcValue > Increaser)
        {
            CurrentBtcValue -= Increaser;
        }
        
        UpdateInfo();
    }
    
    private void OnPlusClick()
    {
        if (CurrentBtcValue + Increaser <= GameController.Instance.User.UserCryptoBalnce)
        {
            CurrentBtcValue += Increaser;
        }
        
        UpdateInfo();
    }
    
    public void OnPlusPointerDown()
    {
        StartCoroutine(DelayBeforeHold(false));
    }
    
    public void OnPlusPointerUp()
    {
        Increaser = IncreaserInitialValue;
        StopAllCoroutines();
    }

    public void OnMinusPointerDown()
    {
        StartCoroutine(DelayBeforeHold(true));
    }
    
    public void OnMinusPointerUp()
    {
        Increaser = IncreaserInitialValue;
        StopAllCoroutines();
    }
    
    private IEnumerator DelayBeforeHold(bool isMinus)
    {
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(OnButtonHold(isMinus));
    }

    private IEnumerator OnButtonHold(bool isMinus)
    {
        Increaser += Time.deltaTime;
        if (isMinus)
        {
            OnMinusClick();
        }
        else
        {
            OnPlusClick();
        }

        yield return null;
        StartCoroutine(OnButtonHold(isMinus));
    }

    private void UpdateInfo()
    {
        Dollars.text = $"{CurrentBtcValue * Course:0.00}$";
        Btc.text = $"{CurrentBtcValue:0.00}";
    }
}
