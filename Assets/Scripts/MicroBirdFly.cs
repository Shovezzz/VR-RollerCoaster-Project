using UnityEngine;
using System.Collections;

public class MicroBirdFly : MonoBehaviour
{
    public GameObject birdObject;
    public Vector3 spawnOffset = new Vector3(-2f, 1.5f, 0f);
    public float flightSpeed = 15f;
    public float flightDuration = 2f; // Теперь управляем временем, а не расстоянием

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(FlyRoutine(other.transform));
            // Чтобы триггер сработал лишь раз, просто выключаем его
            GetComponent<Collider>().enabled = false;
        }
    }

    IEnumerator FlyRoutine(Transform player)
    {
        // Появление птицы
        birdObject.transform.position = player.position + (player.rotation * spawnOffset);
        birdObject.transform.rotation = player.rotation;
        birdObject.SetActive(true);

        // Полет
        float time = 0;
        while (time < flightDuration)
        {
            birdObject.transform.Translate(Vector3.forward * flightSpeed * Time.deltaTime);
            time += Time.deltaTime;
            yield return null;
        }

        // Исчезновение
        birdObject.SetActive(false);
    }
}