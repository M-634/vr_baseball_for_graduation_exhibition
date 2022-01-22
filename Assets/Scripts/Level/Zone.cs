using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ファールゾーン,ホームランゾーンに入ったらボールを消す
/// </summary>
public class Zone : MonoBehaviour, IBallHitObjet
{
    //[SerializeField] ZoneType zone;

    private void Awake()
    {
        if (TryGetComponent(out MeshRenderer meshRenderer))
        {
            meshRenderer.enabled = false;
        }
    }

    public void OnHit(Rigidbody rb, RaycastHit hitObjectInfo, float ballSpeed)
    {
        //switch (zone)
        //{
        //    case ZoneType.FoulZone:
        //        GameFlowManager.Instance.Foul = true;
        //        break;
        //    case ZoneType.OutZone:
        //        GameFlowManager.Instance.Out = true;
        //        break;
        //    case ZoneType.CatcherZone:
        //        GameFlowManager.Instance.Strike = true;
        //        break;
        //    case ZoneType.HomeRunZone:      
        //        GameFlowManager.Instance.UpdateHitType(HitZoneType.HomeRun);
        //        break;
        //}
        //rb.gameObject.SetActive(false);
    }
}
