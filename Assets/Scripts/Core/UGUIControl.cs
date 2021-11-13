using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// WorldSpaceè„ÇÃUIêßå‰Ç∑ÇÈÉNÉâÉX
/// </summary>
public class UGUIControl : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI displayJudgeText = default;

    // Start is called before the first frame update
    void Start()
    {
        displayJudgeText.gameObject.SetActive(false);

        BaseBallLogic.Instance.OnReceiveMessage += (judgeType) =>
        {
            if (judgeType == JudgeType.None) return;

            if (displayJudgeText)
            {
                displayJudgeText.gameObject.SetActive(true);
                displayJudgeText.text = judgeType.ToString();
                StartCoroutine(DelayActive(displayJudgeText.gameObject, false, 2f));
            }  
        };
    }

    IEnumerator DelayActive(GameObject gameObject,bool value,float delayTime = 0f)
    {
        while (delayTime > 0f)
        {
            delayTime -= Time.deltaTime;
            yield return null;
        }
        gameObject.SetActive(value);
    }
}
