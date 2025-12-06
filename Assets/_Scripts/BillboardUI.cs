using UnityEngine;

public class BillboardUI : MonoBehaviour {
    [SerializeField] private Camera _camera;

    void LateUpdate() {
        if (_camera == null) return;

        // Make UI face the camera
        transform.forward = _camera.transform.forward;
    }
}