using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionComponent : MonoBehaviour
{
    public bool Condition1()
    {
        Debug.Log("Condition1");
        return true;
    }

    public bool Condition2()
    {
        Debug.Log("Condition2");
        return false;
    }

    public bool Condition3()
    {
        Debug.Log("Condition3");
        return true;
    }
}
