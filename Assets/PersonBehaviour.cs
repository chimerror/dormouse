using UnityEngine;
using System.Collections;

public class PersonBehaviour : MonoBehaviour
{
    public GameObject _goal;
    public bool MoveToGoal = false;
    public bool Alive = true;
    public bool Eating = false;
    public float Hunger = 0.75f;
    public float HungerRate = -0.01f;
    public bool Drinking = false;
    public float Thirst = 0.75f;
    public float ThirstRate = -0.05f;
    public float Sanity = 0.75f;
    public float SanityRate = -0.07f;
    public float Energy = 1.0f;
    public float EnergyRate = -0.01f;
    public float Health = 1.0f;
    public float HealthRate = -0.1f;
    public float Velocity = 0.0f;

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
        Hunger = AdjustStatistic(delta, Eating ? 0.1f : HungerRate, Hunger);
        Thirst = AdjustStatistic(delta, Drinking ? 0.1f : ThirstRate, Thirst);

        if (_goal == null && !Drinking && Thirst <= 0.4f)
        {
            _goal = GameObject.FindGameObjectWithTag("Water");
            MoveToGoal = true;
        }
        else if (Drinking && Thirst >= 1.0f)
        {
            Drinking = false;
            _goal = null;
            MoveToGoal = false;
        }

        if (_goal == null && !Eating && Hunger <= 0.4f)
        {
            _goal = GameObject.FindGameObjectWithTag("Food");
            MoveToGoal = true;
        }
        else if (Eating && Hunger >= 1.0f)
        {
            Eating = false;
            _goal = null;
            MoveToGoal = false;
        }

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

        if (_goal != null && MoveToGoal)
        {
            transform.position =
                Vector3.MoveTowards(transform.position, _goal.transform.position, Velocity * delta);
        }
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject == _goal)
        {
            MoveToGoal = false;

            if (collider.gameObject.tag.Equals("Water"))
            {
                Drinking = true;
            }

            if (collider.gameObject.tag.Equals("Food"))
            {
                Debug.LogFormat("Started eating at: {0}", Time.fixedTime);
                Eating = true;
            }
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
