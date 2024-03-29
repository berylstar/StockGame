using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum eStockType
{
    RANDOM,         // 沓棋
    CRESCENDO,      // 繊繊 装亜
    BIGUP,          // 怯雌
    BIGDOWN,        // 怯喰
    WAVE,           // 鯵説爽
    STEADY,         // 企舌爽
}

public class GameController : MonoBehaviour
{
    public static readonly WaitForSecondsRealtime delay_1s = new WaitForSecondsRealtime(1f);
    public static readonly WaitForSecondsRealtime delay_025s = new WaitForSecondsRealtime(0.25f);

    public static GameController game_inst = null;

    public GameObject[] stocks;
    public Text textNews;
    public Text textTime;
    public Text textMyMoney;

    private int iHour = 9;
    private int iMin = 0;
    public int myMoney = 2000;
    private bool flag_GameOver = false;
    public float minClick = 0.3f;

    private int stage = 0;  // 恥 14 什砺戚走
    private int bigUpStage = 0;
    private int bigDownStage = 0;

    private eStockType _tnews = eStockType.RANDOM;

    private List<int> indexList = new List<int>() { 0, 1, 2, 3, 4 };
    private List<eStockType> typeList = new List<eStockType>() { eStockType.CRESCENDO, eStockType.BIGUP, eStockType.BIGDOWN, eStockType.WAVE, eStockType.STEADY };

    private void Awake()
    {
        if (game_inst == null)
            game_inst = this;

        DrawLots();

        textTime.text = "09:00";

        StartCoroutine(GameStart());
    }

