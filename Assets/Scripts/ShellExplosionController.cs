using UnityEngine;
using System.Collections;

public class ShellExplosionController : MonoBehaviour {

    public float lifetime = 1.5f;

	// Use this for initialization
	void Start () {
	    GameObject.Destroy(this.gameObject, lifetime);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
