using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Transform pos;

    private void OnTriggerEnter(Collider other)
    {
        CharacterController p = other.gameObject.GetComponent<CharacterController>();

        if (p != null)
        {
                // Отключаем CharacterController, чтобы избежать проблем с коллизией
                p.enabled = false;

            if (p.CompareTag("Player"))
            {
                print("1234");
                p.gameObject.transform.position = pos.position;
            }
                // Перемещаем персонажа


                // Включаем CharacterController обратно
                p.enabled = true;
        }
    }
}
