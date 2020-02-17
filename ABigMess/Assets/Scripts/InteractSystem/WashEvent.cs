using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "BIGMESS_EVENTS/Wash Event")]
public class WashEvent : InteractEvent
{

    [SerializeField] GameObject sparklesParticles;

    [SerializeField] float sparklesParticlesHeight = 0.5f;

    public override void InteractionEvent(GameObject objConcerned)
    {
        if(MusicManager.Instance!=null)
        {
            MusicManager.Instance.GetSoundManager().UseSpongeSound();
        }
        SetupObjectState(objConcerned);
        TryInstantiateParticleFX(objConcerned);
        if (sparklesParticles != null)
        {
            objConcerned.GetComponent<MonoBehaviour>().StartCoroutine(InstantiateSparkles(objConcerned));
        }
    }

    public IEnumerator InstantiateSparkles(GameObject objConcerned)
    {
        yield return new WaitForSeconds(chronoBeforeKillingParticles);
        if (objConcerned.GetComponentInChildren<ParticleSystem>() == null)
        {
            GameObject particles = Instantiate(sparklesParticles, objConcerned.transform);
            particles.transform.position = new Vector3(particles.transform.position.x, 
                particles.transform.position.y + sparklesParticlesHeight, 
                particles.transform.position.z);

            Destroy(particles, 1f);
        }
    }
}
