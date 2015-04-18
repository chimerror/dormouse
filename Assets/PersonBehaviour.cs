using UnityEngine;
using System.Collections;

public class PersonBehaviour : MonoBehaviour
{
    private GameObject _goal;

    public bool Alive = true;
    public float Hunger = 0.75f;
    public float HungerRate = -0.01f;
    public float Thirst = 0.75f;
    public float ThirstRate = -0.05f;
    public float Sanity = 0.75f;
    public float SanityRate = -0.07f;
    public float Energy = 1.0f;
    public float EnergyRate = -0.01f;
    public float Health = 1.0f;
    public float HealthRate = -0.1f;

    void Start()
    {
    }

    void FixedUpdate()
    {
        if (!Alive)
        {
            return;
        }

        float delta = Time.fixedDeltaTime;
        Hunger = AdjustStatistic(delta, HungerRate, Hunger);
        Thirst = AdjustStatistic(delta, ThirstRate, Thirst);

        if (Hunger <= 0.0f || Thirst <= 0.0f)
        {
            Health = AdjustStatistic(delta, HealthRate, Health);
        }

        if (Health <= 0.0f)
        {
            GetComponent<SpriteRenderer>().color = Color.black;
            Alive = false;
            Debug.LogFormat("Person {0} died!", name);
        }
    }

    private float AdjustStatistic(float delta, float rate, float currentValue)
    {
        float nextValue = currentValue + (rate * delta);
        if (nextValue < 0.0f)
        {
            nextValue = 0.0f;
        }
        else if (nextValue > 1.0f)
        {
            nextValue = 1.0f;
        }

        return nextValue;
    }
}
