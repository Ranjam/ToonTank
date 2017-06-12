using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TankController : MonoBehaviour {
    // 枪口
    private Transform muzzle;
    // 坦克刚体
    private Rigidbody rigidbody_;
    // 子弹预置体
    public GameObject shell;
    // 子弹速度
    public float shellSpeed = 15.0f;
    // 血量
    public int HP = 100;
    // 最大血量
    private int maxHP_;
    // 血量显示
    public Slider HealthSlider;
    // 血量颜色
    public Image HpImage;
    // 坦克爆炸特效
    public GameObject tankExplosionPrefab;
    // 蓄力显示
    public Slider AimSlider;
    // 当前蓄力
	private float power_;
	public float Power {
		get { 
			return power_;
		}
	}
	// 最大蓄力
	private float maxPower_;
	public float MaxPower{
		get { 
			return maxPower_;
		}
	}
    // 是否处于蓄力中
	public bool isCharging_;

    // 控制子弹发射频率
    public float fireFrequency;
    private float fireCycle_;
    private float nextFireTime_;

    // 初始化
    void Start() {
        // 获取Tank的刚体组件
        rigidbody_ = this.GetComponent<Rigidbody>();

        // 获取枪口位置
        muzzle = transform.Find("Muzzle");
       
        // 满血
        maxHP_ = HP;
        HpImage.color = Color.green;

        // 初始力量倍数为 1.0
        power_ = 1.0f;
        isCharging_ = false;
		maxPower_ = AimSlider.maxValue;

        // 子弹发射周期
        fireCycle_ = 1.0f / fireFrequency;
        nextFireTime_ = 0.0f;
    }

    void Update() {
        // 更新血量显示
        HealthSlider.value = (float) HP / maxHP_;
        // 更新力量显示
        AimSlider.value = power_;
    }

    public void Move(float speed) {
        // 给予Tank前进与后退速度
        rigidbody_.velocity = this.transform.forward*speed;
    }

    private float GetAngle(Vector3 a, Vector3 b) {

        // 通过反余弦函数获取 向量 a、b 夹角（默认为 弧度）
        float angle = Mathf.Acos(Vector3.Dot(a.normalized, b.normalized)) * Mathf.Rad2Deg;
        // 叉乘
        Vector3 c = Vector3.Cross(a, b);

        if (c.y > 0) {
            return angle;
        }

        return 360.0f - angle;
    }

    public void LookAt(Vector2 deltaPosition) {
        if (deltaPosition != Vector2.zero) {
            Vector3 towards = new Vector3(deltaPosition.x, 0.0f, deltaPosition.y);
            Vector3 offset = new Vector3(-1, 0, 1);
            float angle = GetAngle(offset, towards);
            transform.eulerAngles = new Vector3(0, angle, 0);
        }
    }

    public void Look(float angularSpeed) {
        // 坦克旋转方向
        rigidbody_.angularVelocity = this.transform.up*angularSpeed;
    }

    public void LookDirection(float angular) {
        transform.Rotate(new Vector3(0.0f, 0.0f, angular));
    }

    // 开火
    public void Fire() {
		if (Time.time > nextFireTime_) {
			nextFireTime_ = Time.time + fireCycle_;
			// 射击音效
			AudioSource shootAudioSource = GameObject.Find ("FireAudio").GetComponent<AudioSource> ();
			shootAudioSource.Play ();
			// 实例化子弹
			GameObject shellInstance = GameObject.Instantiate (shell, muzzle.position, muzzle.rotation) as GameObject;
			shellInstance.GetComponent<Rigidbody> ().velocity = shellInstance.transform.forward * shellSpeed * power_;
			shellInstance.GetComponent<ShellController> ().Owner = gameObject;
		}
        // 力量倍数归零
        power_ = 1.0f;
        AudioSource shootChargingSource = GameObject.Find("ShootChargingAudio").GetComponent<AudioSource>();
        if (shootChargingSource.isPlaying) {
            shootChargingSource.Stop();
        }
        isCharging_ = false;
    }

    // 受伤伤害计算
    public void TakeDamage() {
        ChangeHP(-(int)(Random.Range(20, 30) * power_));
    }

	public void TakeResume() {
		ChangeHP ((int)(Random.Range (20, 30)));
	}

    // 血量改变
    public void ChangeHP(int value) {
        HP += value;
		if (HP > maxHP_) {
			HP = maxHP_;
		}

        if (HP <= 0) {
            TankDestroy();
        }else if (HP < 0.5*maxHP_) {
            HpImage.color = Color.red;
        }else {
            HpImage.color = Color.green;
        }
    }
    // 储存力量
    public void StorePower(float value) {
        
        AudioSource shootChargingSource = GameObject.Find("ShootChargingAudio").GetComponent<AudioSource>();
        // fire按住0.5s后才播放蓄力音效
        if (!shootChargingSource.isPlaying && isCharging_ == false) {
            shootChargingSource.PlayDelayed(0.5f);
        }
        // 蓄力
        isCharging_ = true;
        power_ += value;
        if (power_ > maxPower_) {
            power_ = maxPower_;
        }
    }

    // 坦克炸毁
    public void TankDestroy() {
        // 炸毁音效
        AudioSource explosionAudioSource = GameObject.Find("TankExplosionAudio").GetComponent<AudioSource>();
        explosionAudioSource.Play();
        GameObject.Destroy(this.gameObject);
        GameObject.Instantiate(tankExplosionPrefab, transform.position + transform.up, transform.rotation);
		if (this.tag == "Enemy") {
			EnemyFactory.EnemyCount--;
			GameManager.score += 100;
		}
    }
}