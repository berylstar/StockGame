using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eStockType
{
    RANDOM,         // ����
    CRESCENDO,      // ���� ����
    BIGUP,          // ����
    BIGDOWN,        // ����
    WAVE,           // ������
    STEADY,         // ������
}

// 3���� ����
// 2���� ������ ���ϴ�� �ൿ
// ���ػ����, ����, ����, ������, ������

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
