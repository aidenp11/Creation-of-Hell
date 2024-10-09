using UnityEditor;
using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.Diagnostics;

public class Aim : MonoBehaviour
{
    [SerializeField] Transform armPivotTransform;
    [SerializeField] Camera cam;
    [SerializeField] SpriteRenderer spriteToFlip;
    private SpriteRenderer gunToFlip;
    public float angle;
    
    void Update()
    {
        Vector3 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);

        Vector3 aimDirection = (mousePosition - transform.position).normalized;
        angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        armPivotTransform.eulerAngles = new Vector3(0, 0, angle);
        gunToFlip = GetComponent<Inventory>().activeWeapon.GetComponent<SpriteRenderer>();


        if (angle > 90 || angle < -90)
        {
            spriteToFlip.flipX = true;
            gunToFlip.flipX = true;
        }
        else
        {
            spriteToFlip.flipX= false;
            gunToFlip.flipX = false;
        }
    }
}
