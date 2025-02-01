using System;
using Mirror;
using UnityEngine;

public class Health : NetworkBehaviour {
    [SerializeField] private int maxHealth = 100;

    public event Action ServerOnDie;

    [SyncVar]
    private int currentHealth;

    #region Server

    public override void OnStartServer() {
        currentHealth = maxHealth;
    }

    [Server]
    public void DealDamage(int damageAmount) {
        if (currentHealth == 0) { return; }

        currentHealth = Mathf.Max(currentHealth - damageAmount, 0);

        if (currentHealth != 0) { return; }

        ServerOnDie?.Invoke();
    }

    #endregion

    #region Client

    #endregion
}
