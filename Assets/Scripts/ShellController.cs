using UnityEngine;
using System.Collections;

public class ShellController : MonoBehaviour {
    // 子弹爆炸特效
    public GameObject shellExplosionPrefab;

	private GameObject owner_;
	public GameObject Owner{
		get { 
			return owner_;
		}
		set { 
			owner_ = value;
		}
	}

    void Start() {}

    void Update() {}

    // 子弹碰撞检测
    void OnTriggerEnter(Collider collider) {

		// 如果是子弹就不爆了
		if (collider.tag == "Shell") {
			return;
		}

		GameObject.Instantiate (shellExplosionPrefab, transform.position, transform.rotation);
		GameObject.Destroy (this.gameObject);

		Explosion ();

		if (collider.tag == "Enemy") {
			collider.SendMessage ("TankDestroy");
			if (owner_ != null) {
				owner_.SendMessage ("TakeResume");
			}
		}
		else if (collider.tag == "Tank" || collider.tag == "Enemy" || collider.tag == "Player") {
			collider.SendMessage ("TakeDamage");
		}
	}

    void Explosion() {
        // 子弹爆炸音效
        AudioSource explosionAudioSource = GameObject.Find("ShellExplosionAudio").GetComponent<AudioSource>();
        explosionAudioSource.Play();
    }
}