using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public Transform target;
    private Vector3 offsetPosition;
	private Camera camera_;
	private float minSize;
	private float maxSize;
	private bool isZoom_ = false;
    
	void Start () {
		camera_ = this.GetComponent<Camera> ();
        offsetPosition = this.transform.position - target.position;
		minSize = camera_.orthographicSize;
		maxSize = 20.0f;
	}
	
	void Update () {
		if (target != null) {
			this.transform.position = target.transform.position + offsetPosition;
		}
		if (Input.GetKey (KeyCode.LeftShift) || Input.GetKey(KeyCode.Mouse1)) {
			ZoomOut (5.0f);
		} else {
			ZoomIn (15.0f);
		}
	}

	void ZoomIn(float dt){
		if (camera_.orthographicSize <= minSize) {
			camera_.orthographicSize = minSize;
		} else {
			camera_.orthographicSize -= dt * Time.deltaTime;
		}
	}

	void ZoomOut(float dt){
		if (camera_.orthographicSize >= maxSize) {
			camera_.orthographicSize = maxSize;
		} else {
			camera_.orthographicSize += dt * Time.deltaTime;
		}
	}
}
