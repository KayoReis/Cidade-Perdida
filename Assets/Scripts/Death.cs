using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;


public class Death : MonoBehaviour
{
    [SerializeField] private GameObject DeathOverlay;

    CanvasGroup DeathCanvas;
    private float FadeSpeed = 1.5f;

    void Start()
    {
        DeathCanvas = DeathOverlay.GetComponent<CanvasGroup>();

        if(DeathCanvas != null)
        {
            DeathOverlay.SetActive(false);
        }
    }

    public void Dead()
    {
        StartCoroutine(DeadSequencie());
    }

    IEnumerator DeadSequencie()
    {
        DeathOverlay.SetActive(true);

        Time.timeScale = 0;

        float alpha = 0;

        while (alpha < 1)
        {
            alpha += Time.unscaledDeltaTime * FadeSpeed;
            DeathCanvas.alpha = alpha;
            yield return null;

        }

        yield return new WaitForSecondsRealtime(1f);

        Time.timeScale = 1;

        SceneManager.LoadScene("GameOver");

    }

}
