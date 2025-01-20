using UnityEngine;

public class SwitchCamera : MonoBehaviour
{
    public GameObject NormalCMCamera;
    public GameObject NormalCrosshair;
    public GameObject AimCMCamera;
    public GameObject AimCrosshair;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButton("Fire2") && Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            NormalCMCamera.SetActive(false);
            NormalCrosshair.SetActive(false);
            AimCMCamera.SetActive(true);
            AimCrosshair.SetActive(true);
        }
        else if(Input.GetButton("Fire2"))
        {
            AimCMCamera.SetActive(true);
            AimCrosshair.SetActive(true);
            NormalCMCamera.SetActive(false);
            NormalCrosshair.SetActive(false);
        }
        else
        {
            NormalCMCamera.SetActive(true);
            NormalCrosshair.SetActive(true);
            AimCMCamera.SetActive(false);
            AimCrosshair.SetActive(false);
        }
    }
}
