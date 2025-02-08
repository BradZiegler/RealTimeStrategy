using System;
using Mirror;
using UnityEngine;

public class Health : NetworkBehaviour {
    [SerializeField] private int maxHealth = 100;

    public event Action ServerOnDie;
    public event Action<int, int> ClientOnHealthUpdated;

    [SyncVar(hook = nameof(HandleHealthUpdated))]
    private int currentHealth;

    #region Server

    public override void OnStartServer() {
        UnitBase.ServerOnPlayerDie += ServerHandlePlayerDie;

        currentHealth = maxHealth;
    }

    public override void OnStopServer() {
        UnitBase.ServerOnPlayerDie -= ServerHandlePlayerDie;
    }

    [Server]
    private void ServerHandlePlayerDie(int connectionId) {
        if (connectionToClient.connectionId != connectionId) { return; }

        DealDamage(currentHealth);
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

    private void HandleHealthUpdated(int oldHealth, int newHealth) {
        ClientOnHealthUpdated?.Invoke(newHealth, maxHealth);
    }

    #endregion
}