    private void Update()
    {
        textMyMoney.text = myMoney + " $";

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ButtonTimeSkip();
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            for (int i = 0; i < stocks.Length; i++)
            {
                stocks[i].transform.GetChild(0).gameObject.SetActive(false);
                stocks[i].transform.GetChild(1).gameObject.SetActive(false);
                stocks[i].transform.GetChild(5).gameObject.SetActive(true);
                stocks[i].transform.GetChild(6).gameObject.SetActive(true);
            }
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            for (int i = 0; i < stocks.Length; i++)
            {
                stocks[i].transform.GetChild(0).gameObject.SetActive(true);
                stocks[i].transform.GetChild(1).gameObject.SetActive(true);
                stocks[i].transform.GetChild(5).gameObject.SetActive(false);
                stocks[i].transform.GetChild(6).gameObject.SetActive(false);
            }
        }
    }

    private void DrawLots()
    {
        int[] shakedIndex = new int[5];
        eStockType[] pickedType = new eStockType[5] { eStockType.RANDOM, eStockType.RANDOM , eStockType.RANDOM , eStockType.RANDOM , eStockType.RANDOM };

        for (int i = 0; i < 5; i++)
        {
            int iRand = Random.Range(0, indexList.Count);
            shakedIndex[i] = indexList[iRand];
            indexList.RemoveAt(iRand);
        }

        for (int i = 0; i < 2; i++)
        {
            int iRand = Random.Range(0, typeList.Count);
            pickedType[i] = typeList[iRand];
            typeList.RemoveAt(iRand);
        }

        for (int i = 0; i < 5; i++)
        {
            stocks[shakedIndex[i]].GetComponent<StockScript>().thistype = pickedType[i];
        }

        bigUpStage = Random.Range(7, 14);
        bigDownStage = Random.Range(7, 14);

        _tnews = pickedType[0];
    }

    public int PriceChange(StockScript stock)
    {
        if (stock.thistype == eStockType.RANDOM)
            return Stock_Random();

        else if (stock.thistype == eStockType.CRESCENDO)
            return Stock_Crescendo();

        else if (stock.thistype == eStockType.BIGUP)
            return Stock_Bigup();

        else if (stock.thistype == eStockType.BIGDOWN)
            return Stock_Bigdown();

        else if (stock.thistype == eStockType.WAVE)
            return Stock_Wave();

        else if (stock.thistype == eStockType.STEADY)
            return Stock_Steady();

        return 0;
    }

    private int Stock_Random()
    {
        return Random.Range(-5, 6) * 100;
    }

    private int Stock_Crescendo()
    {
        return Random.Range(0, 4) * 100;
    }

    private int Stock_Bigup()
    {
        if (stage == bigUpStage)
            return Random.Range(8, 12) * 100;
        else
            return Stock_Random();
    }

    private int Stock_Bigdown()
    {
        if (stage == bigDownStage)
            return-Random.Range(10, 15) * -100;
        else
            return Stock_Random();
    }

    private int Stock_Wave()
    {
        return Random.Range(-2, 2) * 300;
    }

    private int Stock_Steady()
    {
        return Random.Range(-3, 2) * 100;
    }

    private void CheckBankrupt()
    {
        if (myMoney < 1)
        {
            for (int i = 0; i < stocks.Length; i++)
            {
                if (stocks[i].GetComponent<StockScript>().stockHolding > 0)
                    return;
            }

            flag_GameOver = true;
        }
        else
        {
            for (int i = 0; i < stocks.Length; i++)
            {
                if (!stocks[i].GetComponent<StockScript>().flag_Delisting)
                    return;
            }

            flag_GameOver = true;
        }
    }

    private void RandomNews()
    {
        string message = "";
        int newsIndex = Random.Range(0, 5);
        int stockIndex = Random.Range(0, 5);

        while (stocks[stockIndex].GetComponent<StockScript>().flag_Delisting)
        {
            stockIndex = Random.Range(0, 5);
        }

        if (stage == 1)
        {
            message = string.Format("[Breaking News]\n\nOne of stocks turned out\nto be \'{0}\'...shocking", _tnews);
            textNews.text = message;
            return;
        }

        if (newsIndex == 0)
        {
            int costChange = stocks[stockIndex].GetComponent<StockScript>().costChange;

            if (costChange > 0)
                message = string.Format("[働臓爽] けけけけ,\n\n硲伐拭 蝕企厭 燈切切 侯形\n穿庚亜級 爽亜 +{0}$ 森著", costChange);
            else if (costChange == 0)
                message = string.Format("[働臓爽] しししし,\n\n叔旋 降妊稽 爽亜 照舛鞠蟹?\n穿庚亜級 爽亜 疑衣 森著", costChange);
            else
                message = string.Format("[働臓爽] ≠≠≠≠,\n\n奄企戚馬 叔旋拭 馬喰室...\n穿庚亜級 爽亜 {0}$ 森著", costChange);
        }
        else if (newsIndex == 1 || newsIndex == 4)
        {
            int plus = 0;
            int minus = 0;

            for (int i = 0; i < 5; i++)
            {
                if (stocks[i].GetComponent<StockScript>().costChange > 0)
                    plus += 1;
                else if (stocks[i].GetComponent<StockScript>().costChange < 0 && !stocks[i].GetComponent<StockScript>().flag_Delisting)
                    minus += 1;
            }

            if (plus <= minus)
                message = string.Format("[井薦]\n杉 什闘軒闘 煽確,\n\"井奄 徴端拭 濁郊寓災 依\"\n穿庚亜級 {0}鯵爽 馬喰 森著", minus);
            else
                message = string.Format("[井薦]\n舛採 爽縦 舛奪 降妊,\n爽縦 獣舌 醗奄 災嬢隔蟹?\n穿庚亜級 {0}鯵爽 雌渋 森著", plus);
        }
        else if (newsIndex == 2)
        {
            int prevChange = stocks[stockIndex].GetComponent<StockScript>().prevChange;
            int costChange = stocks[stockIndex].GetComponent<StockScript>().costChange;
            
            if (prevChange > 0 && costChange > 0)
                message = string.Format("[働臓爽] 』』』』,\n\n室域稽 蟹焼亜澗 'K-奄穣'\n爽亜 暁 陥獣 雌渋 森著");
            else if (prevChange < 0 && costChange < 0)
                message = string.Format("[働臓爽] ⊃⊃⊃⊃,\n\n井奄 徴端 駅差 叔鳶...\n爽亜 暁 陥獣 馬喰 森著");
            else if (prevChange > 0 && costChange < 0)
                message = string.Format("[働臓爽] ∋∋∋∋,\n\n逢拙什君錘 井奄 徴端拭 爽茶\n雌渋梅揮 爽亜 仙託 馬喰");

            if (costChange > 0)
                message = string.Format("[働臓爽] けけけけ,\n\n硲伐拭 蝕企厭 燈切切 侯形\n穿庚亜級 爽亜 +{0}$ 森著", costChange);
            else if (costChange == 0)
                message = string.Format("[働臓爽] しししし,\n\n叔旋 降妊稽 爽亜 照舛鞠蟹?\n穿庚亜級 爽亜 疑衣 森著", costChange);
            else
                message = string.Format("[働臓爽] ≠≠≠≠,\n\n奄企戚馬 叔旋拭 馬喰室...\n穿庚亜級 爽亜 {0}$ 森著", costChange);
        }
        else if (newsIndex == 3)
        {
            bool flagg = false;
            for (int i = 0; i < 5; i++)
            {
                if (stocks[i].GetComponent<StockScript>().costNext <= 0 && !stocks[i].GetComponent<StockScript>().flag_Delisting)
                {
                    message = string.Format("[働臓爽] −−−−,\n\n姥繕繕舛 戚嬢 社勺猿走...\n衣厩 督至 箭託 高焼");
                    flagg = true;
                }
            }
            if (!flagg)
            {
                // message = string.Format("[働臓爽] けけけけ,\n\nK-奄穣税 毘 左食爽蟹...\n督至 是奄拭辞 社持");

                int costChange = stocks[stockIndex].GetComponent<StockScript>().costChange;

                if (costChange > 0)
                    message = string.Format("[働臓爽] けけけけ,\n\n硲伐拭 蝕企厭 燈切切 侯形\n穿庚亜級 爽亜 +{0}$ 森著", costChange);
                else if (costChange == 0)
                    message = string.Format("[働臓爽] しししし,\n\n叔旋 降妊稽 爽亜 照舛鞠蟹?\n穿庚亜級 爽亜 疑衣 森著", costChange);
                else
                    message = string.Format("[働臓爽] ≠≠≠≠,\n\n奄企戚馬 叔旋拭 馬喰室...\n穿庚亜級 爽亜 {0}$ 森著", costChange);
            }
                
        }

        textNews.text = message;
    }

    public void ButtonTimeSkip()
    {
        if (flag_GameOver)
            return;

        if (0 < iMin && iMin < 20)
            iMin = 20;
        else if (30 < iMin && iMin < 50)
            iMin = 50;
        textTime.text = string.Format("{0:D2}:{1:D2}", iHour, iMin);
    }

    IEnumerator GameStart()
    {
        while ((iHour < 15 || iMin < 30) && !flag_GameOver)
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
            {
                StartCoroutine(Blink());
                SoundManager.inst.PlaySound("BeforeTime");
            }
                
            if (iMin == 0 || iMin == 30)
            {
                for (int i = 0; i < stocks.Length; i++)
                {
                    stocks[i].GetComponent<StockScript>().CalcStock();
                }

                stage += 1;

                CheckBankrupt();

                if (!flag_GameOver)
                    RandomNews();                

                SoundManager.inst.PlaySound("OnTime");
            }
        }

        textTime.text = "OVER";

        for (int i = 0; i < stocks.Length; i++)
        {
            stocks[i].GetComponent<StockScript>().ButtonAllSelling();
        }
    }

    IEnumerator Blink()
    {
        textTime.color = new Color(255, 255, 0, 0);

        yield return delay_025s;

        textTime.color = new Color(255, 255, 0, 1);
    }
}
