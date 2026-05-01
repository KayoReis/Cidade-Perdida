using System.Collections;
using UnityEngine;

public class SimpleTutorial : MonoBehaviour
{

    public GameObject Text;
    private void Awake()
    {
        Text.SetActive(false);
    }
    public void Show()
    {
        StartCoroutine(Time());
    }

    public void Hide()
    {
        Text.SetActive(false);
    }

    IEnumerator Time()
    {
        Text.SetActive(true);


        yield return new WaitForSeconds(1.5f);

        Text.SetActive(false);

    
    }
}
