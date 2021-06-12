using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour 
{
	
	GameManager GM;		// instância única do GameManager

	private const KeyCode KEY_CODE_INTERACT = KeyCode.X;	// tecla para interagir com as mesas


	Video video;			// script que controla os vídeos

	public GameObject videoPlayer;		// objecto que contém o videoplayer


	public GameObject trayImage;		// imagem da bandeja

	public GameObject productImage;		// imagem do produto na bandeja

	GameObject collisionObject;			// objecto em colisão com o jogador

	int holdingID = -1;			// ID do objecto na bandeja

	Table Table;				// Componente Table da mesa em colisão



	void Start () 
	{

		GM = GameManager.GM;	// vai buscar a instância única do GameManager

		video = videoPlayer.GetComponent<Video> ();	// vai buscar o script que controla os vídeos

	}

	public void Update () 
	{
		if (GameManager.isPaying)	// se o jogador estiver a pagar, retorna daqui
			
			return;
		

		if ( Input.GetKeyDown(KEY_CODE_INTERACT) || Input.GetKeyDown(KeyCode.Return) ) 				// Se a tecla para interacção for pressionada :
		{	
			
			if ( collisionObject != null ) 							// Se houver um objecto em colisão :
			{
				GM.debugLog = "collision object not null";

				if ( collisionObject.CompareTag("KitchenSlot") ) 		// Se o objecto em colisão for uma máquina :
				{
					kitchenInteract ();
				}
				else if ( collisionObject.CompareTag("ClientSlot") ) 	// Se o objecto em colisão for uma mesa do cliente :
				{	
					GM.debugLog = "Coisão com mesa: " 
						+ collisionObject.GetComponent<Table> ().tableID 
						+ " e produto: "
						+ collisionObject.GetComponent<Table> ().productID;

					clientInteract ();
				}
				else
				{
					GM.debugLog = "colisão nula";
				}
			}

		}

	}


	// COLISÕES :


	public void OnTriggerEnter2D(Collider2D col)	// Ao entrar em colisão com qualquer Trigger :
	{
		if (col.CompareTag("ClientSlot") || col.CompareTag("KitchenSlot"))
		{
			collisionObject = col.gameObject;	// atribui o "gameObject" da colisão 

			col.GetComponent<ColliderGlow> ().Glow (true);	// chama a função que activa o efeito "Halo" do objecto
		}
	}


	public void OnTriggerExit2D(Collider2D col)		// Ao sair da colisão com qualquer Trigger :
	{
		if (col.CompareTag("ClientSlot") || col.CompareTag("KitchenSlot"))
		{	
			collisionObject = null;		// descarta o "gameObject" da colisão

			col.GetComponent<ColliderGlow> ().Glow (false);	// chama a função que desactiva o efeito "Halo" do objecto
		}
	}



	// INTERAÇÕES :


	/*
	*	Chamado ao interagir com máquinas dos produtos :
	*/

	void kitchenInteract () 
	{

		holdingID = collisionObject.GetComponent<Machine>().machineId;	// vai buscar o ID do produto

		productImage.GetComponentInChildren<SpriteRenderer>().sprite = DataBase.item[holdingID].itemSprite;	// vai buscar a Sprite do produto na base de dados
				
	}



	/*
	*	Chamado ao interagir com mesas dos clientes :
	*/

	void clientInteract () 
	{
		
		Table = collisionObject.GetComponent<Table> ();	// vai buscar a componente "Table" da colisão


		if (Table.isPaying && GameManager.isPaused) 
		{
			GameManager.isPaused = false;	// Retira a pausa

			return;							// e retorna daqui
		} 
		else 
		{
			GameManager.isPaused = false;	// Retira a pausa mas continua
		}
			

		if ( Table.isPaying )		// Se a mesa estiver a tentar pagar :
		{
			GameManager.isPaying = true;			// avisa ao Game Manager que o jogador está a processar pagamentos

			Table.StartPayment();
		}

		else if ( holdingID == -1 )		// Se a bandeja estiver vazia :
		{	
			if ( !Table.isDelivered && (Table.productID != -1) ) 	// Se houver um pedido por entregar :

				PlayOrderVideo (Table.productID);
		}

		else if ( Table.TryDelivery ( holdingID ) )			// Tenta entregar produto à mesa :
		{	
			holdingID = -1;		// retira o produto da bandeja

			productImage.GetComponentInChildren<SpriteRenderer>().sprite = null;	// descarta a "Sprite" do produto na bandeja
		}

		else if ( Table.productID != -1 )	// Produto não entregue, mas há um pedido :
		{
			video.PlayVideo (10);		// Roda vídeo recusando o produto errado (vídeo de índice 10)
		}
		
	}



	/*
	*	Roda vídeo conforme produto pedido :
	*/

	void PlayOrderVideo (int _productID) 
	{
		int videoId = 0;

		switch (_productID)	// conforme o id do produto, estabelece o id do vídeo :
		{
		case 0:
			videoId = 38;
			break;
		case 1:
			videoId = 29;
			break;
		case 2:
			videoId = 14;
			break;
		case 3:
			videoId = 22;
			break;
		case 4:
			videoId = 17;
			break;
		case 5:
			videoId = 39;
			break;
		case 6:
			videoId = 13;
			break;
		case 7:
			videoId = 24;
			break;
		case 8:
			videoId = 33;
			break;
		case 9:
			videoId = 25;
			break;
		}

		video.PlayVideo (videoId);		// Chama função do script de vídeo
	}

}
