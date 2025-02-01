using Mirror;
using UnityEngine;

public class UnitProjectile : NetworkBehaviour {
    [SerializeField] private Rigidbody rb = null;
    [SerializeField] private float launchForce = 10f;
    [SerializeField] private float destroyAfterSeconds = 5f;

    void Start() {
        rb.linearVelocity = transform.forward * launchForce;
    }

    public override void OnStartServer() {
        Invoke(nameof(DestroySelf), destroyAfterSeconds);
    }

    [Server]
    private void DestroySelf() {
        NetworkServer.Destroy(gameObject);
    }
}
