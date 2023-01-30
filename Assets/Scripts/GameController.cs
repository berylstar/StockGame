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
    private int costChange = 0;


    public void Stock_Random()
    {
        costChange = Random.Range(-5, 6) * 100;
    }

    public void Stock_Crescendo()
    {
        costChange = Random.Range(0, 5) * 100;
    }

    public void Stock_Bigup()
    {

    }

    public void Stock_Bigdown()
    {

    }

    public void Stock_Wave()
    {
        costChange = Random.Range(-3, 3) * 200;
    }

    public void Stock_Steady()
    {
        costChange = Random.Range(-3, 3) * 100;
    }
}
