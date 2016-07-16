using UnityEngine;
using System.Collections;
using System.Linq;

[IntegrationTest.DynamicTest("TownIntegrationTests")]
[IntegrationTest.Timeout(60)]
public class LarryDrinksAndEatsTest : MonoBehaviour
{
    private GameObject _target;
    private DesireBehaviour _targetThirst;
    private DesireBehaviour _targetHunger;
    private bool _hasDrank = false;
    private bool _hasEaten = false;

    void Start()
    {
        _target = GameObject.FindGameObjectsWithTag("Person").First(p => p.name == "Larry");
        var desires = _target.GetComponents<DesireBehaviour>();
        _targetThirst = desires.First(d => d.DesireName.Equals("Thirst"));
        _targetHunger = desires.First(d => d.DesireName.Equals("Hunger"));
    }

    void Update()
    {
        if (_targetThirst.Satisfying)
        {
            _hasDrank = true;
        }

        if (_targetHunger.Satisfying)
        {
            _hasEaten = true;
        }

        if (_hasDrank && _hasEaten)
        {
            IntegrationTest.Pass();
        }
    }
}
