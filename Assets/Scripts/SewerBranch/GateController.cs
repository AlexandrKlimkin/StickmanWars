using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateController : MonoBehaviour
{
    [SerializeField] private GameObject _gate;
    [SerializeField] private GameObject _flootButtun;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _gate.SetActive(false);
            _flootButtun.SetActive(false);
        }
    }

    private void OnDisable()
    {
        Debug.Log("floor buttun make sound! Click!");
    }

}

