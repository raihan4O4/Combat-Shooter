using UnityEngine;

public class SwitchCamera : MonoBehaviour
{
    public GameObject NormalCMCamera;
    public GameObject NormalCrosshair;
    public GameObject AimCMCamera;
    public GameObject AimCrosshair;
    public Animator playerAnimator;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButton("Fire2") && Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            playerAnimator.SetBool("Idle",false);
            playerAnimator.SetBool("IdleAim",true);
            playerAnimator.SetBool("AimWalk",true);
            playerAnimator.SetBool("Walk", true);

            NormalCMCamera.SetActive(false);
            NormalCrosshair.SetActive(false);
            AimCMCamera.SetActive(true);
            AimCrosshair.SetActive(true);
        }
        else if(Input.GetButton("Fire2"))
        {
            playerAnimator.SetBool("Idle", false);
            playerAnimator.SetBool("IdleAim", true);
            playerAnimator.SetBool("AimWalk", false);
            playerAnimator.SetBool("Walk", false);

            AimCMCamera.SetActive(true);
            AimCrosshair.SetActive(true);
            NormalCMCamera.SetActive(false);
            NormalCrosshair.SetActive(false);
        }
        else
        {
            playerAnimator.SetBool("Idle", true);
            playerAnimator.SetBool("IdleAim", false);
            playerAnimator.SetBool("AimWalk", false);

            NormalCMCamera.SetActive(true);
            NormalCrosshair.SetActive(true);
            AimCMCamera.SetActive(false);
            AimCrosshair.SetActive(false);
        }
    }
}
