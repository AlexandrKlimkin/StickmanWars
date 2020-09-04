using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateController : MonoBehaviour
{
    [SerializeField] private GameObject _Gate;
    [SerializeField] private GameObject _FlootButton;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<CharacterUnit>() != null)
        {
            _Gate.SetActive(false);
            _FlootButton.SetActive(false);
        }
    }

    private void OnDisable()
    {
        Debug.Log(" Floor button make sound! Click!");
    }

}

