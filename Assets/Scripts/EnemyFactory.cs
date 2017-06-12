using UnityEngine;
using System.Collections;

public class EnemyFactory : MonoBehaviour {

	// 敌人数量
	public static int EnemyCount = 0;
	public int EnemyMaxCount = 10;
	private int nextEnemyType = 1;
	public GameObject EnemyBlue;
	public GameObject EnemyRed;
	public GameObject EnemyBlack;
	public GameObject EnemyGold;

	// 控制敌人生成时间
	public float enemySpawnTime = 5.0f;
	private float nextEnemySpawn_;

	void Start () {
		nextEnemySpawn_ = 5.0f + Time.time;
	}
	
	void FixedUpdate () {

		if (EnemyCount < EnemyMaxCount && Time.time > nextEnemySpawn_) {
			Vector3 enemyPosition = new Vector3 (Random.Range (-40, 45), 0, Random.Range (-20, 40));
			Vector3 enemyRotation = new Vector3(0, Random.Range(0, 360),0);
			if (nextEnemyType % 7 == 0) {
				GameObject.Instantiate (EnemyGold, enemyPosition, Quaternion.Euler (enemyRotation));	
			}
			else if (nextEnemyType % 5 == 0) {
				GameObject.Instantiate (EnemyBlack, enemyPosition, Quaternion.Euler (enemyRotation));
			} else if (nextEnemyType % 3 == 0) {
				GameObject.Instantiate (EnemyRed, enemyPosition, Quaternion.Euler (enemyRotation));
			} else {
				GameObject.Instantiate (EnemyBlue, enemyPosition, Quaternion.Euler (enemyRotation));
			}
			nextEnemyType++;
			EnemyCount++;
			nextEnemySpawn_ = Time.time + enemySpawnTime;
		}
	}
}
