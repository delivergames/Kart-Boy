using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour {

	public Button car1Button;
	public Button car2Button;
	public Button playButton;
	public Button mainMenuButton;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void playButtonPressed() {
	
		car1Button.gameObject.SetActive(true);
		car2Button.gameObject.SetActive(true);
		mainMenuButton.gameObject.SetActive(true);
		playButton.gameObject.SetActive(false);
	}

	public void car1ButtonPressed() {

		//set the car type selected
		PlayerPrefHandler.setSelectedCar(1);
		Application.LoadLevel(1); //load main  level1
	}

	public void car2ButtonPressed() {
		
		//set the car type selected
		PlayerPrefHandler.setSelectedCar(2);
		Application.LoadLevel(1); //load main  level1
	}

	public void backButtonPressed() {
		
		car1Button.gameObject.SetActive(false);
		car2Button.gameObject.SetActive(false);
		mainMenuButton.gameObject.SetActive(false);
		playButton.gameObject.SetActive(true);
	}

}
