using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

    public int Health = 100;
	
	// Update is called once per frame
	public void TakeDamage(int amount)
    {
        Health -= amount;
        if (Health < 0)
            Health = 0;

	}
}
