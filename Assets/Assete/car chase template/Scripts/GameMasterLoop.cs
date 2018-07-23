using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameMasterLoop : MonoBehaviour {

	public GameObject car1;
	public GameObject car2;
	public bool gameStarted;
	private float startTime;
	private bool beginTimer = false;
	public Text guiTimeText;
	public bool gameOver;
	public Button pauseButton;
	public Button turnRightButton;
	public Button turnLeftButton;
	public GameObject pauseMenu;
	public GameObject resumeButton;
	private SimpleCarController carController;
	// Use this for initialization
	void Start () {
	
		//first show payer selected car to drive
		showSelectedPlayerCar();
		carController = GameObject.FindGameObjectWithTag("Player").GetComponent<SimpleCarController>();
	}
	
	// Update is called once per frame
	void Update () {
		updateGUITime();
	}

	public void startGame() {

		guiTimeText.gameObject.SetActive(true);
		gameStarted = true;
		startTimer();
		pauseButton.gameObject.SetActive(true);
		turnLeftButton.gameObject.SetActive(true);
		turnRightButton.gameObject.SetActive(true);
	}


	public void showSelectedPlayerCar() {
	
		switch(PlayerPrefHandler.getSelectedCar()) {
		
			case 1:
			car1.SetActive(true);
			break;

			case 2:
			car2.SetActive(true);
			break;
		}
	}

	void updateGUITime() {
		if(gameOver || !beginTimer) 
			return;
		
		float guiTime = Time.time - startTime;
		int minutes;
		int seconds;
		int fraction;
		
		minutes = (int) guiTime / 60;
		seconds = (int) guiTime % 60;
		fraction = (int) (guiTime * 100) % 100;
		
		guiTimeText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, fraction);

	}

	public void startTimer () {
		
		if(beginTimer)
			return; //call this function once
		beginTimer = true;
		//get a hold of start time
		startTime = Time.time;
	}
	
	public void stopTimer() {
		beginTimer = false;
	}

	public void pause() {
	
		//show pause menu
		pauseMenu.SetActive(true);
		resumeButton.SetActive(true);
		pauseButton.gameObject.SetActive(false);
		turnLeftButton.gameObject.SetActive(false);
		turnRightButton.gameObject.SetActive(false);
		Time.timeScale = 0f;

	}

	public void resume() {

		pauseButton.gameObject.SetActive(true);
		pauseMenu.SetActive(false);
		turnLeftButton.gameObject.SetActive(true);
		turnRightButton.gameObject.SetActive(true);
		Time.timeScale = 1f;
	}

	public void restart() {
		Time.timeScale = 1.0f;
		Application.LoadLevel(Application.loadedLevel);
	}

	public void mainMenu() {

		Time.timeScale = 1f;
		Application.LoadLevel(0); //go back to main menu scene
	}

	public void gameOverMenuShow() {
	
		//show pause menu
		Time.timeScale = 1.0f;
		pauseMenu.SetActive(true);
		resumeButton.SetActive(false);
		pauseButton.gameObject.SetActive(false);
		turnLeftButton.gameObject.SetActive(false);
		turnRightButton.gameObject.SetActive(false);
	}
	
}
