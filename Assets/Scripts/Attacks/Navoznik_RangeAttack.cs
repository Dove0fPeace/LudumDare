using UnityEngine;

public class Navoznik_RangeAttack : RangeAttack
{
    public float BallRestoreTime;
    private float currentRestoreTime;

    private bool ballIsSpawned;
    private BugBall currentProjectile;

    protected override void Start()
    {
        base.Start();
        SpawnBall();
        currentRestoreTime = BallRestoreTime;
    }

    private void Update()
    {
        if(ballIsSpawned == false)
        {
            currentRestoreTime -= Time.deltaTime;
            if(currentRestoreTime <= 0)
            {
                SpawnBall();
                currentRestoreTime = BallRestoreTime;
            }
        }
    }

    private void SpawnBall()
    {
        ballIsSpawned = true;
        var projectile = Instantiate(ProjectilePrefab, HandsPlace.position, HandsPlace.rotation, HandsPlace);
        projectile.Spawn(self, Damage);
        currentProjectile = projectile.transform.GetComponent<BugBall>();
    }

    public override bool Attack()
    {
        if (ballIsSpawned == false) return false;
        currentProjectile.Launch();
        ballIsSpawned = false;
        return true;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if(currentProjectile != null)
        {
            Destroy(currentProjectile.gameObject);
        }
    }
}
