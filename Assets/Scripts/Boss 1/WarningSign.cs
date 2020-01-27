using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningSign : MonoBehaviour
{

    private void Update()
    {
        StartCoroutine(FlickerRoutine());
    }


    IEnumerator FlickerRoutine()
    {
        int a = 10;
        while (a > 0)
        {
            yield return new WaitForSeconds(0.3f);
            gameObject.SetActive(false);
            yield return new WaitForSeconds(0.3f);
            gameObject.SetActive(true);
            a--;
        }
    }
}
