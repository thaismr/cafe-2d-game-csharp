using UnityEngine;

public class ColliderUnderGlow : MonoBehaviour 
{

	SpriteRenderer halo;


	void Start()
	{

		halo = GetComponent<SpriteRenderer> ();	

	}


	public void Glow(bool isEnabled)		// Recebe um "Boolean"  verdadeiro / falso
	{
		
		halo.enabled = isEnabled;

	}

}
