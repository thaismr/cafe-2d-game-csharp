using UnityEngine;
using UnityEngine.UI;

public class Machine : MonoBehaviour 
{

	public int machineId;		// id desta máquina

	public GameObject priceTag;		// etiqueta de preço


	// Use this for initialization
	void Start () 
	{
		priceTag = Instantiate (priceTag);

		priceTag.transform.SetParent (transform);

		priceTag.transform.position = transform.position;

		priceTag.GetComponent<Text> ().text = DataBase.item [machineId].sellingPrice.ToString("C");
	}

}
