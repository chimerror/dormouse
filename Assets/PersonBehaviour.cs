using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PersonBehaviour : MonoBehaviour
{
    private Stack<DesireBehaviour> _desireStack = new Stack<DesireBehaviour>();
    private Rigidbody2D _rigidBody;

    public DesireBehaviour CurrentDesire = null;
    public GameObject Goal = null;
    public bool MoveToGoal = false;
    public bool Alive = true;
    public float Health = 1.0f;
    public float Velocity = 0.0f;
    public float TEMPVelocity = 0.0f;
    public Vector2 TEMPGoalPosition;
    public Vector2 TEMPRealGoalPosition;
    public Vector2 TEMPCalculatedGoalPosition;
    public float Acceleration = 4.0f;
    public float AngularVelocity = 1.25f; // rad/s^2
    public float AngularAcceleration = 1.25f; // rad/s^2
    public float TEMPAngularVelocity = 0.0f;
    public float Stubborness = 0.03f;

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (!Alive)
        {
            return;
        }

        float delta = Time.fixedDeltaTime;

        if (CurrentDesire != null && CurrentDesire.DesireValue >= CurrentDesire.DesireMax)
        {
            CurrentDesire = _desireStack.Count > 0 ? _desireStack.Pop() : null;
            Goal = null;
            MoveToGoal = false;
        }

        DesireBehaviour maxDesire = GetComponents<DesireBehaviour>()
            .Where(d => d.Utility > 0.0f)
            .OrderByDescending(d => d.Utility)
            .FirstOrDefault();
        if (maxDesire == null)
        {
            if (CurrentDesire != null)
            {
                Debug.LogWarning("Was holding on to an old desire.");
                CurrentDesire = null;
            }
        }
        else if (CurrentDesire == null)
        {
            CurrentDesire = maxDesire;
        }
        else if (maxDesire.Utility >= CurrentDesire.Utility + Stubborness)
        {
            _desireStack.Push(CurrentDesire);
            CurrentDesire = maxDesire;
        }

        if (CurrentDesire != null)
        {
            GameObject nextGoal = GameObject.FindGameObjectWithTag(CurrentDesire.DesireTag);
            if (nextGoal != Goal)
            {
                Goal = nextGoal;
                MoveToGoal = true;
            }
        }

        if (Health <= 0.0f)
        {
            GetComponent<SpriteRenderer>().color = Color.black;
            Alive = false;
            Debug.LogFormat("Person {0} died!", name);
        }

        TEMPAngularVelocity = _rigidBody.angularVelocity;

        if (Goal != null && MoveToGoal && Velocity > 0.0f)
        {
            Vector2 currentPosition = transform.position;
            Vector2 desiredPosition = Goal.GetComponent<Collider2D>().bounds.ClosestPoint(currentPosition);
            TEMPRealGoalPosition = desiredPosition;
            Vector2 localDesiredPosition = _rigidBody.GetPoint(desiredPosition);
            TEMPGoalPosition = localDesiredPosition;
            TEMPCalculatedGoalPosition = _rigidBody.GetRelativePoint(localDesiredPosition);

            float stoppingDistance = Mathf.Pow(Velocity, 2.0f) / Acceleration;
            Vector2 desiredVelocity = Vector2.up * Velocity * Mathf.Sign(localDesiredPosition.y);
            desiredVelocity *= Mathf.Min(1.0f, Mathf.Abs(localDesiredPosition.y) / stoppingDistance);

            Vector2 currentVelocity = _rigidBody.velocity;
            Vector2 localVelocity = _rigidBody.GetVector(currentVelocity);
            TEMPVelocity = localVelocity.y;
            float velocityDelta = desiredVelocity.y - localVelocity.y;
            float currentSpeed = localVelocity.magnitude;
            if (currentSpeed + velocityDelta < 0.0f)
            {
                velocityDelta = -currentSpeed;
            }

            float desiredAcceleration = velocityDelta / delta;
            if (Mathf.Abs(desiredAcceleration) > Acceleration)
            {
                desiredAcceleration = Mathf.Sign(desiredAcceleration) * Acceleration;
            }

            Vector2 localForce = Vector2.up * _rigidBody.mass * desiredAcceleration;
            Vector2 force = _rigidBody.GetRelativeVector(localForce);
            _rigidBody.AddForce(force, ForceMode2D.Force);

            Vector2 normalizedLocalDesiredPosition = localDesiredPosition.normalized;
            float directionToTurn = Mathf.Sign(Vector3.Cross(Vector2.up, normalizedLocalDesiredPosition).z);
            float distanceToTurn = Vector2.Angle(Vector2.up, normalizedLocalDesiredPosition) * Mathf.Deg2Rad * directionToTurn;
            float angularStoppingDistance = Mathf.Pow(AngularVelocity, 2.0f) / AngularAcceleration;
            float desiredAngularVelocity = Mathf.Sign(distanceToTurn) * AngularVelocity;
            desiredAngularVelocity *= Mathf.Min(1.0f, Mathf.Abs(distanceToTurn) / angularStoppingDistance);

            float currentAngularVelocity = _rigidBody.angularVelocity * Mathf.Deg2Rad;
            float desiredAngularAcceleration = (desiredAngularVelocity - currentAngularVelocity) / delta;
            if (Mathf.Abs(desiredAngularAcceleration) > AngularAcceleration)
            {
                desiredAngularAcceleration = Mathf.Sign(desiredAngularAcceleration) * AngularAcceleration;
            }
            float torque = desiredAngularAcceleration * _rigidBody.mass * 0.5f;
            _rigidBody.AddTorque(torque, ForceMode2D.Force);
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject == Goal)
        {
            MoveToGoal = false;
        }
    }
}
