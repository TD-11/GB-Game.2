// Este exemplo invoca uma corrotina e continua executando a função em paralelo. 

using UnityEngine; 
using System.Collections; 

public class ExampleClass : MonoBehaviour 
{ 

    private IEnumerator coroutine; 

    void Start() 
    { 
        print("Iniciando " + Time.time ); 

        coroutine = WaitAndPrint(1.0f); 
        StartCoroutine(coroutine); 

        print("Antes de WaitAndPrint terminar " + Time.time ); 
    } 
    private IEnumerator WaitAndPrint(float waitTime)
    {
        while (Time.time <= 5f)
        {
            yield return new WaitForSeconds(waitTime);
            print("WaitAndPrint " + Time.time);
        }
    }
}