using UnityEngine;

public class FaceCamera : MonoBehaviour {
    private Transform mainCameraTransform;

    private void Start() {
        mainCameraTransform = Camera.main.transform;
    }

    void LateUpdate() {
        transform.LookAt(
            transform.position + mainCameraTransform.rotation * Vector3.forward, 
            mainCameraTransform.rotation * Vector3.up);
    }
}
