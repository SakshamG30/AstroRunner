using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class AstroShoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    private Transform gunPoint; //Where the bullet instantiates
    public Transform arm;
    public float bulletSpeed;
    public LineRenderer trajectoryLine;
    public LayerMask collisionMask;
    public float maxRaycastDistance;
    private float originalArmAngle;
    private bool trajectoryVisible = false;
    private AstroScript astro;
    private bool facingRight, isGravityInverted;
    public int bulletCount;
    public TextMesh playerText;
    public Transform shield;
    public bool isShieldActive = false;
    public int bulletLayer = 8;
    public int swordLayer = 11;
    public BulletManager bulletManager;
    // Start is called before the first frame update
    void Start()
    {
        arm = transform.Find("ArmPivot");
        gunPoint = arm.transform.Find("GunPoint");
        shield = transform.Find("Shield");
        trajectoryLine.enabled = trajectoryVisible;
        originalArmAngle = arm.eulerAngles.z;
        astro = GetComponent<AstroScript>();
        facingRight = astro.facingRight;
        isGravityInverted = astro.isGravityInverted;
        shield.gameObject.SetActive(false);
        bulletManager.UpdateBulletCount(bulletCount);
    }

    // Update is called once per frame
    void Update()
    {
        facingRight = astro.facingRight;
        isGravityInverted = astro.isGravityInverted;
        arm = transform.Find("ArmPivot");
        gunPoint = arm.transform.Find("GunPoint");
        AimAtMouse();
        ShowTrajectory();

        if (Input.GetMouseButton(1))
        {
            ActivateShield(true);
        }
        else
        {
            ActivateShield(false);
        }

    }

    private void AimAtMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - transform.position).normalized;

        //To get the angle of the player towards the mouse
        float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) + 90f;

        /*if (!facingRight)
        {
            angle += 180f; // Adjust the angle by 180 degrees when flipped
        }

        float clampedAngle = Mathf.Clamp(angle, originalArmAngle - 88f, originalArmAngle + 200f);*/

        gunPoint.rotation = Quaternion.Euler(new Vector3(0, 0 , angle));

        arm.rotation = Quaternion.Euler(new Vector3(0, 0 , angle));

        if (Input.GetKeyDown(KeyCode.V))
        {
            trajectoryVisible = !trajectoryVisible;
        }
        trajectoryLine.enabled = trajectoryVisible;

        if (Input.GetMouseButtonDown(0))
        {
            if (isShieldActive)
            {
                StartCoroutine(ShowTextForSecond("Can't Shoot while shield is active!"));
            }
            else {
                if (bulletCount > 0)
                {
                    RemoveBullet();
                    Shoot(direction);
                }
                else
                {
                    if (this.isActiveAndEnabled)
                    {
                        StartCoroutine(ShowTextForSecond("Bullets are Empty!"));
                    }
                }
            }
        }
        
        
    }

    private void ActivateShield(bool activate)
    {
        isShieldActive = activate;
        shield.gameObject.SetActive(activate);
    }

    private void ShowTrajectory()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - gunPoint.position).normalized;

        RaycastHit2D hitInfo = Physics2D.Raycast(gunPoint.position, direction, maxRaycastDistance, collisionMask);

        /*if (hitInfo.collider)
        {
            trajectoryLine.SetPosition(0, gunPoint.position);
            trajectoryLine.SetPosition(1, hitInfo.point);
        }
        else
        {
            trajectoryLine.SetPosition(0, gunPoint.position);
            trajectoryLine.SetPosition(1, gunPoint.position + (Vector3)direction * maxRaycastDistance);
        }*/
    }

    private void Shoot(Vector2 direction)
    {
        GameObject bullet = Instantiate(bulletPrefab, gunPoint.position, gunPoint.rotation);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.shooter = this.gameObject;
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        bulletRb.velocity = direction * bulletSpeed;
    }
    public IEnumerator ShowTextForSecond(string text)
    {
        playerText.text = text;
        yield return new WaitForSeconds(1);
        playerText.text = "";
    }

    public void AddBullet()
    {
        bulletCount++;
        bulletManager.UpdateBulletCount(bulletCount);
    }

    public void RemoveBullet()
    {
        bulletCount--;
        bulletManager.UpdateBulletCount(bulletCount);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isShieldActive)
        {
            int layer = other.gameObject.layer;

            // Check if the collider's layer is the bullet layer
            if (layer == bulletLayer)
            {
                AddBullet();
                if (this.isActiveAndEnabled)
                {
                    StartCoroutine(ShowTextForSecond("Bullet Absorbed!"));
                }
            }
            if (layer == swordLayer)
            {
                StartCoroutine(ShowTextForSecond("Nice Block!"));
                AddBullet();
            }
        }
    }
}
