using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    // 鼠标隐藏
    private bool isCursorLock_ = true;

	// 摄像机切换
	public GameObject mainCamera;
	public GameObject FPSCamera;
	private bool switchCamera = true;

	// 是否开始游戏
	private bool isStartGame_ = false;
	// 游戏是否结束
	public bool isGameOver = false;

	// UI
	public GameObject startMenu;
	public GameObject gameOverMenu;
	public GameObject gamePlayingMenu;
	public GameObject tutorialMenu;
	public UILabel UIScore;
	public UILabel UITime;

    // Joystick
    public GameObject Joystick;

	// Score
	public static int score = 0;
	private float playingTime = 0.0f;
	
	public bool isVectory = false;

    void Start() {
		Time.timeScale = 0;
		score = 0;
		playingTime = 0.0f;
	}

    void Update() {
		if (GameObject.Find ("Player") == null) {
			isGameOver = true;
		}
		if (isStartGame_) {
            Joystick.SetActive(true);
			// UpdateCursorLock ();
			UpdateScore ();
			if (Input.GetKeyDown(KeyCode.Tab)) {
				if (switchCamera) {
					mainCamera.SetActive (false);
					FPSCamera.SetActive (true);
				} else {
					mainCamera.SetActive (true);
					FPSCamera.SetActive (false);
				}
				switchCamera = !switchCamera;
			}
		}
		if (isGameOver) {
            Joystick.SetActive(false);
			gameOverMenu.SetActive (true);
			if (Input.GetKey (KeyCode.R)) {
				SceneManager.LoadScene ("Main");
			}
			mainCamera.SetActive (true);
		}
	}

	public void FixedUpdate() {
		if (!isGameOver && isStartGame_) {
			playingTime += Time.deltaTime;
		}
	}

	void checkWin() {

	}

    // 隐藏鼠标指针
    public void UpdateCursorLock() {
        if (Input.GetKeyUp(KeyCode.Escape) || isGameOver) {
            isCursorLock_ = false;
        }
        else if (Input.GetMouseButtonUp(0)) {
            isCursorLock_ = true;
        }

        if (isCursorLock_) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else if (!isCursorLock_) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
	
	public void StartGame() {

		startMenu.SetActive(false);
		gamePlayingMenu.SetActive (true);
		tutorialMenu.SetActive (true);

		Time.timeScale = 1;
		isStartGame_ = true;
	}

	public void ReloadGame() {
		SceneManager.LoadScene ("Main");
		EnemyFactory.EnemyCount = 0;
	}

	public void QuitGame() {
		Application.Quit ();
	}

	public void UpdateScore() {
		UIScore.text = "Score: " + score;
		UITime.text = "Time: " + playingTime.ToString("##.###") + "s";
	}
}