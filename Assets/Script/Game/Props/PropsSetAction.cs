using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
public class PropsSetAction : MonoBehaviour 
{

    /// <summary>
    /// Evento que necesita para ejecutar después de la colisión
    /// </summary>
    [Header("Evento que necesita para ejecutar después de la colisión")]
    public Button.ButtonClickedEvent TriggerEvent;

    [Header("Si el personaje está cayendo no puede disparar con los objetos")]
    public bool isFallTrigger = false;
    public void OnTriggerEnter2D(Collider2D coll)
    {

        if (!isFallTrigger)
        {
            HeroCtrl ctrl = coll.GetComponent<HeroCtrl>();
            if (ctrl != null && ctrl.playerRigidbody2D.velocity.y < 0)
            {
                return;
            }
        }
        TriggerEvent.Invoke();

        if (eventId != EventDef.LevelEvent.None)
        {
            SendEvent();
        }
    }

    public EventDef.LevelEvent eventId = EventDef.LevelEvent.None;
    private void SendEvent()
    {
        if (AppMgr.Instance)
        AppMgr.Instance.SendEvent((int)eventId,this.gameObject);
    }
	
}
