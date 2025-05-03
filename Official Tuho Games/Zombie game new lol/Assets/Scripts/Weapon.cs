using System.Collections;
using UnityEngine;
using TMPro;

public class Weapon : MonoBehaviour
{
    [Header("Weapon Damage Related")]
    public float bulletDamage = 10f;
    public float damageFalloffStart = 50f;

    [Header("Ammo")]
    public float cooldownBetweenShots = 0.2f;
    public int magazineSize = 30;
    public int maxAmmo = 90;
    public float reloadTime;

    [Header("Burst")]
    public bool isBurstFire;
    public int burstCount = 3;
    public float burstFireRate = 0.1f;

    [Header("Shotgun")]
    public bool isShotgun;
    public int pelletsCount = 9;
    public float spreadAngle = 5f;

    [Header("Knockback")]
    public float knockBackPower;
    public float returnSpeed;

    [Header("UI")]
    [SerializeField] TMP_Text ammoText;

    [Header("Others")]
    private int currentAmmo;
    public GameObject box;
    private int currentMagazine;
    private bool isReloading = false;
    private float lastShotTime;
    public Camera camerar;
    public ArmSway arm;
    float originalZ;

    void Start()
    {
        currentMagazine = magazineSize;
        currentAmmo = maxAmmo;
        UpdateUI();
        originalZ = transform.localPosition.z;
    }

    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            if (Time.time >= lastShotTime + cooldownBetweenShots)
            {
                if (currentMagazine <= 0 && !isReloading)
                {
                    StartCoroutine(Reload());
                }
                else if (isBurstFire)
                {
                    StartCoroutine(BurstFire());
                }
                else
                {
                    AutomaticFire();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
        }
    }

    private IEnumerator BurstFire()
    {
        if (currentMagazine >= burstCount && !isReloading)
        {
            for (int i = 0; i < burstCount; i++)
            {
                Shoot();
                yield return new WaitForSeconds(burstFireRate);
            }
            lastShotTime = Time.time;
        }
    }

    private void AutomaticFire()
    {
        if (currentMagazine > 0 && !isReloading)
        {
            Shoot();
        }
        else if (currentMagazine <= 0 && !isReloading)
        {
            StartCoroutine(Reload());
        }
    }


    private void Shoot()
    {
        if (currentMagazine <= 0) return;

        currentMagazine--;
        UpdateUI();
        lastShotTime = Time.time;
        Knockback(knockBackPower, returnSpeed);

        int layerMask = ~LayerMask.GetMask("IgnoreForWeapon");

        if (isShotgun)
        {
            for (int i = 0; i < pelletsCount; i++)
            {
                Vector3 randomSpread = new Vector3(
                    Random.Range(-spreadAngle, spreadAngle),
                    Random.Range(-spreadAngle, spreadAngle),
                    0);

                Ray ray = camerar.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
                ray.direction = Quaternion.Euler(randomSpread) * ray.direction;

                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
                {
                    ProcessHit(hit);
                }
            }
        }
        else
        {
            Ray ray = camerar.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                ProcessHit(hit);
            }
        }
    }

    private void ProcessHit(RaycastHit hit)
    {
        float distance = hit.distance;
        float finalDamage = bulletDamage;

        if (distance > damageFalloffStart)
        {
            float falloffFactor = (distance - damageFalloffStart) / (distance - damageFalloffStart + 1f);
            finalDamage *= Mathf.Clamp01(1 - falloffFactor);
        }

        var target = hit.collider;
        GameObject indigator = Instantiate(box, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(indigator, 2f);

        Debug.Log(target + " " + finalDamage);

        if (target.CompareTag("Zombie"))
        {
            ZombieHealth health = target.GetComponent<ZombieHealth>();
            health.TakeDamage(finalDamage);
        }
    }
    private IEnumerator Reload()
    {
        if (isReloading || currentAmmo <= 0 || currentMagazine == magazineSize) yield break;

        isReloading = true;
        yield return new WaitForSeconds(reloadTime); // Example reload time

        int ammoToLoad = magazineSize - currentMagazine;
        if (currentAmmo >= ammoToLoad)
        {
            currentMagazine += ammoToLoad;
            currentAmmo -= ammoToLoad;
        }
        else
        {
            currentMagazine += currentAmmo;
            currentAmmo = 0;
        }

        UpdateUI();
        isReloading = false;
    }

    public void UpdateUI()
    {
        ammoText.text = currentMagazine.ToString() + "/" + currentAmmo;
    }

    public void AddAmmo(int amount)
    {
        currentAmmo = Mathf.Min(maxAmmo, currentAmmo + amount);
        UpdateUI();
    }

    public void Knockback(float power, float returnSpeed)
    {
        transform.localPosition -= new Vector3(0, 0, power);
        StartCoroutine(ReturnToOriginalPosition(returnSpeed));
    }

    private IEnumerator ReturnToOriginalPosition(float speed)
    {
        while (Mathf.Abs(transform.localPosition.z - originalZ) > 0.01f)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(transform.localPosition.x, transform.localPosition.y, originalZ), speed * Time.deltaTime);
            yield return null;
        }

        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, originalZ);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector3 startPoint = camerar.transform.position;
        Vector3 forwardDirection = camerar.transform.forward;
        Vector3 falloffEndPoint = startPoint + forwardDirection * damageFalloffStart;
        Gizmos.DrawLine(startPoint, falloffEndPoint);

        if (camerar == null && isShotgun == false) return;

        Gizmos.color = Color.red; // Choose a color for the Gizmo lines

        // Draw the spread for each pellet
        for (int i = 0; i < pelletsCount; i++)
        {
            Vector3 randomSpread = new Vector3(
                Random.Range(-spreadAngle, spreadAngle),
                Random.Range(-spreadAngle, spreadAngle),
                0);

            Ray ray = camerar.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            ray.direction = Quaternion.Euler(randomSpread) * ray.direction;

            // Draw a line to represent the ray
            Gizmos.DrawRay(ray.origin, ray.direction * 10); // Adjust the length as needed
        }
    }
}
