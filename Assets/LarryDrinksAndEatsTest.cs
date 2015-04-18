using UnityEngine;
using System.Collections;
using System.Linq;

[IntegrationTest.DynamicTest("TownIntegrationTests")]
[IntegrationTest.Timeout(60)]
public class LarryDrinksAndEatsTest : MonoBehaviour
{
    private GameObject _target;
    private PersonBehaviour _targetBehaviour;
    private bool _hasDrank = false;
    private bool _hasEaten = false;

    void Start()
    {
        _target = GameObject.FindGameObjectsWithTag("Person").First(p => p.name == "Larry");
        _targetBehaviour = _target.GetComponent<PersonBehaviour>();
    }

    void Update()
    {
        if (_targetBehaviour.Drinking)
        {
            _hasDrank = true;
        }

        if (_targetBehaviour.Eating)
        {
            _hasEaten = true;
        }

        if (_hasDrank && _hasEaten)
        {
            IntegrationTest.Pass();
        }
    }
}
