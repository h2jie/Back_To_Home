using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
public class PropsSetAction : MonoBehaviour 
{

    /// <summary>
    /// Evento que tiene ejecutado después de la colisión
    /// </summary>
    [Header("Evento que tiene ejecutado después de la colisión")]
    public Button.ButtonClickedEvent TriggerEvent;

    [Header("¿Puede el personaje dispararse al caer?")]
    public bool isFallTrigger = false;

    public string AudioName;

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

        SoundManager.Instance.PlaySound(AudioName);

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
