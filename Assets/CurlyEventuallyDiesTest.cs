using UnityEngine;
using System.Collections;
using System.Linq;

[IntegrationTest.DynamicTest("TownIntegrationTests")]
[IntegrationTest.Timeout(60)]
public class CurlyEventuallyDiesTest : MonoBehaviour
{
    private GameObject _target;
    private PersonBehaviour _targetBehaviour;

    void Start ()
    {
        _target = GameObject.FindGameObjectsWithTag("Person").First(p => p.name == "Curly");
        _targetBehaviour = _target.GetComponent<PersonBehaviour>();
    }

    void Update ()
    {
        if (!_targetBehaviour.Alive)
        {
            IntegrationTest.Pass();
        }
    }
}
