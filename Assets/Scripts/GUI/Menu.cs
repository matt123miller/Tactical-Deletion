using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

    private Animator animator;
    private CanvasGroup cGroup;

    public bool IsOpen
    {
        get { return animator.GetBool("IsOpen"); }
        set { animator.SetBool("IsOpen", value); }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        cGroup = GetComponent<CanvasGroup>();

        var rect = GetComponent<RectTransform>();
        rect.offsetMax = rect.offsetMin = new Vector2(0, 0);
    }

    // Update is called once per frame
    public void Update()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Open"))
            cGroup.blocksRaycasts = cGroup.interactable = false;
        else
            cGroup.blocksRaycasts = cGroup.interactable = true;
    }
}
