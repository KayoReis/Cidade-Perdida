using System;
using UnityEngine;
using UnityEngine.Events;
public class GatilhoTutorial: MonoBehaviour
{

    [SerializeField] private UnityEvent OnEnter;
    [SerializeField] private UnityEvent OnExit;

    private bool FisrtTime = true;
    void OnTriggerEnter(Collider other)
    {
        if (FisrtTime){
        if(other.gameObject.tag == "Player")
        {
            Debug.Log("Entrou");
            OnEnter.Invoke();
            FisrtTime = false;
        }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            OnExit.Invoke();
        }
    }
}
