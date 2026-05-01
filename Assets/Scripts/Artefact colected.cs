using System.Collections;
using UnityEngine;

public class Artefactcolected : MonoBehaviour
{
    [SerializeField] private GameObject WallWithDoor;
    [SerializeField] private GameObject Door;

    [SerializeField] private LayerMask ItemMask;

    [SerializeField] private Transform cam;

    [SerializeField] private GameObject PickUpText;

    [SerializeField] private GameObject FinalObject;

    CanvasGroup canvasGroup;

    private GameObject Artefact;


    void Start()
    {

        canvasGroup = FinalObject.GetComponent<CanvasGroup>();
        Door.SetActive(false);
        Artefact = GameObject.FindGameObjectWithTag("Artefato");
        PickUpText.SetActive(false);
        canvasGroup.alpha = 0;
        FinalObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {


        if (Physics.Raycast(cam.position, cam.forward, out RaycastHit raycasthit, 2f, ItemMask))
        {
            PickUpText.SetActive(true);
        }
        else
        {
            PickUpText.SetActive(false);
        }
    }

    public void Oncollected()
    {
    
            Destroy(Artefact);
            Destroy(WallWithDoor);
            Door.SetActive(true);

            StartCoroutine(Mission());


    

    }
    IEnumerator Mission()
    {
        FinalObject.SetActive(true);
        canvasGroup.alpha = 0;
        yield return new WaitForSeconds(0.8f);

        yield return StartCoroutine(Fade(0, 1));

        yield return new WaitForSeconds(0.8f);

        yield return StartCoroutine(Fade(1, 0));

        yield return new WaitForSeconds(0.8f);

        FinalObject.SetActive(false);


    }

    IEnumerator Fade(float start, float end)
    {
        float time = 0;

        while (time < 0.5)
        {
            time += Time.deltaTime;

            float progression = time / 0.5f;

            canvasGroup.alpha = Mathf.Lerp(start, end, progression);

            yield return null;
        }

        canvasGroup.alpha = end;
    }
}


