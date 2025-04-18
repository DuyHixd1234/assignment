using UnityEngine;
using TMPro;

public class WeaponSelector : MonoBehaviour
{
    public PlayerController playerController;
    public TMP_Text messageText;

    public void ChooseGun()
    {
        playerController.EnableGun(true);
        playerController.EnableSword(false);
        if (messageText != null)
            messageText.text = "Ban da chon sung lam vu khi cua minh.";
    }

    public void ChooseSword()
    {
        playerController.EnableSword(true);
        playerController.EnableGun(false);
        if (messageText != null)
            messageText.text = "Ban da chon kiem lam vu khi cua minh.";
    }
}
