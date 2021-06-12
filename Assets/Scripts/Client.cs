using UnityEngine;


/// <summary>
/// 
/// Controlo interno dos clientes, individualmente.
/// 
/// </summary>


public class Client : MonoBehaviour 
{
	GameManager GM;			// instância única do GameManager


	Video video;			// script que controla os vídeos


	Rigidbody2D rBody;		// rigidbody para controlar os movimentos

	Animator anim;			// animator para controlar as animações


	Table Table = null;		// componente Table da mesa deste cliente


	bool isWalking;			// está em movimento ?

	bool isSeated;			// tem mesa definida ?

	bool isEating;			// está a comer ?

	bool isPaying;			// está a pagar ?


	public int clientID = -1;	// id deste cliente

	public int productID = -1;	// id do pedido deste cliente

	public int tableID = -1;	// id da mesa deste cliente


	public GameObject videoPlayer;		// objecto que contém o videoplayer

	public GameObject clientRestart;	// local de onde saem os clientes

	public GameObject _orderImage;		// Imagem do produto a receber na mesa

	GameObject orderImage;		// Imagem do pedido


	const float _timeToEat = 5;		// segundos para comer o seu lanche (valor fixo inicial)

	float timeToEat;				// contagem dos segundos para comer

	float clientMood = 60 * 2;		// humor (a diminuir -1 por segundo, conforme espera)



	// Inicialização do cliente :

	void Start () 
	{

		GM = GameManager.GM;					// vai buscar a instância única do GameManager

		rBody = GetComponent<Rigidbody2D> ();	// vai buscar a componente RigidBody deste cliente

		anim = GetComponent<Animator> ();		// vai buscar a componente Animator deste cliente


		video = videoPlayer.GetComponent<Video> ();	// vai buscar o script que controla os vídeos

	}


	// Determina a posição da mesa :

	public void SeatClient (int _clientID, int _productID, int _clientTable) 
	{

		if (! isSeated)			//  Se ainda não tem mesa definida :
		{	
			clientID = _clientID;			// recebe seu ID de cliente

			productID = _productID;			// recebe o id do produto a pedir

			tableID = _clientTable;			// recebe o número da mesa onde sentar

			isSeated = true;				// passa a ter mesa definida e não será chamado a outra


			// Activa movimento e animação para andar :
		
			isWalking = true;

			anim.SetBool ("isWalking", true);

		}

	}



	// FixedUpdate é chamado de forma mais estável do que o Update() :

	void FixedUpdate () 
	{
		
		if (isWalking)		// Move o cliente para a direita, enquanto isWalking for verdadeira :
		{
			rBody.MovePosition(rBody.position + Vector2.right/15);	
		}

		else if (isEating && (timeToEat <= 0))		// tempo para comer concluído :
		{	
			FinishEating ();
		}

		else if (isEating && (timeToEat > 0))		// retira -1 do tempo de comer, a cada segundo :
		{
			timeToEat -= 1 * Time.fixedDeltaTime;
		}

		else if (Table)		// se já tem mesa definida :
		{
			if (Table.isDelivered && !isEating && !isPaying)	// mesa servida, começar a comer :

				StartEating ();

			else if (Table.productID == -1)		// mesa liberada, ir para casa :

				GoHome();

		}

		if (clientMood > 0)		// retira -1 do humor do cliente, a cada segundo :
		{
			clientMood -= 1 * Time.fixedDeltaTime;
		}

	}


	// Ao entrar em colisão :

