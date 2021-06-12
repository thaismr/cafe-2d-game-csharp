using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;


public class GameManager : MonoBehaviour 
{
	
	//	GERENCIA O CICLO DO DIA :

	public const float SECONDS_PER_DAY = 60 * 10;		// duração do dia no jogo (em segundos)		60 * 10  são  10 minutos

	public float secondsPerDay;                         // duração do dia restante (variável calculada)

    float thirdDaySeconds;                              // dois terços do dia (calculado para iniciar segundo nível)

    float halfDaySeconds;                               // metade do dia (calculado para iniciar terceiro nível)



    //	GERENCIA OS NÍVEIS :

    public const float CLIENTS_PER_SEC_LVL1 = 35;          // segundos antes de cada novo cliente, nível de dificuldade 1

    public const float CLIENTS_PER_SEC_LVL2 = 25;          // segundos antes de cada novo cliente, nível de dificuldade 2

    public const float CLIENTS_PER_SEC_LVL3 = 15;          // segundos antes de cada novo cliente, nível de dificuldade 3

    public static float clientsPerSecond = 35;              // segundos antes de cada novo cliente, para o nível corrente



    //	GERENCIA A PONTUAÇÃO :

    public const int SCORE_FACTOR = 150;	// quanto vale cada cliente para a pontuação

	public int clientsServed;				// contador do número de clientes atendidos

	public int highScore;					// pontuação do jogador

	public decimal scoreMoney;		// dinheiro total recebido com as vendas



	//	GERENCIA CLIENTES E PEDIDOS :

	public static int[] tablesList = {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1};

	public static bool isPaying;			// pausa jogador para processar pagamento

	public static decimal paymentChange;	// valor do troco

	public static int paymentOption;		// valor pago pelo cliente

	public static int productId;			// ID do produto a pagar

	public Sprite[] currencySprites;		// imagens das moedas e notas

	public static Table clientTable;        // componente Table que está em colisão



    //	GERENCIA OS VÍDEOS :

    static Video video;			// script que controla os vídeos

	public GameObject videoPlayer;		// objecto que contém o videoplayer



	// GERENCIA O ARQUIVO QUE SALVA O HIGH SCORE :

	private KeyCode KEY_CODE_DELETE = KeyCode.R;			// Premir "R" deleta o arquivo "save.dat" para modificações à estrutura do SaveData.cs

	private const string SAVE_DATA_FILE = "save.dat";		// Nome do arquivo onde salvar os dados

	private string saveDataPath;

	private SaveData saveData;



	// TECLA PARA SAIR DO JOGO :

	private KeyCode KEY_CODE_QUIT = KeyCode.Q;



	//	GERENCIA A INSTANCIA DO GAME MANAGER :

	const string GAME_MANAGER_PATH = "Prefabs/GameManager";		// localização do prefab do GameManager

	static GameManager _instance;								// instância do tipo 'static', acessível por qualquer script



	//	TEXTO PÚBLICO PARA DEBUG :

	public string debugLog = "Texto debug...";



	//	PAUSA DO JOGO :

	public static bool isPaused = true;




	void Update()
	{

		//	GERENCIA O CICLO DO DIA :

		secondsPerDay -= Time.deltaTime;		// retira 1 a cada segundo

        

        if (secondsPerDay < 0)	    // quando o tempo chegar a 0 :

			LevelOver ();           // acaba o nível / dia


        else if (secondsPerDay < halfDaySeconds)            // quando o tempo chegar à metade :

            clientsPerSecond = CLIENTS_PER_SEC_LVL3;        // aumenta o nível


        else if (secondsPerDay < thirdDaySeconds)           // quando o tempo chegar a dois terços :

            clientsPerSecond = CLIENTS_PER_SEC_LVL2;        // aumenta o nível



        // GERENCIA A PONTUAÇÃO :

        highScore = clientsServed * SCORE_FACTOR;		// pontuação conforme clientes atendidos



		// GERENCIA BOTÕES OU TECLADO :

		if (Input.GetKeyDown(KEY_CODE_DELETE))		// Apagar arquivo SaveData
		{
			DeleteSaveData();
		}
		else if (Input.GetKeyDown(KEY_CODE_QUIT))	// Sai do jogo
		{
			#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
			#else
			Application.Quit();
			#endif
		}

	}


