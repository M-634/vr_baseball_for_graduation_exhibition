using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Katsumata
{
    public class ScoreManager
    {
        int m_score = 0;

        public int GetScore
        {
            get
            {
                return m_score;
            }
        }

        public void AddScore(int addScore)
        {
            m_score += addScore;
        }
    }
}