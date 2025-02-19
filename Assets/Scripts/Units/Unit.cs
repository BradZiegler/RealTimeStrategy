using System;
using Mirror;
using UnityEngine;
using UnityEngine.Events;

public class Unit : NetworkBehaviour {
    [SerializeField] private int resourceCost = 10;
    [SerializeField] private Health health = null;
    [SerializeField] private UnityEvent onSelected = null;
    [SerializeField] private UnityEvent onDeselected = null;
    [SerializeField] private UnitMovement unitMovement = null;
    [SerializeField] private Targeter targeter = null;

    public static event Action<Unit> ServerOnUnitSpawned;
    public static event Action<Unit> ServerOnUnitDespawned;
    
    public static event Action<Unit> AuthorityOnUnitSpawned;
    public static event Action<Unit> AuthorityOnUnitDespawned;

    public int GetResourceCost() {
        return resourceCost;
    }

    public UnitMovement GetUnitMovement() {
        return unitMovement;
    }

    public Targeter GetTargeter() {
        return targeter;
    }

    #region Server

    public override void OnStartServer() {
        ServerOnUnitSpawned?.Invoke(this);
        health.ServerOnDie += ServerHandleDie;
    }

    public override void OnStopServer() {
        ServerOnUnitDespawned?.Invoke(this);
        health.ServerOnDie -= ServerHandleDie;
    }

    [Server]
    private void ServerHandleDie() {
        NetworkServer.Destroy(gameObject);
    }

    #endregion

    #region Client

    public override void OnStartAuthority() {
        AuthorityOnUnitSpawned?.Invoke(this);
    }

    public override void OnStopClient() {
        if (!isOwned) { return; }

        AuthorityOnUnitDespawned?.Invoke(this);
    }

    [Client]
    public void Select() {
        if (!isOwned) { return; }

        onSelected?.Invoke();
    }

    [Client]
    public void Deselect() {
        if (!isOwned) { return; }

        onDeselected?.Invoke();
    }

    #endregion
}