	public void LevelOver()		// Acaba o ciclo do dia
	{
		// reset values: 

		isPaying = false;

		secondsPerDay = SECONDS_PER_DAY;

        clientsPerSecond = CLIENTS_PER_SEC_LVL1;

        halfDaySeconds = SECONDS_PER_DAY / 2;       // calcula a metade do dia

        thirdDaySeconds = SECONDS_PER_DAY * 2 / 3;      // calcula um terço do dia

        tablesList = new int[] {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1};



        // stop game :

        isPaused = true;

		Time.timeScale = 0;

		Cursor.lockState = CursorLockMode.None;

		Cursor.visible = true;

		GameObject.Find ("CanvasUI").GetComponentInChildren<UI> ().LevelOver ();

	}





	//	GERENCIA A INSTANCIA DO GAME MANAGER :

	public static GameManager GM
	{
		get
		{
			if (_instance == null)		// somente se ainda não houver nenhuma instância :
				CreateGameManager();	// chama a função para criar a instância

			return _instance;			// retorna a instância
		}
	}

	static void CreateGameManager()
	{
		GameManager gameManagerPrefab = Resources.Load<GameManager>(GAME_MANAGER_PATH);		// vai buscar o prefab do GameManager

		_instance = Instantiate(gameManagerPrefab);		// instancia o prefab do GameManager
	}

	void Start()
	{
		DontDestroyOnLoad(gameObject);		// não deixa destruir o gameObject ao qual este script pertence, ao carregar uma scene

		InitSaveData ();


		video = videoPlayer.GetComponent<Video> ();     // vai buscar o script que controla os vídeos


        // Reseta os valores iniciais :

        secondsPerDay = SECONDS_PER_DAY;

        halfDaySeconds = SECONDS_PER_DAY / 2;       // calcula a metade do dia

        thirdDaySeconds = SECONDS_PER_DAY * 2 / 3;      // calcula dois terços do dia

        clientsPerSecond = CLIENTS_PER_SEC_LVL1;

        isPaying = false;

        clientsServed = 0;

        highScore = 0;

        scoreMoney = 0M;
    }


	public void Restart()
	{
        // Reseta os valores iniciais :

        tablesList = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };

        secondsPerDay = SECONDS_PER_DAY;

        halfDaySeconds = SECONDS_PER_DAY / 2;       // calcula a metade do dia

        thirdDaySeconds = SECONDS_PER_DAY * 2 / 3;      // calcula um terço do dia

        clientsPerSecond = CLIENTS_PER_SEC_LVL1;

        isPaused = true;

        isPaying = false;

		clientsServed = 0;

		highScore = 0;

		scoreMoney = 0M;
	}


	// GERENCIA O ARQUIVO DE SALVAMENTO :

	private void InitSaveData()
	{
		saveDataPath = Application.persistentDataPath + "/" + SAVE_DATA_FILE;

		if (File.Exists(saveDataPath))
			LoadSaveData();
		else
			CreateSaveData();
	}

	private void CreateSaveData()
	{
		saveData = new SaveData();
	}

	private void LoadSaveData()
	{
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		FileStream      fileStream      = File.Open(saveDataPath, FileMode.Open);

		saveData = (SaveData)binaryFormatter.Deserialize(fileStream);

		fileStream.Close();
	}

	private void StoreSaveData()
	{
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		FileStream      fileStream      = File.Create(saveDataPath);

		binaryFormatter.Serialize(fileStream, saveData);

		fileStream.Close();
	}

	private void DeleteSaveData()	// Apaga arquivo de salvamento
	{
		saveDataPath = Application.persistentDataPath + "/" + SAVE_DATA_FILE;

		File.Delete (saveDataPath);
	}






}

