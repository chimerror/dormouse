using UnityEngine;
using System.Collections;

public class DesireBehaviour : MonoBehaviour
{
    private PersonBehaviour _person;

    public string DesireName;
    public string DesireTag;
    public float DesireValue = 0.75f;
    public float DesireMax = 1.0f;
    public float DesireSeekThreshold = 0.4f;
    public float DesireMin = 0.0f;
    public float DesireRate = -0.1f;
    public float SatisfactionRate = 0.1f;
    public float DesireWeight = 1.0f;
    public bool Satisfying = false;
    public float HealthPenalty = -0.1f;

    public float Utility
    {
        get
        {
            if (DesireValue > DesireSeekThreshold)
            {
                return 0.0f;
            }

            return DesireWeight * (DesireSeekThreshold - DesireValue) * Mathf.Abs(HealthPenalty) * Mathf.Abs(DesireRate);
        }
    }

    void Start()
    {
        _person = GetComponent<PersonBehaviour>();
    }

    void FixedUpdate()
    {
        if (!_person.Alive)
        {
            return;
        }

        float delta = Time.fixedDeltaTime;
        float updateRate = Satisfying ? SatisfactionRate : DesireRate;
        DesireValue += updateRate * delta;

        if (DesireValue <= DesireMin)
        {
            DesireValue = DesireMin;
            _person.Health += HealthPenalty * delta;
        }
        else if (DesireValue > DesireMax)
        {
            DesireValue = DesireMax;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(DesireTag))
        {
            Satisfying = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(DesireTag))
        {
            Satisfying = false;
        }
    }
}
