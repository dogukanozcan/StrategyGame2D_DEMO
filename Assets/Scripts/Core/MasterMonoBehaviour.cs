using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterMonoBehaviour : MonoBehaviour
{
    public Coroutine Delay(Action action, float delay)
    {
        return StartCoroutine(Delay_handler(action, delay));
    }
    private IEnumerator Delay_handler(Action action, float delay) 
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
    }
}

