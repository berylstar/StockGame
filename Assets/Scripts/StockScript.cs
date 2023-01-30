using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StockScript : MonoBehaviour
{
    public Text textCost;
    public Text textChange;
    public Text textStockHolding;

    public int stockHolding = 0;
    public int costNow;
    private int costNext;
    private int costChange;
    private bool flag_Delisting = false;

    private void Start()
    {
        costNext = costNow;
        costChange = 0;
        CostChange();
    }

    public void CostChange()
    {
        NextToNowCost();
        textCost.text = costNow + "$";
        AppleTextChange();

        PriceChange();
        costNext = costNow + costChange;
    }

    private void PriceChange()
    {
        costChange = Random.Range(-5, 6) * 100;
    }

    private void AppleTextChange()
    {
        if (costChange > 0)
        {
            textChange.text = "(" + costChange + " ¡ã)";
        }
        else if (costChange < 0)
        {
            textChange.text = "(" + costChange + "¡å)";
        }
        else
        {
            textChange.text = "(------)";
        }

        if (flag_Delisting)
        {
            textChange.text = "(»óÀåÆóÁö)";
        }
    }

    private void NextToNowCost()
    {
        costNow = costNext;

        if (costNow <= 0)
        {
            flag_Delisting = true;
            costNow = 0;
            stockHolding = 0;
            textStockHolding.text = stockHolding + "";
        }
        else if (costNow > 0)
            flag_Delisting = false;
    }

    public void ButtonBuying()
    {
        if (TimeController.myMoney >= costNow && !flag_Delisting)
        {
            TimeController.myMoney -= costNow;
            stockHolding += 1;
            textStockHolding.text = stockHolding + "";
            SoundManager.inst.PlaySound("Buy");
        }
        else
            SoundManager.inst.PlaySound("Error");
    }

    public void ButtonSelling()
    {
        if (stockHolding > 0)
        {
            TimeController.myMoney += costNow;
            stockHolding -= 1;
            textStockHolding.text = stockHolding + "";
            SoundManager.inst.PlaySound("Sell");

        }
        else
            SoundManager.inst.PlaySound("Error");
    }
}