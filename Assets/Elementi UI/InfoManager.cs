using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoManager : MonoBehaviour {

	public int pageNumber;
	[Space]
	public GameObject panelInfo;
	public GameObject panelBonus;
	public GameObject panelCredits;
	[Space]
	public Button forward;
	public Button backward;


	// Use this for initialization
	void Start () {
		pageNumber = 0;

		panelInfo.SetActive (true);

		panelBonus.SetActive(false);

		panelCredits.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {

		if (pageNumber == 0) {
			//possiamo andare avanti
			forward.interactable = true;
			//Non possiamo andare indietro
			backward.interactable = false;
			// Pagina 1 è visiile
			panelInfo.SetActive (true);
			//Pagina 2 disattiva
			panelBonus.SetActive(false);
			//Pagina 3 disattiva
			panelCredits.SetActive(false);
		}
		else if (pageNumber == 1) {
			//Pagina 1 disattiva
			panelInfo.SetActive (false);
			//Pagina 2 attiva
			panelBonus.SetActive(true);
			//Pagina 3 disattiva
			panelCredits.SetActive(false);

			//Entrambi i bottoni sono interagibili
			forward.interactable = true;
			backward.interactable = true;
		}
		else if (pageNumber == 2) {
			//Non possiamo più andare avanti
			forward.interactable = false;
			//Pagina 2 disattiva
			panelBonus.SetActive(false);
			//Pagina 3 attiva
			panelCredits.SetActive(true);
		}
	}

	public void PageForward (){
		pageNumber++;
	}

	public void PageBackward (){
		pageNumber--;
	}

	public void Reset (){
		pageNumber = 0;
	} 
}
