using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum eStockType
{
    RANDOM,         // 랜덤
    CRESCENDO,      // 점점 증가
    BIGUP,          // 떡상
    BIGDOWN,        // 떡락
    WAVE,           // 개잡주
    STEADY,         // 대장주
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

    private int stage = 0;  // 총 14 스테이지
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

        if (Input.GetKeyDown(KeyCode.Space) && !flag_GameOver)
        {
            if (0 < iMin && iMin < 20)
                iMin = 20;
            else if (30 < iMin && iMin < 50)
                iMin = 50;
            textTime.text = string.Format("{0:D2}:{1:D2}", iHour, iMin);
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
            print((shakedIndex[i], pickedType[i]));
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

    // 힌트 종류
    // 1. 이전 라운드 ??한 종목 중 하나가 ??합니다. (상승/하락/유지)
    // 2. ??? + 200$ (종목)
    // 3. ??개의 주가가 ??합니다. (갯수) (상승/하락/유지)   -> 하락이 많으면 전체적인 경제 침체입니다 이런식으로
    // 4. 전문가의 의견 -> 주식 전문가 ㅁㅁ씨가 ?? 종목을 매도했다는 소식입니다. + 신뢰도 ??%
    // 5. 초반에 특수 주식 타입에 대한 정보 한 개 주기

    // 상장폐지 힌트

    private void RandomNews()
    {
        string message = "";
        int newsIndex = Random.Range(0, 5);
        int stockIndex = Random.Range(0, 5);

        while (stocks[stockIndex].GetComponent<StockScript>().flag_Delisting)
        {
            stockIndex = Random.Range(0, 5);
        }

        if (newsIndex == 0)
        {
            int costChange = stocks[stockIndex].GetComponent<StockScript>().costChange;

            if (costChange > 0)
                message = string.Format("[특징주] ㅁㅁㅁㅁ,\n\n시장 호황에 상승 전망...\n전문가들 주가 +{0}$ 예측", costChange);
            else if (costChange == 0)
                message = string.Format("[특징주] ㅇㅇㅇㅇ,\n\n실적 발표로 주가 정상화...\n전문가들 주가 동결 예측", costChange);
            else
                message = string.Format("[특징주] ㅂㅂㅂㅂ,\n\n기대이하 성장에 하락세...\n전문가들 주가 {0}$ 예측", costChange);
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

            if (plus > minus)
                message = string.Format("[경제]\n정부의 주식 정책 발표에\n시장 활성화 전망...\n전문가들 {0}개주 상승 예측", plus);
            else
                message = string.Format("[경제]\n정부의 주식 정책 발표에\n시장 침체 전망...\n전문가들 {0}개주 하락 예측", minus);
        }
        else if (newsIndex == 2)
        {
            int prevChange = stocks[stockIndex].GetComponent<StockScript>().prevChange;
            int costChange = stocks[stockIndex].GetComponent<StockScript>().costChange;
            
            if (prevChange > 0 && costChange > 0)
                message = string.Format("[특징주] ㅁㅁㅁㅁ,\n\n지난 분기 대비 실적 상승...\n주가 연이어 상승 예측");
            else if (prevChange < 0 && costChange < 0)
                message = string.Format("[특징주] ㅁㅁㅁㅁ,\n\n경기 침체 극복 실패...\n주가 연이어 하락 예측");
            else if (prevChange > 0 && costChange < 0)
                message = string.Format("[특징주] ㅁㅁㅁㅁ,\n\n갑작스러운 경기 침체에 주춤\n상승했던 주가 재차 하락");

            else if (costChange > 0)
                message = string.Format("[특징주] ㅁㅁㅁㅁ,\n\n시장 호황에 상승 전망...\n전문가들 주가 +{0}$ 예측", costChange);
            else if (costChange == 0)
                message = string.Format("[특징주] ㅇㅇㅇㅇ,\n\n실적 발표로 주가 정상화...\n전문가들 주가 동결 예측", costChange);
            else
                message = string.Format("[특징주] ㅂㅂㅂㅂ,\n\n기대이하 성장에 하락세...\n전문가들 주가 {0}$ 예측", costChange);
        }
        else if (newsIndex == 3)
        {
            bool flagg = false;
            for (int i = 0; i < 5; i++)
            {
                if (stocks[i].GetComponent<StockScript>().costNext <= 0 && !stocks[i].GetComponent<StockScript>().flag_Delisting)
                {
                    message = string.Format("[특징주] ㅁㅁㅁㅁ,\n\n구조조정 이어 소송까지...\n결국 파산 신청");
                    flagg = true;
                }
            }
            if (!flagg)
            {
                // message = string.Format("[특징주] ㅁㅁㅁㅁ,\n\nK-기업의 힘 보여주나...\n파산 위기에서 소생");

                int costChange = stocks[stockIndex].GetComponent<StockScript>().costChange;

                if (costChange > 0)
                    message = string.Format("[특징주] ㅁㅁㅁㅁ,\n\n시장 호황에 상승 전망...\n전문가들 주가 +{0}$ 예측", costChange);
                else if (costChange == 0)
                    message = string.Format("[특징주] ㅇㅇㅇㅇ,\n\n실적 발표로 주가 정상화...\n전문가들 주가 동결 예측", costChange);
                else
                    message = string.Format("[특징주] ㅂㅂㅂㅂ,\n\n기대이하 성장에 하락세...\n전문가들 주가 {0}$ 예측", costChange);
            }
                
        }
        //else if (newsIndex == 4)
        //{
        //    message = string.Format("이번 종목 중 하나는\n{0} 타입의 주식입니다.", _tnews);
        //}

        textNews.text = message;
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
    }

    IEnumerator Blink()
    {
        textTime.color = new Color(255, 255, 0, 0);

        yield return delay_025s;

        textTime.color = new Color(255, 255, 0, 1);
    }
}
