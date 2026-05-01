using System.Collections;
using UnityEngine;

public class InitialText : MonoBehaviour
{
    CanvasGroup canvasGroup;

    [SerializeField] private float FadeDuration = 0.5f;
    [SerializeField] private float VisibleTime = 0.8f;

    public GameObject CameraTutorial;

    public GameObject MoveTutorial;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null || CameraTutorial == null || MoveTutorial == null)
        {
            Debug.LogError("Objetos não encontrados");
            return;
        }
        CameraTutorial.SetActive(false);
        MoveTutorial.SetActive(false);

        StartCoroutine(TextAnimation());


    }

    IEnumerator TextAnimation()
    {
        canvasGroup.alpha = 0;

        yield return new WaitForSeconds(0.8f);

        CameraTutorial.SetActive(true);

        yield return StartCoroutine(Fade(0, 1));

        yield return new WaitForSeconds(VisibleTime);

        CameraTutorial.SetActive(false);
 MoveTutorial.SetActive(true);
        yield return StartCoroutine(Fade(1, 0));

        

        yield return new WaitForSeconds(0.8f);

        Destroy(CameraTutorial);
        Destroy(gameObject);
        Destroy(MoveTutorial);
    }

    IEnumerator Fade(float start, float end)
    {
        float time = 0;

        while (time < FadeDuration)
        {
            time += Time.deltaTime;

            float progression = time / FadeDuration;

            canvasGroup.alpha = Mathf.Lerp(start, end, progression);

            yield return null;
        }

        canvasGroup.alpha = end;
    }
}
