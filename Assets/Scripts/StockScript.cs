using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StockScript : MonoBehaviour
{
    public int costNow;
    private int costNext;

    public Text textCost;
    public Text textChange;     // (- ¡ã ¡å)

    private void Start()
    {
        StartCoroutine(NextInNow());
    }

    private void CostChange()
    {
        int iRand = Random.Range(-5, 6);
        costNext = costNow + iRand * 100;
    }

    IEnumerator NextInNow()
    {
        while(true)
        {
            CostChange();

            yield return TimeController.delay_1s;

            costNow = costNext;
            textCost.text = costNow + "$";
        }
    }
}
