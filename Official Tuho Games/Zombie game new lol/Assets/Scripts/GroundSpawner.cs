using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class GroundSpawner : MonoBehaviour
{
    [SerializeField] ZombieHealth zombiePrefab;
    [SerializeField] Transform whereToSpawn;
    [SerializeField] ParticleSystem particles;
    [SerializeField] GameObject visualizer;

    public float delayBetweenSpawningAndParticle = 1.5f, howQuicklyMovesZombieFromUnderground = 2f;

    ZombieComponetHolder zomponents;
    ZombieHealth currentZombie;

    [SerializeField] RoundManager roundManager;
    [SerializeField] Points points;
    [SerializeField] CurrentDrops currentDrops;

    public bool isSpawningZombieCurrently;

    private void Start()
    {
        Destroy(visualizer);
    }

    public void StartTheSpawning()
    {
        StartCoroutine(SpawnZombie());
    }

    public IEnumerator SpawnZombie()
    {
        if(isSpawningZombieCurrently == false)
        {
            isSpawningZombieCurrently = true;
            var mainModule = particles.main;
            mainModule.duration = delayBetweenSpawningAndParticle + howQuicklyMovesZombieFromUnderground - .2f;
            particles.Play();

            currentZombie = Instantiate(zombiePrefab, whereToSpawn.position, whereToSpawn.rotation);
            roundManager.theZombies.Add(currentZombie);
            zomponents = currentZombie.GetComponent<ZombieComponetHolder>();
            zomponents.ActivateAllComponents(false);

            currentZombie.points = points;
            currentZombie.currentDrops = currentDrops;
            currentZombie.roundManager = roundManager;

            yield return new WaitForSeconds(delayBetweenSpawningAndParticle);

            StartCoroutine(MoveZombieToLocation(currentZombie, transform.position, howQuicklyMovesZombieFromUnderground));
        }
    }

    private IEnumerator MoveZombieToLocation(ZombieHealth zombie, Vector3 targetPosition, float duration)
    {
        if (zombie == null)
        {
            isSpawningZombieCurrently = false;
            particles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            yield break;
        }

        Vector3 startPosition = zombie.transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            if (zombie == null)
            {
                isSpawningZombieCurrently = false;
                particles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                yield break;
            }

            zombie.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (zombie != null)  // Check again before accessing the zombie's components
        {
            zombie.transform.position = targetPosition;
            zomponents.ActivateAllComponents(true);
        }

        yield return new WaitForSeconds(1f);
        isSpawningZombieCurrently = false;
    }

}
