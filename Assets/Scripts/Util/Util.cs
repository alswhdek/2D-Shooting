using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util 
{
    public static int GetItemDropRate(int[] table)
    {
        if (table == null || table.Length == 0) return -1;

        int sum = 0;
        int num = 0;
        
        for(int i=0; i<table.Length; i++)
        {
            sum += table[i]; //모든 아이템 확률을 더한다.
        }
        num = Random.Range(1, sum + 1);// 1 ~ 아이템 전체 더한 확률로 ItemDrop률은 구한다.

        sum = 0;

        for(int i=0; i<table.Length; i++)
        {
            if(num > sum && num <= sum+table[i])
            {
                return i;//조건에 만족하면 반환
            }
            sum += table[i];//만족하지 않는다면 누적
        }
        return -1; //테이블안에 들어오지 않았으면
    }
}