	void OnTriggerEnter2D(Collider2D col)
	{

		if (col.name == "Mesa" + tableID)		// Se a colisão for com a mesa de número determinado para este cliente
		{
			rBody.MovePosition (col.transform.position + new Vector3(0,.5f,0));	// move para a posição exata da mesa

			isWalking = false;								// desliga o movimento, desativando a Boolean isWalking deste cliente

			anim.SetBool ("isWalking", false);				// desliga a animação, desativando a Boolean isWalking do Animator

			GetComponent<SpriteRenderer> ().sortingOrder = 3;	// passa a personagem para a camada de cima

			Table = col.gameObject.GetComponent<Table> ();		// pega a componente Table desta mesa

			Table.productID = productID;	// atribui ID do pedido à mesa

			AskProduct ();		// Faz o pedido
		}


		if (col.name == "ClientExit")		// Se a colisão for com o limite de saída do cenário :
		{
			rBody.MovePosition (clientRestart.transform.position);	// move cliente para a posição inicial

			isWalking = false;								// desliga o movimento, desativando a Boolean isWalking deste cliente

			anim.SetBool ("isWalking", false);				// desliga a animação, desativando a Boolean isWalking do Animator

	//		++GM.clientsServed;
				
			// Reseta suas variáveis :

			clientID = -1;
			productID = -1;
			tableID = -1;
			isSeated = false;
			isPaying = false;
			Table = null;
		}

	}



	// Faz o pedido :

	void AskProduct () 
	{
		orderImage = Instantiate (_orderImage);		// cria instância do balão de pedido, para este cliente

		orderImage.transform.parent = transform;	// delega instância ao jogador

		orderImage.transform.position = transform.position + new Vector3(0,2f,0);	// posiciona conforme jogador

		orderImage.GetComponentsInChildren<SpriteRenderer>()[1].sprite = DataBase.item[productID].itemSprite; // atribui sprite do produto
	}


	// Começa a comer :

	void StartEating () 
	{
		isEating = true;

		timeToEat = _timeToEat;

		orderImage.SetActive(false);
	}

	// Termina de comer :

	void FinishEating () 
	{
		isEating = false;			// regista que não está a comer

		isPaying = true;			// regista que está a efetuar pagamento


		// Processar o valor e o troco :

		orderImage.SetActive(true);		// reativa balão de pagamento

		orderImage.GetComponentsInChildren<SpriteRenderer>()[1].enabled = false;	// mas desativa imagem do produto

		orderImage.GetComponentsInChildren<SpriteRenderer>()[2].enabled = true;		// e ativa imagem das moedas


		decimal sellingPrice = (decimal)DataBase.item[productID].sellingPrice;		// vai buscar o preço do produto (converte para decimal)
	


		// Estabelece a opção de moeda/nota para pagamento :

		int paymentOption = 0;

		/*
		if (sellingPrice < 0.5) 
		{
			paymentOption = 0;
			change = 0.5 - sellingPrice;
		} 
		else if (sellingPrice < 1) 
		{
			paymentOption = 1;
			change = 1 - sellingPrice;
		} 
		else if (sellingPrice < 2) 
		{
			paymentOption = 2;
			change = 2 - sellingPrice;
		} 
		else if (sellingPrice < 5) 
		{
			paymentOption = 3;
			change = 5 - sellingPrice;
		} 
		else 
		{
			paymentOption = 4;
			change = 10 - sellingPrice;
		}
		*/



		for (int i = 5; i < Payment._currencyValues.Length; i++)		// acha uma moeda/nota
		{
			if (Payment._currencyValues[i] > sellingPrice)					// com valor maior do que o preço
			{
				decimal change = Payment._currencyValues [i] - sellingPrice;		// calcula o valor do troco

				int j = (i - 5);

				Table.FinishEating(j, change);		// e envia à componente Table

				orderImage.GetComponentsInChildren<SpriteRenderer>()[2].sprite = GM.currencySprites[i]; 	// atribui sprite da moeda/nota

				break;
			}
		}

			

	}



	/*
	*	Tudo concluído, cliente dispensado :
	*/

	void GoHome () 
	{
		orderImage.SetActive(false);					// desativa balão do cliente

		// Vai embora :

		isWalking = true;								// reativa o movimento, ativando a Boolean isWalking deste cliente

		anim.SetBool ("isWalking", true);				// reativa a animação, ativando a Boolean isWalking do Animator

		rBody.MovePosition (transform.position + new Vector3(0,.5f,0));	// move posição de volta para o corredor

		GetComponent<SpriteRenderer> ().sortingOrder = 2;	// passa a personagem para a camada de trás
	}

}
