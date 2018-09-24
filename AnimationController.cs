using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class AnimationController : MonoBehaviour {
    // Use this for initialization
    public bool isMoving = false;

    private string anim;
    private float angleBetweenCameraAndObject;
    private bool flip;
    private float currentAnimationTime = 0.0f;
    private Animator animator;
    private NavMeshAgent agent;
    private SpriteRenderer spriteRenderer;
    // Update is called once per frame
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }
    void Update() {
        if (agent.velocity.Equals(Vector3.zero)){
            SetMoving(false);
        }else{
            SetMoving(true);
        }
        anim = AngleToString(SubtractAngles(GetAngle(Camera.main.transform, gameObject.transform), -gameObject.transform.eulerAngles.y));
        SwitchAnimations(anim, flip);
    }
    float GetAngle(Transform objectOne, Transform objectTwo) {
        float angle = Mathf.Atan2(objectTwo.position.z - objectOne.position.z, objectTwo.position.x - objectOne.position.x) * 180 / Mathf.PI;
        if (angle < 0)
        {
            angle = angle + 360;
        }
        return angle;
    }
    float SubtractAngles(float angleOne, float angleTwo) {
        float angle = angleOne - angleTwo;
        if (angle < 0)
        {
            angle = angle + 360;
        }
        return angle;
    }
    float AddAngles(float angleOne, float angleTwo)
    {
        float angle = angleOne + angleTwo;
        if (angle > 360)
        {
            angle = angle - 360;
        }
        return angle;
    }
    string AngleToString(float angle) {
        int side = (int)AddAngles(angle, 22.5f) / 45;
        switch (side)
        {
            case 4:
                flip = true;
                return "Left";
            case 5:
                flip = true;
                return "Front-Left";
            case 6:
                flip = true;
                return "Front";
            case 7:
                flip = false;
                return "Front-Left";
            case 0:
                flip = false;
                return "Left";
            case 1:
                flip = false;
                return "Back-Left";
            case 2:
                flip = true;
                return "Back";
            case 3:
                flip = true;
                return "Back-Left";

        }
        return "Error";

    }
    void SwitchAnimations(string angleName, bool flip) {
        currentAnimationTime = (animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1.0f);
        if (isMoving) {
            animator.speed = 0.2f;
        }
        else{
            currentAnimationTime = 0.0f;
            animator.speed = 0.0f;
        }
        spriteRenderer.flipX = flip;
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(angleName))
        {
            animator.Play(angleName,0);
        }
        else
        {
            animator.Play(angleName, 0, currentAnimationTime);
        }
    }
    void SetMoving(bool isMoving) {
        this.isMoving = isMoving;
    }
}
