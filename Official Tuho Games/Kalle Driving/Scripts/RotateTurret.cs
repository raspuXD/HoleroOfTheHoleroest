using System.Collections;
using UnityEngine;

public class RotateTurret : MonoBehaviour
{
    public float sensitivity = 100f;
    public float minXRotation = -45f;
    public float maxXRotation = 45f;
    public float minYRotation = -90f;
    public float maxYRotation = 90f;

    private float xRotation = 0f;
    private float yRotation = 0f;
    private bool canShoot = true;

    public GameObject whatToSpawn;
    public Transform shootPoint;
    public float delayBetweenShots = 1f;
    public float shootDistance = 100f;
    public LineRenderer lineRenderer;
    public LayerMask ignoreLayer;
    public Animator animta;

    private Quaternion initialRotation;

    void Start()
    {
        ignoreLayer = LayerMask.GetMask("Mitsu");
        initialRotation = transform.localRotation;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        yRotation += mouseX;
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, minXRotation, maxXRotation);
        yRotation = Mathf.Clamp(yRotation, minYRotation, maxYRotation);

        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);

        if (canShoot)
        {
            ShowAimingLine();
        }
        else
        {
            lineRenderer.enabled = false;
        }

        if (Input.GetMouseButtonDown(0) && canShoot)
        {
            StartCoroutine(Shoot());
        }
    }

    public void ResetRotation()
    {
        transform.localRotation = initialRotation;
    }

    void ShowAimingLine()
    {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, shootPoint.position);
        lineRenderer.SetPosition(1, shootPoint.position + shootPoint.forward * shootDistance);
    }

    IEnumerator Shoot()
    {
        canShoot = false;
        lineRenderer.enabled = false;

        RaycastHit hit;
        if (Physics.Raycast(shootPoint.position, shootPoint.forward, out hit, shootDistance, ~ignoreLayer))
        {
            AudioManager.Instance.PlaySFX("Cannon");
            animta.SetTrigger("Shoot");
            Instantiate(whatToSpawn, hit.point, Quaternion.identity);
            StartCoroutine(ShowShotLine(hit.point));
        }

        yield return new WaitForSeconds(delayBetweenShots);
        canShoot = true;
    }

    IEnumerator ShowShotLine(Vector3 hitPoint)
    {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, shootPoint.position);
        lineRenderer.SetPosition(1, hitPoint);
        yield return new WaitForSeconds(0.1f);
        lineRenderer.enabled = false;
    }
}
