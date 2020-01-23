using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    [SerializeField]
    int roomNb = 0;

    [SerializeField]
    List<MeshRenderer> wallsToHide;

    private void Start()
    {
        InitWallsFeedback();
    }

    private void InitWallsFeedback()
    {
        foreach (var item in wallsToHide)
        {
            GameObject wallFeedback = Instantiate(item.gameObject, item.transform.position, Quaternion.identity) as GameObject;
            wallFeedback.transform.parent = item.transform;
            wallFeedback.transform.localScale = new Vector3(wallFeedback.transform.localScale.x, 0.2f, wallFeedback.transform.localScale.z*0.95f);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerManager>().CurrentRoomNb = roomNb;
            CameraManager.Instance.SwitchCamera(roomNb);
            ModifyWallsMaterial(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerManager>().CurrentRoomNb = roomNb;
            CameraManager.Instance.SwitchCamera(roomNb);
            ModifyWallsMaterial(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ModifyWallsMaterial(false);
            other.gameObject.GetComponent<PlayerManager>().CurrentRoomNb = -1;
        }
    }

    private void ModifyWallsMaterial(bool isTransp)
    {
        if(isTransp)
        {
            foreach (var item in wallsToHide)
            {
                for(int i = 0; i <item.materials.Length;i++)
                {
                    SetupMaterial(item.materials[i], "Transparent");
                }
            }
        }
        else
        {
            foreach (var item in wallsToHide)
            {
                for (int i = 0; i < item.materials.Length; i++)
                {
                    SetupMaterial(item.materials[i], "Opaque");
                }
            }
        }
    }

    /// <summary>
    /// Permet de changer le mode du shader standard
    /// </summary>
    /// <param name="material">le material à utiliser</param>
    /// <param name="blendMode"> le mode voulu : Opaque , Transparent</param>
    private static void SetupMaterial(Material material, string blendMode)
    {
        switch (blendMode)
        {
            case "Opaque":
                material.SetFloat("_Mode", 0);
                material.color = new Color(material.color.r, material.color.g, material.color.b,1f);
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = -1;
                break;
            case "Transparent":
                material.SetFloat("_Mode", 3);
                material.color = new Color(material.color.r, material.color.g, material.color.b, 0.01f);
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 3000;
                break;
        }
    }
}

