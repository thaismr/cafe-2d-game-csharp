using UnityEngine;

/// <summary>
/// 
/// Controla os movimentos do jogador
/// 
/// </summary>

public class PlayerMovement : MonoBehaviour 
{

	Rigidbody2D rBody;		// rigidbody para controlar os movimentos

	Animator anim;			// animator para controlar as animações do jogador



	// Inicialização do jogador:

	void Start () 
	{

		rBody = GetComponent<Rigidbody2D> ();	// vai buscar a componente RigidBody deste jogador

		anim = GetComponent<Animator> ();		// vai buscar a componente Animator deste jogador

	}



	void FixedUpdate () 
	{
		
		// Se jogador estiver pagando, retorna daqui :
		
		if (GameManager.isPaying) 
		{
			anim.SetBool ("isWalking", false);
			
			return;
		}


		// Senão, pega o Input do jogdor (teclado ou controlador) :

		Vector2 movement = new Vector2 (Input.GetAxisRaw("Horizontal") , Input.GetAxisRaw("Vertical"));		



		if (movement != Vector2.zero)	// se o vector 'movement' for diferente de zero, há Input em alguma direção
		{
			anim.SetBool ("isWalking" , true);		// portanto, 'isWalking' é verdadeiro

			anim.SetFloat("inputX" , movement.x);	// actualiza as variáveis de direcção dentro da animação
			anim.SetFloat("inputY" , movement.y);


			if (movement == Vector2.left)
				GetComponent<SpriteRenderer> ().flipX = true;	// se o movimento for para a esquerda, vira a Sprite do jogador (para esquerda)

			else if (movement == Vector2.right)
				GetComponent<SpriteRenderer> ().flipX = false;	// se o movimento for para a direita, retorna a Sprite ao lado direito
		}

		else
			anim.SetBool("isWalking", false);		// senão, 'isWalking' é falso



		// Move a componente  'RigidBody2d'  do jogador, acresentando à posição actual, o valor do vector  'movement' (/6) :

		rBody.MovePosition(rBody.position + movement/8);

	}

}
