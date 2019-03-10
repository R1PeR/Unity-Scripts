using System.Collections;
using UnityEngine;

public class LeverWindlassScript : MonoBehaviour
{
    public GameObject hand;
    public GameObject objectToWorkOn;
    //public bool workOnce = false;
    public int whichStateToChange = 0;
    //Changes particular handle in doors, if there is one handle then 0 is main handle.
    private bool isUp = true;
    public bool useOnce = false;
    public bool justDestroyObject = false;
    public string AnimOne;
    public string AnimTwo;
    private DoorScript_new otherScript;
    private Animator animator;
    private Collider collider;
    // Update is called once per frame
    void Start()
    {
        if (!justDestroyObject)
        {
            otherScript = objectToWorkOn.GetComponent<DoorScript_new>();
        }
        animator = transform.parent.GetComponent<Animator>();
        collider = this.GetComponent<Collider>();
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == hand.name)
        {
            if (isUp)
            {
                StartCoroutine(AnimOnePlay());
                if (justDestroyObject)
                {
                    Destroy(objectToWorkOn);
                }
                else
                {
                    otherScript.ChangeDoorState(whichStateToChange, isUp);
                }
            }
            else if (!isUp && !useOnce)
            {
                StartCoroutine(AnimTwoPlay());
                if (justDestroyObject)
                {
                    Destroy(objectToWorkOn);
                }
                else
                {
                    otherScript.ChangeDoorState(whichStateToChange, isUp);
                }
            }
        }
    }
    public IEnumerator AnimOnePlay()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName(AnimOne))
        {
            animator.Play(AnimOne);
            ChangeCollidersEnabled(false);
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
            ChangeCollidersEnabled(true);
            isUp = false;
        }
    }
    public IEnumerator AnimTwoPlay()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName(AnimTwo))
        {
            animator.Play(AnimTwo);
            ChangeCollidersEnabled(false);
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
            ChangeCollidersEnabled(true);
            isUp = true;
        }
    }
    public void ChangeCollidersEnabled(bool isEnabled)
    {
        collider.enabled = isEnabled;
    }
}