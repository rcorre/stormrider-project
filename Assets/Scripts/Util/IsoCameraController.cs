using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class IsoCameraController : MonoBehaviour {
    public float ScrollSpeed = 40f;
    public float ZoomSpeed = 40f;
    public float RotateSpeed = 100f;
    public float AutoScrollFactor = 1f;
    public float AutoZoomFactor = 2f;
    public float MouseDragSpeed = 0.25f;
    public float AutoFocusDistance = 80f;

    private Vector3 _dragStartMousePos;
    private Vector3 _focalPoint;
    private TileMapMesh _terrainMesh;
    private Vector3 _previousPosition;
    private Quaternion _previousRotation;
    private bool _autoMode;
    private Vector3 _autoTarget;

    // Use this for initialization
    void Start() {
        _terrainMesh = FindObjectOfType<TileMapMesh>();
    }

    // Update is called once per frame
    void Update() {
        if (_autoMode) {
            if (Input.anyKeyDown) {
                _autoMode = false;
            }
            else {
		var disp = (_autoTarget - transform.position);
                var dist = ScrollSpeed * AutoScrollFactor * Time.deltaTime;
                var move = Vector3.ClampMagnitude(disp.normalized * dist, disp.magnitude);
                transform.position += move;
            }
        }
        // keyboard movement
        if (Input.GetKey(KeyCode.W)) {
            var vec = transform.forward;
            vec.y = 0;
            transform.position += (vec * Time.deltaTime * ScrollSpeed);
        }
        else if (Input.GetKey(KeyCode.S)) {
            var vec = -transform.forward;
            vec.y = 0;
            transform.position += (vec * Time.deltaTime * ScrollSpeed);
        }

        if (Input.GetKey(KeyCode.D)) {
            transform.Translate(Vector3.right * Time.deltaTime * ScrollSpeed);
        }
        else if (Input.GetKey(KeyCode.A)) {
            transform.Translate(Vector3.left * Time.deltaTime * ScrollSpeed);
        }

        // mouse scroll zoom
        var scroll = Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * ZoomSpeed;
        transform.Translate(Vector3.forward * scroll * ZoomSpeed * Time.deltaTime);

        // rotation
        if (Input.GetKey(KeyCode.Q)) {
            transform.RotateAround(_focalPoint, Vector3.up, RotateSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.E)) {
            transform.RotateAround(_focalPoint, Vector3.up, -RotateSpeed * Time.deltaTime);
        }

        // right click rotation
        if (Input.GetMouseButtonDown(1)) {
            _dragStartMousePos = Input.mousePosition;
        }
        if (Input.GetMouseButton(1)) {
            var mouseMovement = Input.mousePosition - _dragStartMousePos;
            _dragStartMousePos = Input.mousePosition;
            // mouse rotation about z
            transform.RotateAround(_focalPoint, Vector3.up, RotateSpeed * Time.deltaTime * mouseMovement.x * MouseDragSpeed);
        }

        var camRay = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hitInfo;
        if (_terrainMesh.collider.Raycast(camRay, out hitInfo, Mathf.Infinity)) {
            _previousPosition = transform.position;
            _previousRotation = transform.rotation;
            _focalPoint = hitInfo.point;
        }
        else {
            transform.position = _previousPosition;
            transform.rotation = _previousRotation;
        }
    }

    public void FocusOn(Vector3 pos) {
        _autoMode = true;
        _autoTarget = pos - transform.forward * AutoFocusDistance;
    }
}