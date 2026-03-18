using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed = 30f;
    private float lifetime = 1f; // Czas ¿ycia pocisku w sekundach

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    void Start()
    {
        // Zniszczenie pocisku po up³ywie czasu lifetime
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }
}
