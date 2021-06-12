using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 
/// BASE DE DADOS : 
/// 
///	Gerencia as informações sobre produtos e máquinas.
/// 
/// </summary>


public class DataBase : MonoBehaviour 
{

	// ESQUELETO DA BASE DE DADOS :


	// ITENS PARA CONSUMO :

	[System.Serializable]		// Serializable para possibilitar a criação de uma Lista com esta classe
	public class Item
	{

		public string name;					// nome do produto

		public Sprite itemSprite;			// imagem Sprite deste item

		public int machineID;				// máquina de venda ou produção


		// ( Unity não serializa variáveis do tipo decimal; converter onde necessário: )

		public float buyingPrice = 0.05f,		// Itens com preço de compra = 0 só poderão ser criados pelo jogador

					sellingPrice = 0.15f,		// Itens com preço de venda = 0 não podem ser vendidos (certos ingredientes, como alface)

					cookingTime = 1f;			// Tempo de preparo (em segundos) - Valor = 0 para itens prontos, como sumos


		public int[] ingredients;		//  cada item pode ser composto por 2 materiais :
										//	(hamburguer é carne + pão, cheeseburguer é hamburguer + queijo, etc.)
	}



	// INICIALIZAÇÃO DA BASE DE DADOS :

	public static DataBase DB;				// static para ser acessível por qualquer script

	public List<Item> _item = new List<Item>();		// Lista "non-static" para preenchimento a partir do editor Unity

	public static List<Item> item;					// Cópia "static" para acesso directo, em qualquer script

	public static List<Item> hasIngredients;		// Sub-lista de itens com + de 1 ingrediente


	void Awake()
	{

		// Instância única da DataBase :

		if (DB == null)
		{
			DB = this;
		}
		else if (DB != this)
		{
			Destroy (gameObject);
		}	


		// Lista com TODOS os itens:

		item = new List<Item>(_item);	// Copia a base de dados "_item" preenchida a partir do editor Unity


		// Sublista de itens com mais de 1 ingrediente:

		hasIngredients = item.FindAll (x => x.ingredients.Length > 1);

	}


	// FUNÇÕES DE ACESSO A DADOS :

	public static int Cooking(int holding, int makeuse)	// Retorna Id do Item (ou -1) a produzir, conforme ingredientes
	{
/*		
		Item cooked = hasIngredients.Find (
			x => ((x.ingredients [0] == holding && x.ingredients [1] == makeuse) || (x.ingredients [0] == makeuse && x.ingredients [1] == holding))
		);

*/

//		return crafted == null ? -1 : crafted.id;

		return 0;

	}

}
