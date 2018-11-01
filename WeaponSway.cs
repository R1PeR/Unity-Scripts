using UnityEngine;
using Custom;
public class WeaponSway : MonoBehaviour {
    private Vector3 startPos;
    private Vector3 desiredPos = new Vector3(0, 0, 0);
    [SerializeField] private Recoil recoil;
    [SerializeField] private float maxSway = 1.0f;
    // Use this for initialization
    // Update is called once per frame
    void Start()
    {
        startPos = gameObject.transform.localPosition;
        recoil.NewTarget(startPos, desiredPos);
    }
    void Update ()
    {
        desiredPos = new Vector3(0, 0, 0);
        desiredPos.y = -Input.GetAxis("Mouse Y")*0.01f;
        desiredPos.x = -Input.GetAxis("Mouse X") * 0.01f - Input.GetAxis("Horizontal") * 0.1f ;
        desiredPos.z = -Input.GetAxis("Vertical") * 0.05f;
        desiredPos.x = Mathf.Clamp(desiredPos.x, -maxSway, maxSway);
        desiredPos.y = Mathf.Clamp(desiredPos.y, -maxSway, maxSway);
        desiredPos.z = Mathf.Clamp(desiredPos.z, -maxSway, maxSway);
        if (desiredPos != Vector3.zero)
        {
            recoil.NewTarget(startPos, desiredPos);
        }
        recoil.RunRoutine();
    }
}
