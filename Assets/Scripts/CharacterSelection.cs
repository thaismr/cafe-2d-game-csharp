using UnityEngine;

/// <summary>
/// 
/// Localizado no objecto que contém todas as personagens jogáveis da cena
/// 
/// </summary>
/// 

public class CharacterSelection : MonoBehaviour 
{

	Player[] characters;		// Lista de personagens para escolha do jogador



	public void SelectCharacter (GameObject playerCharacter)		// Recebe o parâmetro  'playerCharacter'
	{

		characters = GetComponentsInChildren<Player>();		// pega todos os componentes Player dentro deste componente


		// Para garantir que só um personagem esteja activo, primeiro desabilita todos :

		foreach (Player chara in characters)		// para cada personagem da lista
			
			chara.gameObject.SetActive(false);		// desabilitar o gameObject de todos
		

		// Agora, activa só a personagem selecionada pelo jogador :

		playerCharacter.SetActive (true);

	}

}
