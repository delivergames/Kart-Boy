using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

public class ControlButtonScript : UIBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler {

	public enum myAction { left,right};
	
	public myAction action;
	
	public void OnPointerDown( PointerEventData eventData ) {

		switch(action) {

		case myAction.right:
			GameObject.FindGameObjectWithTag("Player").GetComponent<SimpleCarController>().turnRight();
			break;

		case myAction.left:
			GameObject.FindGameObjectWithTag("Player").GetComponent<SimpleCarController>().turnLeft();
			break;
	}
	}

	
	public void OnPointerUp( PointerEventData eventData ) {

		switch(action) {
			
		case myAction.right:
			GameObject.FindGameObjectWithTag("Player").GetComponent<SimpleCarController>().stopTuning();
			break;
			
		case myAction.left:
			GameObject.FindGameObjectWithTag("Player").GetComponent<SimpleCarController>().stopTuning();
			break;
		}
	}
	
	
	public void OnPointerExit( PointerEventData eventData ) {

	}
}