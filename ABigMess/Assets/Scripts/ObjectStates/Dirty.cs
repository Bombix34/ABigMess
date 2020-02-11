using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

class Dirty : MonoBehaviour
{

    Material defaultMaterial;
    ObjectState objectState;

    private Material dirtyMaterial;

    private Renderer[] renderers;

    private void Awake()
    {
        defaultMaterial = GetComponent<Renderer>().material;
        dirtyMaterial = Instantiate(Resources.Load<Material>(@"Materials/Dirty"));

        dirtyMaterial.SetFloat("_offsetX", Random.Range(0, 200));
        dirtyMaterial.SetFloat("_offsetY", Random.Range(0, 200));

        renderers = GetComponentsInChildren<Renderer>();
    }

    void Start()
    {
        objectState = GetComponent<ObjectState>();
        foreach (var renderer in renderers)
        {
            // Append outline shaders
            var materials = renderer.sharedMaterials.ToList();

            materials.Add(dirtyMaterial);

            renderer.materials = materials.ToArray();
        }
    }

    private void OnDestroy()
    {

        foreach (var renderer in renderers)
        {
            // Append outline shaders
            var materials = renderer.sharedMaterials.ToList();

            materials.Remove(dirtyMaterial);

            renderer.materials = materials.ToArray();
        }
    }
}

