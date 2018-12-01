using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerDamage : MonoBehaviour {

    private bool _isCausingDamage = false;

    public float DamageRepeatRate = 0.1f;
    public int DamageAmount = 1;
    public bool Repeating = true;

    private void OnTriggerEnter(Collider other)
    {
        _isCausingDamage = true;
        PlayerHealth player = other.gameObject.GetComponent<PlayerHealth>();
        if(player != null)
        {
            if (Repeating)
            {
                StartCoroutine(TakeDamage(player, DamageRepeatRate));
            }
            else
            {
                player.TakeDamage(DamageAmount);
            }
        }
    }

    IEnumerator TakeDamage(PlayerHealth player, float repeatRate)
    {
        while (_isCausingDamage)
        {
            player.TakeDamage(DamageAmount);
            TakeDamage(player, repeatRate);
            yield return new WaitForSeconds(repeatRate);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        PlayerHealth player = other.gameObject.GetComponent<PlayerHealth>();
        if(player != null)
        {
            _isCausingDamage = false;
        }
    }

}
