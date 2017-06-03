using UnityEngine;

public class CamCallToggle : MonoBehaviour
{
    public GameObject firstPerson;
    public GameObject thirdPerson;
    private Camera fpc;
    private Camera tpc;
    private GlobalStateManager gsm;
    [SerializeField] private AudioReverbZone reverbZone;
    private AudioSource[] audios;

    void Awake()
    {
        fpc = firstPerson.GetComponent<Camera>();
        tpc = thirdPerson.GetComponent<Camera>();

        fpc.enabled = false;
        thirdPerson.SetActive(true);

        fpc.tag = "Untagged";
        tpc.tag = "MainCamera";
        

        gsm = gameObject.GetComponentInChildren<GlobalStateManager>();
        audios = gsm.gameObject.GetComponents<AudioSource>();
        reverbZone = gsm.gameObject.GetComponent<AudioReverbZone>();
        reverbZone.enabled = false;
    }

    public void ToggleCall()
    {
        fpc.enabled = !fpc.enabled;

        if (fpc.enabled)
        {
            fpc.tag = "MainCamera";
            thirdPerson.SetActive(false);

            reverbZone.enabled = true;
            foreach (AudioSource var in audios)
            {
                var.volume /= 2;
            }
        }
        else
        {
            tpc.tag = "MainCamera";
            thirdPerson.SetActive(true);

            reverbZone.enabled = false;
            foreach (AudioSource var in audios)
            {
                var.volume *= 2;
            }
        }

    }
}
