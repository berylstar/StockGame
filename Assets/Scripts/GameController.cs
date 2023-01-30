using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eStockType
{
    RANDOM,         // 랜덤
    CRESCENDO,      // 점점 증가
    BIGUP,          // 떡상
    BIGDOWN,        // 떡락
    WAVE,           // 개잡주
    STEADY,         // 대장주
}

// 3개는 랜덤
// 2개는 정해진 패턴대로 행동
// 꾸준상승주, 떡상, 떡락, 개잡주, 대장주

public class GameController : MonoBehaviour
{
    public static GameController game_inst = null;

    public GameObject[] stocks;

    private List<int> indexList = new List<int>() { 0, 1, 2, 3, 4 };
    private List<eStockType> typeList = new List<eStockType>() { eStockType.CRESCENDO, eStockType.BIGUP, eStockType.BIGDOWN, eStockType.WAVE, eStockType.STEADY };

    private void Awake()
    {
        if (game_inst == null)
            game_inst = this;

        DrawLots();
    }

    private void DrawLots()
    {
        int[] shakedIndex = new int[5];
        eStockType[] pickedType = new eStockType[5] { eStockType.RANDOM, eStockType.RANDOM , eStockType.RANDOM , eStockType.RANDOM , eStockType.RANDOM };

        for (int i = 0; i< 5; i++)
        {
            int iRand = Random.Range(0, indexList.Count);
            shakedIndex[i] = indexList[iRand];
            indexList.RemoveAt(iRand);
        }

        // 패턴 두개 뽑기
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
        return Random.Range(0, 5) * 100;
    }

    private int Stock_Bigup()
    {
        return 0;
    }

    private int Stock_Bigdown()
    {
        return 0;
    }

    private int Stock_Wave()
    {
        return Random.Range(-4, 4) * 200;
    }

    private int Stock_Steady()
    {
        return Random.Range(-3, 2) * 100;
    }
}
