using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ScoreManager : MonoBehaviour
{
    [SerializeField] long m_baseIncome = 1000000;
    Score m_score;

    public int GetScore
    {
        get
        {
            return m_score.m_scorePoint;
        }
    }

    /// <summary>
    /// 年収のゲッター
    /// </summary>
    public long GetIncome
    {
        get
        {
            return m_score.m_annualIncome;
        }
    }

    private void Start()
    {
        m_score = new Score(m_baseIncome, m_baseIncome);
    }

    public void AddScore()
    {
        m_score.m_scorePoint++;
        m_score.CalAnnualIncome();
    }
}


public class Score
{
    //得点
    public int m_scorePoint = 0;
    //年収
    public long m_annualIncome;
    long m_baseIncome;
    int m_incomePow = 4;

    public Score(long income, long baseIncome)
    {
        m_annualIncome = income;
        m_baseIncome = baseIncome;
    }

    /// <summary>
    /// 得点を追加する関数
    /// </summary>
    /// <param name="addScore"></param>
    public void AddScore(int addScore)
    {
        m_scorePoint += addScore;
    }

    /// <summary>
    /// 年収の計算関数
    /// </summary>
    /// <returns></returns>
    public void CalAnnualIncome()
    {
        if (m_scorePoint == 0)
        {
            m_annualIncome = m_baseIncome;
        }
        else if (m_scorePoint == 1)
        {
            m_annualIncome = m_baseIncome + m_baseIncome * (long)Mathf.Pow(2, m_incomePow) / 2;//２点のときの年収の半額
        }
        else
        {
            m_annualIncome = m_baseIncome + m_baseIncome * (long)Mathf.Pow(m_scorePoint, m_incomePow);
        }
    }
}
