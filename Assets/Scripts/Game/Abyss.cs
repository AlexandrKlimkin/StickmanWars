using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abyss : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        Destroy(collider.gameObject);
        if ((Layers.Masks.Character | (1 << collider.gameObject.layer)) == Layers.Masks.Character &&
            collider.tag == "Player")
        {
            GameManager.Instance.EndGame();
        }
    }
}
