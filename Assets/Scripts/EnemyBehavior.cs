using System;
using UnityEngine;
using Random = System.Random;

public class EnemyBehavior : MonoBehaviour {

    private GameObject player_;
    private TankController tank_;
    private UnityEngine.AI.NavMeshAgent navigation_;
    public ENEMYTYPE enemyType;

    private float blueTankActionRate = 10.0f;
    private float blueTankNextTime = 0.0f;
    private float blueTankActionTime = 0.0f;

    public enum ENEMYTYPE {
        RED,
        BLACK,
        BLUE,
        GOLD
    }

    void Start() {
        tank_ = gameObject.GetComponent<TankController>();
        player_ = GameObject.Find("Player");
        navigation_ = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    void FixedUpdate() {
        Action();
    }

    void Action() {
        if (player_ == null) {
            return;
        }
        if (this.enemyType == ENEMYTYPE.RED) {
            navigation_.SetDestination(player_.transform.position);
            tank_.Fire();
        }
        else if (this.enemyType == ENEMYTYPE.BLUE) {
            BlueTankBehavior();
        }
        else if (this.enemyType == ENEMYTYPE.BLACK) {
            this.transform.LookAt(player_.transform);
            tank_.StorePower(1.0f*Time.deltaTime);
            if (EstimatePlayer()) {
                tank_.Fire();
            }
        }
        else if (this.enemyType == ENEMYTYPE.GOLD) {
            GoldTankBehavior();
        }
    }

    private bool EstimatePlayer() {
        float targetDistance = Vector3.Distance(player_.transform.position, this.transform.position);
        float calculatDistance;
        if (targetDistance < 30.0f) {
            calculatDistance = tank_.shellSpeed*tank_.Power*1.02f;
        }
        else {
            calculatDistance = tank_.shellSpeed*tank_.Power*1.27f;
        }
        if (targetDistance < 11.0f || Math.Abs(tank_.Power - tank_.MaxPower) < 0.001 ||
            Mathf.Abs(targetDistance - calculatDistance) <= 1.50f) {
            return true;
        }
        else {
            return false;
        }
    }

    // 蓝色坦克行动类型
    private float attackTime = 0.0f;

    private void BlueTankBehavior() {
        blueTankActionTime += Time.deltaTime;

        if (blueTankActionTime > blueTankNextTime) {
            blueTankNextTime += blueTankActionRate;

            tank_.LookDirection(UnityEngine.Random.Range(30.0f, 60.0f));
        }

        tank_.Move(3.0f);
        attackTime += Time.deltaTime;

        if ((int) (attackTime)%3 == 0) {
            this.transform.LookAt(player_.transform);
            tank_.StorePower(1.0f*Time.deltaTime);
            if (EstimatePlayer()) {
                tank_.Fire();
            }
        }

    }

    // 金色坦克行动
    private void GoldTankBehavior() {
        blueTankActionTime += Time.deltaTime;

        if (blueTankActionTime > blueTankNextTime) {
            blueTankNextTime += blueTankActionRate;

            Vector3 newPosition = player_.transform.position
                                  + new Vector3(UnityEngine.Random.Range(0.0f, 20.0f),
                                      UnityEngine.Random.Range(0.0f, 20.0f),
                                      UnityEngine.Random.Range(0.0f, 20.0f)
                                  );
            navigation_.SetDestination(newPosition);
        }

        this.transform.LookAt(player_.transform);
        tank_.StorePower(1.0f*Time.deltaTime);
        if (EstimatePlayer()) {
            tank_.Fire();
        }
    }
}
