using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Globalization;

public class UI : MonoBehaviour 
{

	GameManager GM;		// instância única do GameManager


    public ClientSelection clientSelection;     // objecto parent que controla seleção de clientes


	public Slider timeSlider;	// slider que exibe o tempo na interface


	public Text timeLeft;		// componente Text do canvas, com o tempo restante até acabar o dia

	public Text debugLog;		// componente Text do canvas, com texto de ajuda

	public Text clientsServed;	// componente Text do canvas, com número de clientes atendidos

	public Text highScore;		// componente Text do canvas, com pontuação do jogador


	//	PAINEL DE FIM DE JOGO:

	public GameObject gameOverPanel;

	public Text gameOverClients;

	public Text gameOverScore;


	public GameObject paymentUI;



	void Start () 
	{

		GM = GameManager.GM;	// vai buscar a instância única do GameManager

        GameManager.isPaused = true;

		Time.timeScale = 0;

	}


	void FixedUpdate () 
	{

		timeSlider.value = GM.secondsPerDay;					// update time slider


//		timeLeft.text = GM.secondsPerDay.ToString("####");		// imprime texto do Timer na tela

		clientsServed.text = GM.clientsServed.ToString();		// imprime texto com número de clientes atendidos

//		highScore.text = GM.highScore.ToString();				// imprime texto com a pontuação calculada


		highScore.text = GM.scoreMoney.ToString("C", CultureInfo.CreateSpecificCulture("pt-PT"));		// imprime texto com a pontuação calculada

		debugLog.text = GM.debugLog;							// imprime texto de ajuda na tela

	}

	public void PlayGame (bool play) 
	{
		if (play) 
		{
            GameManager.isPaused = false;

			Time.timeScale = 1;

			Cursor.lockState = CursorLockMode.Locked;

			Cursor.visible = false;

			paymentUI.SetActive (true);

            clientSelection.StartLevel();
		} 
		else 
		{
			Time.timeScale = 0;

			Cursor.lockState = CursorLockMode.None;

			Cursor.visible = true;

			GameManager.isPaying = false;

			paymentUI.SetActive (false);

			EventSystem.current.SetSelectedGameObject (GameObject.Find("Jogar-Pausa"));
		}
	}


	public void LevelOver()		// Acaba o ciclo do dia
	{
		
		paymentUI.SetActive (false);


		// manage Game Over panel:

		gameOverPanel.SetActive (true);

		gameOverClients.text = "Clientes : " + GM.clientsServed.ToString();

//		gameOverScore.text = "Pontos : " + GM.highScore.ToString();

		gameOverScore.text = "Lucro : " + GM.scoreMoney.ToString("C", CultureInfo.CreateSpecificCulture("pt-PT"));

		EventSystem.current.SetSelectedGameObject (GameObject.Find("Jogar-Novo"));
	}

}


