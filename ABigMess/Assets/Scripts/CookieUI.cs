using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookieUI : MonoBehaviour
{
    [SerializeField]
    private GameObject goldCookie;
    [SerializeField]
    private GameObject silverCookie;
    [SerializeField]
    private GameObject bronzeCookie;

    [SerializeField]
    private Animator anim;

    // 0 no cookie
    // 1 bronze
    // 2 silver
    // 3 gold
    public void SwitchCookie(int currentCookie)
    {
        anim.SetTrigger("Medal");
        StartCoroutine(LaunchCookieAnim(currentCookie));
    }

    private IEnumerator LaunchCookieAnim(int currentCookie)
    {
        yield return new WaitForSeconds(1.3f);
        if (currentCookie == 0)
        {
            //no cookie
            silverCookie.SetActive(false);
            bronzeCookie.SetActive(false);
            goldCookie.SetActive(false);

        }
        else if (currentCookie == 1)
        {
            //bronze
            silverCookie.SetActive(false);
            bronzeCookie.SetActive(true);
            goldCookie.SetActive(false);
        }
        else if (currentCookie == 2)
        {
            //silver
            silverCookie.SetActive(true);
            bronzeCookie.SetActive(false);
            goldCookie.SetActive(false);
        }
        else if (currentCookie == 3)
        {
            //gold
            silverCookie.SetActive(false);
            bronzeCookie.SetActive(false);
            goldCookie.SetActive(true);
        }
    }
}
