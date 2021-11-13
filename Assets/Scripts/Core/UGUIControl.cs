using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// WorldSpace���UI���䂷��N���X
/// </summary>
public class UGUIControl : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        BaseBallLogicEventManager.Instance.OnReceiveMessage += (judgeType) =>
        {
            if (text)
            {
                text.gameObject.SetActive(true);
                text.text = judgeType.ToString();
            }  
        };
    }
}
