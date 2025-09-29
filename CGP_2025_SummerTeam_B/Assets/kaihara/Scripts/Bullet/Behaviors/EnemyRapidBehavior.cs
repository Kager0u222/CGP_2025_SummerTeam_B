using UnityEngine;

public class EnemyRapidBehavior : MagicBehavior
{
    public override void First(ParticleSystem particle,ParticleSystemRenderer renderer)
    {
        ParticleSystem.MainModule main = particle.main;
        ParticleSystem.EmissionModule emission = particle.emission;
        ParticleSystem.ShapeModule shape = particle.shape;
        ParticleSystem.TrailModule trail = particle.trails;

        trail.enabled = false;

        main.startLifetime = 1;
        main.startSpeed = 10;
        main.startSize = 7f;
        main.startColor = Color.white;

        emission.rateOverTime = 0;
        emission.rateOverDistance = 20;

        shape.angle = 10;
        shape.radius = 0.00001f;
        particle.Play();
    }
    public override void Movement()
    {

    }

    public override void Hit()
    {

    }

}