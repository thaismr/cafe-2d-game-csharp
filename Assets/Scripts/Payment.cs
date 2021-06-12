using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Payment : MonoBehaviour
{

	const KeyCode KEY_CODE_ADD = KeyCode.UpArrow;

	const KeyCode KEY_CODE_SUB = KeyCode.DownArrow;

	const KeyCode KEY_CODE_LEFT = KeyCode.LeftArrow;

	const KeyCode KEY_CODE_RIGHT = KeyCode.RightArrow;

	const KeyCode KEY_CODE_ACCEPT = KeyCode.X;

	const KeyCode KEY_CODE_CANCEL = KeyCode.Z;


	// Alternative key codes :

	const KeyCode KEY_CODE_ADD_ALT = KeyCode.W;

	const KeyCode KEY_CODE_SUB_ALT = KeyCode.S;

	const KeyCode KEY_CODE_LEFT_ALT = KeyCode.A;

	const KeyCode KEY_CODE_RIGHT_ALT = KeyCode.D;

	const KeyCode KEY_CODE_ACCEPT_ALT = KeyCode.Return;

	const KeyCode KEY_CODE_CANCEL_ALT = KeyCode.Backspace;



	private const string INVENTORY_IMAGES_PATH  = "Prefabs/Currency";

	private int[] _currencyCount = {0,0,0,0,0,0,0,0,0,0,0};		// Quantidade de cada nota/moeda (11 slots)

	public static decimal[] _currencyValues = {0.01M,0.02M,0.05M,0.10M,0.20M,0.50M,1.00M,2.00M,5.00M,10.00M,20.00M};	// M = com dois decimais

	public decimal _currencyTotal;

	public decimal _currencyCompare = 0.05M;

	public Text _totalScore;

	public Text _alerts;

	public Button[] _slots;

	public EventSystem _eventSystem;

	public Button _selectedButton;

	public int _selectedSlot;

	public bool _inventoryIsActive;

	public GameObject _currencyPanel;

	private GameObject _currencyInstance;

	public GameObject panelPosition;

	public GameObject panelBackground;

	public GameObject panelPause;

	public Sprite[] productSprites;

	public Sprite[] tagSprites;

	Image _currencyImage;



	Video video;			// script que controla os vídeos

	public GameObject videoPlayer;		// objecto que contém o videoplayer



	void Start () 
	{
		video = videoPlayer.GetComponent<Video> ();		// vai buscar o script que controla os vídeos
	}


	// SCORE UI

	public void SetScore(int value)
	{
		_totalScore.text = value.ToString();
	}


	// ALERTS/DEBUG UI

	public void SetAlert(string text)
	{
		_alerts.text = text;
	}


	// PAUSE PANEL

	public void PauseMenu()
	{
		Time.timeScale = 0;

		Cursor.lockState = CursorLockMode.None;

		Cursor.visible = true;


		panelPause.SetActive (true);

		EventSystem.current.SetSelectedGameObject (GameObject.Find("Jogar-Pausa"));


		gameObject.SetActive (false);
	}




	// PAINEL DE PAGAMENTO

	public void SetCurrency()	// Imagens das moedas e notas
	{
		panelBackground.SetActive (true);

		_currencyCompare = GameManager.paymentChange;	// vai buscar valor do troco no Game Manager


		// refaz painel de pagamento por inteiro, para reorganizar notas e moedas:

		if (_currencyInstance != null)
			Destroy (_currencyInstance);

		_currencyInstance = Instantiate (_currencyPanel);

		_currencyInstance.transform.SetParent (this.transform);

		_currencyInstance.transform.position = panelPosition.transform.position;


		// Atualiza imagem do produto e etiqueta de preço:

		Image[] productImages = _currencyInstance.GetComponentsInChildren<Image> ();

		productImages[1].sprite = tagSprites[GameManager.productId];

		productImages[1].transform.localScale = new Vector3 (0.8f,0.8f, 1f);

		productImages[2].sprite = productSprites [GameManager.productId];

		productImages[2].transform.localScale = new Vector3 (1f,1f, 1f);



		// Exibe as moedas e notas :

		for (int i = (_currencyCount.Length - 1); i >= 0; i--)		// Gambiarra para exibir do maior ao menor valor
		{	
			if (_currencyCount[i] > 0)
			{
				_currencyImage = Resources.Load<Image> (INVENTORY_IMAGES_PATH +"/"+ i);

				for (int j = 0; j < _currencyCount[i]; j++)	
				{
					_currencyImage = Instantiate (_currencyImage);
					_currencyImage.rectTransform.SetParent (_currencyInstance.transform);
				}
			}
		}

		_currencyInstance.GetComponentInChildren<Text>().color = _currencyTotal == _currencyCompare ? Color.green : Color.red;

		_currencyInstance.GetComponentInChildren<Text>().text = " =  " + _currencyTotal + " €";

		_currencyInstance.GetComponentInChildren<Text>().GetComponent<RectTransform>().SetAsLastSibling ();
	}

	public void CurrencySelect(int slot)		// Seleção dos botões das moedas e notas
	{
		_selectedSlot = slot;

	//	panelBackground.SetActive (true);

		SetCurrency ();
	}


	public void CurrencyAdd()		// Seta para cima
	{
		++_currencyCount [_selectedSlot];

		_currencyTotal += _currencyValues [_selectedSlot];

		SetCurrency ();		// Atualiza imagens na UI
	}

	public void CurrencySubtract()		// Seta para baixo
	{
		if (_currencyCount [_selectedSlot] > 0) 
		{
			--_currencyCount [_selectedSlot];

			_currencyTotal -= _currencyValues [_selectedSlot];

			SetCurrency ();		// Atualiza imagens na UI
		}
	}



	// INVENTORY PANEL

	public void SetItemCollected(int i, int collectableId, int count)	// Update single slot
	{
		_slots [i].interactable = true;
		_slots [i].GetComponentInChildren<RawImage> ().texture = Resources.Load<Texture> (INVENTORY_IMAGES_PATH +"/"+ collectableId);
		_slots [i].GetComponentInChildren<Text> ().text = DataBase.item [collectableId].name + "(" + count + ")";
	}

	public void SetInventory(int[] collectableId, int[] count)	// Update all slots
	{
		for (int i = 0; i < _slots.Length; i++)
		{	
			if (collectableId [i] != -1)
			{
				_slots [i].interactable = true;
				_slots [i].GetComponentInChildren<RawImage> ().texture = Resources.Load<Texture> (INVENTORY_IMAGES_PATH +"/"+ collectableId[i]);
				_slots [i].GetComponentInChildren<Text> ().text = DataBase.item [collectableId [i]].name + "(" + count[i] + ")";
			}
			else
			{
				_slots [i].interactable = false;
				_slots [i].GetComponentInChildren<RawImage> ().texture = null;
				_slots [i].GetComponentInChildren<Text> ().text = "";
			}
		}
	}


	// CURRENCY NAVIGATION

	void CurrencySelectLeft() 
	{			
		if (_selectedSlot > 0)
			
			_selectedButton = _slots [--_selectedSlot];

		_selectedButton.Select();
	}

	void CurrencySelectRight() 
	{			
		if (_selectedSlot < _slots.Length)
			
			_selectedButton = _slots [++_selectedSlot];

		_selectedButton.Select();
	}



	void AcceptPayment() 
	{

//		panelBackground.SetActive (true);	// Abrir painel de pagamento

		SetCurrency ();


		if (_currencyCompare == _currencyTotal) 	// Se o troco está correto :
		{
			GameManager.GM.scoreMoney += (decimal)DataBase.item [GameManager.productId].sellingPrice;

			GameManager.GM.clientsServed += 1;

			GameManager.clientTable.FinishPayment();

			CancelPayment ();

			video.PlayVideo (40);		// Roda vídeo se despedindo
		}

		else if (_currencyTotal == 0)	// Se jogador(a) ainda não selecionou nenhum troco :
		{
			PlayPaymentValue ();

			SetCurrency ();
		}
	}

	void CancelPayment() 
	{
		Destroy (_currencyInstance);

		GameManager.isPaying = false;

		panelBackground.SetActive (false);


		// reinicia valores :

		_currencyTotal = 0;

		_currencyCount = new int[] {0,0,0,0,0,0,0,0,0,0,0};

		_selectedSlot = 0;

		_selectedButton = null;

		_eventSystem.SetSelectedGameObject(null);
	}


	// GERENCIA OS VÍDEOS :

	public void PlayPaymentValue()
	{
		int videoId = 0;

		switch (GameManager.paymentOption)	// conforme o valor do pagamento, estabelece o id do vídeo :
		{
		case 0:
			videoId = 4;
			break;
		case 1:
			videoId = 0;
			break;
		case 2:
			videoId = 1;
			break;
		case 3:
			videoId = 2;
			break;
		case 4:
			videoId = 3;
			break;
		}

		video.PlayVideo (videoId);		// Chama função do script de vídeo
	}


		

	// INVENTORY NAVIGATION


	public void SelectInventoryButton() 
	{			
		if (_inventoryIsActive) // If already active, then close
		{
			_eventSystem.SetSelectedGameObject (null);
			_inventoryIsActive = false;
	//		Player.Freeze (false);
			//			Time.timeScale = 1;
		}
		else if (_selectedButton.interactable)
		{
			_selectedButton.Select ();
			_inventoryIsActive = true;
	//		Player.Freeze (true);
			//			Time.timeScale = 0;
		}
	}




	void Update()
	{
		// Botões de pagamento :

		if ( ( Input.GetKeyDown(KEY_CODE_ADD) || Input.GetKeyDown(KEY_CODE_ADD_ALT) ) && GameManager.isPaying)
		{
			CurrencyAdd();
		}
		else if ( ( Input.GetKeyDown(KEY_CODE_SUB) || Input.GetKeyDown(KEY_CODE_SUB_ALT) ) && GameManager.isPaying)
		{
			CurrencySubtract();
		}
		else if ( ( Input.GetKeyDown(KEY_CODE_LEFT) || Input.GetKeyDown(KEY_CODE_LEFT_ALT) ) && GameManager.isPaying)
		{
			CurrencySelectLeft();
		}
		else if ( (Input.GetKeyDown(KEY_CODE_RIGHT) || Input.GetKeyDown(KEY_CODE_RIGHT_ALT) ) && GameManager.isPaying)
		{
			CurrencySelectRight();
		}
		else if ( (Input.GetKeyDown(KEY_CODE_ACCEPT) || Input.GetKeyDown(KEY_CODE_ACCEPT_ALT) ) && GameManager.isPaying)
		{
			AcceptPayment();
		}
		else if ( (Input.GetKeyDown(KEY_CODE_CANCEL) || Input.GetKeyDown(KEY_CODE_CANCEL_ALT) ) && GameManager.isPaying)
		{
			CancelPayment();
		}
		else if ( (Input.GetKeyDown(KEY_CODE_CANCEL) || Input.GetKeyDown(KEY_CODE_CANCEL_ALT) ) && !GameManager.isPaying)
		{
			PauseMenu();
		}
    }




}
