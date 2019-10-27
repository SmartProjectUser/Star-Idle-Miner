using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

public class StockController : MonoBehaviour
{
    public float MinRefreshRate;
    public float MaxRefreshRate;
    public float MinStockValueRate;
    public float MaxStockValueRate;

    public List<StockItemView> StockItemViews;
    public Button FirstButton;
    public Button SecondButton;
    public Button ThirdButton;
    //public List<Button> Buttons;
    public Popup Popup;
    private Coroutine Cour;
    
    // Start is called before the first frame update
    void Start()
    {
        FirstButton.onClick.AddListener(() => {OnButtonClick(0); });
        SecondButton.onClick.AddListener(() => {OnButtonClick(1); });
        ThirdButton.onClick.AddListener(() => {OnButtonClick(2); });

        for (int i = 0; i < StockItemViews.Count; i++)
        {
            Refresh(StockItemViews[i]);
        }
        
        //Refresh();
        Cour = GameController.Instance.StartCoroutine(StockRefreshService());
    }

    private void OnButtonClick(int index)
    {
        Popup.gameObject.SetActive(true);
        Popup.OpenPopup(StockItemViews[index].StockCourse);
        if (Cour != null)
        {
            StopCoroutine(Cour);
        }

        Popup.PopupClose += OnPopupClose;
    }

    private void OnPopupClose()
    {
        Popup.PopupClose -= OnPopupClose;
        Popup.gameObject.SetActive(false);
        Cour = GameController.Instance.StartCoroutine(StockRefreshService());
    }

    private IEnumerator StockRefreshService()
    {
        yield return new WaitForSeconds(Random.Range(MinRefreshRate, MaxRefreshRate));

        Refresh();
        Cour = GameController.Instance.StartCoroutine(StockRefreshService());
    }

    private void Refresh(StockItemView defaultItemView = null)
    {
        StockItemView currentView = StockItemViews[Random.Range(0, StockItemViews.Count)];
        if (defaultItemView != null)
            currentView = defaultItemView;
        Text tempText = currentView.StockText;
        float valueToDisplay = Random.Range(MinStockValueRate, MaxStockValueRate);
        float maxWithoutMin = MaxStockValueRate - MinStockValueRate;
        float valueToDisplayWithoutMin = valueToDisplay - MinStockValueRate;
        tempText.text = $"{valueToDisplay:0.00}";
        currentView.StockCourse = valueToDisplay;
        tempText.color = Color.Lerp(Color.red, Color.green, valueToDisplayWithoutMin / maxWithoutMin);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
