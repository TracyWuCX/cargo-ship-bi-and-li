using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("=== Stats Settings ===")]
    public Rigidbody rb;
    public GameObject explosion;
    public LayerMask Enemy;
    [Range(0.111f, 0.999f)]
    public float bouciness;
    public bool useGravity;

    [Header("=== Damage Settings ===")]
    public int explosionDamage;
    public float explosionRange;

    [Header("=== Lifetime Settings ===")]
    public int maxCollisions;
    public float maxLifetime;
    public bool explosionOnTouch = true;

    int collisions;
    PhysicMaterial physics_mat;

    // Start is called before the first frame update
    private void Start()
    {
        // Create a new Physic material
        physics_mat = new PhysicMaterial();
        physics_mat.bounciness = bouciness;
        physics_mat.frictionCombine = PhysicMaterialCombine.Minimum;
        physics_mat.bounceCombine = PhysicMaterialCombine.Maximum;
        // Assign material to collider
        GetComponent<SphereCollider>().material = physics_mat;
        // Use Gravity
        rb.useGravity = useGravity;
    }

    // Update is called once per frame
    private void Update()
    {
        if (collisions > maxCollisions)
        {
            Explode();
        }
        maxLifetime -= Time.deltaTime;
        if (maxLifetime <= 0)
        {
            Explode();
        }
    }

    private void Explode()
    {
        if (explosion != null && Time.timeScale == 1)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
        }

        Collider[] enemies = Physics.OverlapSphere(transform.position, explosionRange, Enemy);
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].GetComponent<Fish>().currentHealth -= 1;
        }

        Invoke("Delay", 0.05f);
    }

    private void Delay()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        collisions++;
        if (collision.collider.CompareTag("Enemy") && explosionOnTouch)
        {
            Explode();
        }
        if (collision.collider.CompareTag("Ground") && explosionOnTouch)
        {
            Explode();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }

}
