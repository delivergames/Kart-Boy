using UnityEngine;
using System.Collections;

public class PlayerPrefHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static void setSelectedCar(int selectedCar) {
	
		PlayerPrefs.SetInt("selectedcar",selectedCar);
	}

	public static int getSelectedCar() {
		if(PlayerPrefs.HasKey("selectedcar")) {
		
			return PlayerPrefs.GetInt("selectedcar");
		}
		else {
			return 1; //default car
		}
	}
}
