using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    public Text textTime;

    public static WaitForSecondsRealtime delay_1s = new WaitForSecondsRealtime(1f);
    public static WaitForSecondsRealtime delay_1m = new WaitForSecondsRealtime(60f);

    private int iHour = 0;
    private int iMin = 0;

    private void Awake()
    {
        textTime.text = "00:00";

        StartCoroutine(StartTime());
    }

    IEnumerator StartTime()
    {
        while (iHour < 24)
        {
            textTime.text = string.Format("{0:D2}:{1:D2}", iHour, iMin);

            yield return delay_1s;

            FlowTime();
        }

        textTime.text = "24:00";
        GameController.flag_GameOver = true;
    }

    private void FlowTime()
    {
        iMin += 10;

        if (iMin >= 60)
        {
            iMin = 0;
            iHour += 1;
        }
    }
}
