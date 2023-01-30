using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    public static readonly WaitForSecondsRealtime delay_1s = new WaitForSecondsRealtime(1f);
    public static readonly WaitForSecondsRealtime delay_10s = new WaitForSecondsRealtime(10f);
    public static readonly WaitForSecondsRealtime delay_1m = new WaitForSecondsRealtime(60f);

    public GameObject[] stocks;
    public Text textTime;
    public Text textAccount;

    private int iHour = 9;
    private int iMin = 0;
    public static int myMoney = 2000;
    public static int stage = 0;

    private void Awake()
    {
        textTime.text = "09:00";

        StartCoroutine(GameStart());
    }

    private void Update()
    {
        textAccount.text = myMoney + " $";
    }

    IEnumerator GameStart()
    {
        while (iHour < 15 || iMin < 30)
        {
            textTime.text = string.Format("{0:D2}:{1:D2}", iHour, iMin);

            yield return delay_1s;

            iMin += 1;

            if (iMin >= 60)
            {
                iMin = 0;
                iHour += 1;
            }

            if ((20 < iMin && iMin < 30) || (50 < iMin))
                SoundManager.inst.PlaySound("BeforeTime");

            if (iMin == 0 || iMin == 30)
            {
                for (int i = 0; i < stocks.Length; i++)
                {
                    stocks[i].GetComponent<StockScript>().CalcStock();
                }

                stage += 1;

                SoundManager.inst.PlaySound("OnTime");
            }
        }

        textTime.text = "15:30";
    }
}
