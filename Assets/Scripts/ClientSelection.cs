using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 
/// Localizado no objecto que contém todos os clientes da cena
/// 
/// </summary>


public class ClientSelection : MonoBehaviour 
{

	List<Client> characters;	// Lista de personagens para clientela

	int tableNumber;			// (para a proxima mesa)


    float secondsPerClient = 30;     // Quantos segundos até o próximo cliente



	void Start () 
	{

		characters = GetComponentsInChildren<Client> ().ToList ();		// pega todos os componentes Client dentro deste componente

        // InvokeRepeating ("AddClient", 2, 45);		// invoca a função "AddClient()", após 2 segundos, e a cada 45 segundos

    }


    public void SetLevel(float _seconds)
    {
        secondsPerClient = _seconds;
    }


    public void StartLevel()
    {
        StartCoroutine(CallNextClient());
    }


    IEnumerator CallNextClient()
    {
        while (true)
        {
            Debug.Log("Clientes por segundo: " + secondsPerClient);

            if (!GameManager.isPaused)
                AddClient();

            yield return new WaitForSeconds(secondsPerClient);
        }
    }


    void AddClient () 
	{
		if (characters.Count == 0)		// se a lista de clientes acabou :
		{
			characters = GetComponentsInChildren<Client> ().ToList ();		// pega todos os Clients novamente

			characters = characters.FindAll (x => x.productID == -1);		// sublista de clientes que ainda não têm pedido

			if (characters.Count == 0)	// se a lista de clientes ainda é vazia :	

				return;		// sai desta função sem terminar
		}

		for (tableNumber = 0; tableNumber < 10; tableNumber++)	// Para cada mesa :
		{
			if (GameManager.tablesList[tableNumber] == -1)	// se a mesa estiver livre
			{
				int client = Random.Range (0 , characters.Count);		// escolhe um cliente aleatório, entre 0 e o total

				int product = Random.Range (0 , DataBase.item.Count);	// escolhe um produto aleatório, entre 0 e o total

				characters [client].GetComponent<Client> ().SeatClient (client, product, tableNumber);

				characters.RemoveAt (client);	// remove cliente da lista

				GameManager.tablesList[tableNumber] = product;	// regista produto para a mesa com o GM

				return;		// retorna, interrompendo o foreach
			}
		}

	}
}
