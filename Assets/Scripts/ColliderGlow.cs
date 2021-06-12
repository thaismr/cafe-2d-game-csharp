using UnityEngine;

public class ColliderGlow : MonoBehaviour 
{

	public int id;

    Table table;                // script Table deste objecto, caso exista

	ColliderUnderGlow halo;		// script na componente que posiciona o glow


	void Start()
	{

		halo = GetComponentInChildren<ColliderUnderGlow> ();		// vai buscar o script no child

        table = GetComponent<Table>();                              // vai buscar o script Table, se houver

	}


	public void Glow(bool isEnabled)		// Recebe um "Boolean"  verdadeiro / falso
	{
        // Verifica se é uma mesa vazia:

        if (table)
        {
            if (table.productID == -1)
            {
                isEnabled = false;          // determina Halo desativo
            }
        }


        halo.Glow (isEnabled);				// ativa ou desativa o Halo

	}

}
