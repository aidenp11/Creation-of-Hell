using UnityEditor;
using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.Diagnostics;

public class Aim : MonoBehaviour
{
    [SerializeField] Transform armPivotTransform;
    [SerializeField] Camera cam;
    [SerializeField] SpriteRenderer spriteToFlip;
    
    void Update()
    {
        Vector3 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);

        Vector3 aimDirection = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        armPivotTransform.eulerAngles = new Vector3(0, 0, angle);

        if (angle > 90 || angle < -90)
        {
            spriteToFlip.flipX = true;
        }
        else
        {
            spriteToFlip.flipX= false;
        }
    }
}
