using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Video : MonoBehaviour 
{
	
	public List<VideoClip> videos;		// Lista de arquivos de vídeo (preenchida no Unity)

	VideoPlayer videoPlayer;			// Componente Video Player deste objecto



	void Start () 
	{
		videoPlayer = GetComponent<VideoPlayer> ();		// Pega a componente VideoPlayer deste objecto

		InvokeRepeating ("HideVideo", 1, 1);
	}



	void HideVideo () 
	{
		if ( !videoPlayer.isPlaying )		// se o vídeo NÃO estiver a rodar e ainda estiver activo :
			
			videoPlayer.enabled = false;		// desativa o VideoPlayer
	}



	// Chamado pelo Game Manager para exibir um vídeo da lista:

	public void PlayVideo (int _id) 
	{
/*

		videoPlayer.clip = videos[_id];		// Pega o arquivo de vídeo na lista (conforme _id)

		videoPlayer.Play ();				// Roda o vídeo

		videoPlayer.enabled = true;			// Volta a activar o VideoPlayer

*/
	}


}
