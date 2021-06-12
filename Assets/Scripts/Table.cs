using UnityEngine;

public class Table : MonoBehaviour 
{
	public int tableID;				// ID desta mesa

	public int productID = -1;		// ID do pedido

	public bool isDelivered = false;	// pedido foi entregue?

	public bool isPaying = false;		// está efetuando pagamento?

	public decimal paymentChange;		// troco calculado para esta mesa

	public int paymentOption;			// pagamento efetuado


	/*
	*	Chamado ao tentar entregar pedido à mesa :
	*/

	public bool TryDelivery (int deliveryID) 
	{
		
		// retorna "falso" se a mesa já foi servida, ou se o produto não corresponde ao pedido:

		if ( isDelivered || ( deliveryID != productID ) )
			
			return false;


		// senão, segue adiante:

		GetComponent<SpriteRenderer> ().sprite = DataBase.item[productID].itemSprite;

		isDelivered = true;

		return true;
	}



	/*
	*	Chamado pelo cliente ao terminar de comer :
	*/

	public void FinishEating (int payment, decimal change) 
	{
		isPaying = true;			// regista que está efetuando pagamento

		paymentChange = change;		// regista valor do troco

		paymentOption = payment;	// regista valor do pagamento
	}


	/*
	*	Chamado pelo jogador ao tentar pagar :
	*/

	public void StartPayment () 
	{
		GameManager.paymentChange = paymentChange;		// atualiza Game Manager com o valor do troco desta mesa

		GameManager.paymentOption = paymentOption;		// atualiza Game Manager com o valor do pagamento

		GameManager.productId = productID;				// atualiza Game Manager com o ID do produto

		GameManager.clientTable = GetComponent<Table>();	// atualiza Game Manager com esta mesa
	}


	/*
	*	Pagamento completo :
	*/

	public void FinishPayment () 
	{
		// retorna aos valores iniciais: 

		productID = -1;
		isDelivered = false;
		isPaying = false;

		GameManager.tablesList[tableID] = -1;	// libera mesa para o Game Manager

		GetComponent<SpriteRenderer> ().sprite = null;	// desativa imagem do produto sobre a mesa

	}

}
