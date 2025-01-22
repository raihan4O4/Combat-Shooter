using Mono.Cecil.Cil;
using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{

    [Header("Gun data")]
    public Camera Maincamera;
    public float shootRange = 100f;
    public float enemyDamage = 10f;
    public PlayerController playerController;
    public Animator playerAnimator;
    private float giveDamage = 2f;

    [Header("Gun Ammunition and shooting")]
    public float fireCharge = 15f;
    public float reloadingTime = 1.3f;
    private float MaxAmmo = 20;
    private float shootInterval = 0f;
    private int mag = 15;
    private float presentAmmo;
    private bool isReloading = false;

    [Header("Particle Effects")]
    public ParticleSystem muzzleSpark;
    public GameObject woodDestroyEffect;

    void Awake()
    {
        presentAmmo = MaxAmmo;
    }

    void Update()
    {

        if (isReloading)
            return;

        if(presentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if(Input.GetButton("Fire1") && Time.time >= shootInterval)
        {
            playerAnimator.SetBool("Fire", true);
            playerAnimator.SetBool("Idle", false);

            shootInterval = Time.time + 1f / fireCharge;
            Shoot();
        }
        else if(Input.GetButton("Fire1") && Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            playerAnimator.SetBool("FireWalk", true);
            playerAnimator.SetBool("Walk", true);
            playerAnimator.SetBool("Idle", false);
            playerAnimator.SetBool("IdleAim", true);
            playerAnimator.SetBool("Reload", false);
        }
        else if (Input.GetButton("Fire1") && Input.GetButton("Fire2"))
        {
            playerAnimator.SetBool("FireWalk", true);
            playerAnimator.SetBool("Idle", false);
        }
        else
        {
            playerAnimator.SetBool("FireWalk", false);
            playerAnimator.SetBool("Fire", false);
            playerAnimator.SetBool("Idle", true);
        }
    }

    public void Shoot()
    {
        if(mag == 0)
        {
            //
        }
        presentAmmo--;

        if(presentAmmo <= 0)
        {
            mag--;
        }
            muzzleSpark.Play();
        if (Physics.Raycast(Maincamera.transform.position, Maincamera.transform.forward, out RaycastHit hit, shootRange))
        {
            Debug.Log(hit.transform.name);
            ObjectDamage objectDamage = hit.transform.GetComponent<ObjectDamage>();
            if(objectDamage != null)
            {
                objectDamage.ObjectHitDamage(giveDamage);
                GameObject destroyWoodBox = Instantiate(woodDestroyEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(destroyWoodBox, 1f);
            }
        }
    }

    IEnumerator Reload()
    {
        playerController.playerSpeed = 0;
        playerController.playerSprint = 0;
        isReloading = true;
        Debug.Log("Reloading...");
        playerAnimator.SetBool("Reload", true);
        yield return new WaitForSeconds(reloadingTime);
        playerAnimator.SetBool("Reload", false);
        presentAmmo = MaxAmmo;
        playerController.playerSpeed = 2f;
        playerController.playerSprint = 3f;
        isReloading = false;

    }
}
