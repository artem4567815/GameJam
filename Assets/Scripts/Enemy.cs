using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2f;
    public float followDistance = 5f;
    public int damage = 10;
    public float attackDelay = 1f;
    public float attackDistance = 1f;
    public float attackPrepareDelay = 0.5f;

    private Transform player;
    private PlayerHealth playerHealth;
    private EnemyHealth health;

    private float lastAttackTime = -Mathf.Infinity;
    private bool isPreparingAttack = false;
    private float attackPrepareTimer = 0f;

    private LineRenderer attackCircleRenderer;
    private MeshFilter filledMeshFilter;
    private GameObject fillObj;
    private int circleSegments = 60;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
    }

    void Start()
{
    // Круг радиуса атаки
    GameObject attackCircleObj = new GameObject("AttackCircle");
    attackCircleObj.transform.parent = transform;
    attackCircleObj.transform.localPosition = Vector3.zero;
    attackCircleRenderer = attackCircleObj.AddComponent<LineRenderer>();
    attackCircleRenderer.positionCount = circleSegments + 1;
    attackCircleRenderer.loop = true;
    attackCircleRenderer.startWidth = 0.07f;
    attackCircleRenderer.endWidth = 0.07f;
    attackCircleRenderer.material = new Material(Shader.Find("Sprites/Default"));
    attackCircleRenderer.startColor = Color.red;
    attackCircleRenderer.endColor = Color.red;
    attackCircleRenderer.enabled = false;

    // Закрашенный сектор
    fillObj = new GameObject("FillMesh");
    fillObj.transform.parent = transform;
    fillObj.transform.localPosition = new Vector3(0, -0.1f, 0); // смещение вниз
    filledMeshFilter = fillObj.AddComponent<MeshFilter>();
    MeshRenderer meshRenderer = fillObj.AddComponent<MeshRenderer>();
    var mat = new Material(Shader.Find("Sprites/Default"));
    Color transparentRed = new Color(1f, 0f, 0f, 0.4f); // красный с альфа 0.4
    mat.color = transparentRed;
    mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
    mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
    mat.SetInt("_ZWrite", 0);
    mat.DisableKeyword("_ALPHATEST_ON");
    mat.EnableKeyword("_ALPHABLEND_ON");
    mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
    mat.renderQueue = 2600; // стандартный слой для Background
    meshRenderer.material = mat;
    fillObj.SetActive(false);

    // Безопасное получение игрока
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
    if (playerObj != null)
    {
        player = playerObj.transform;
        playerHealth = playerObj.GetComponent<PlayerHealth>();
    }
    else
    {
        Debug.LogError("Player с тегом 'Player' не найден!");
    }

    health = GetComponent<EnemyHealth>();
}

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        float fillPercent = 0f;

        if (distance <= followDistance || health.currentHealth < health.maxHealth)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            transform.position += (Vector3)(direction * speed * Time.deltaTime);
        }

        if (distance <= attackDistance)
        {
            attackCircleRenderer.enabled = true;
            fillObj.SetActive(true);
            if (!isPreparingAttack && Time.time >= lastAttackTime + attackDelay)
            {
                isPreparingAttack = true;
                attackPrepareTimer = 0f;
            }
            if (isPreparingAttack)
            {
                attackPrepareTimer += Time.deltaTime;
                fillPercent = Mathf.Clamp01(attackPrepareTimer / attackPrepareDelay);
                DrawFilledSector(fillPercent);
                if (attackPrepareTimer >= attackPrepareDelay)
                {
                    if (playerHealth != null)
                    {
                        playerHealth.TakeDamage(damage);
                        lastAttackTime = Time.time;
                    }
                    isPreparingAttack = false;
                    fillObj.SetActive(false);
                }
            }
            else
            {
                fillObj.SetActive(false);
            }
        }
        else
        {
            isPreparingAttack = false;
            fillObj.SetActive(false);
            attackCircleRenderer.enabled = false;
        }
    }

    void DrawFilledSector(float fillPercent)
    {
        float currentRadius = attackDistance * fillPercent;
        float angleStep = 360f / circleSegments;
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        vertices.Add(Vector3.zero);

        for (int i = 0; i <= circleSegments; i++)
        {
            float angle = Mathf.Deg2Rad * (i * angleStep);
            float x = Mathf.Cos(angle) * currentRadius;
            float y = Mathf.Sin(angle) * currentRadius;
            vertices.Add(new Vector3(x, y, 0));
        }

        for (int i = 1; i < vertices.Count - 1; i++)
        {
            triangles.Add(0);
            triangles.Add(i);
            triangles.Add(i + 1);
        }

        Mesh mesh = new Mesh();
        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        filledMeshFilter.mesh = mesh;
    }
}