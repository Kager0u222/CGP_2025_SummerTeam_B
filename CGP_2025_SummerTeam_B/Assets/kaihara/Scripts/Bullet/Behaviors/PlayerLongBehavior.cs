using TMPro;
using UnityEngine;

public class PlayerLongBehavior : MagicBehavior
{

    public override void First(ParticleSystem particle,ParticleSystemRenderer renderer)
    {
        ParticleSystem.MainModule main = particle.main;
        ParticleSystem.EmissionModule emission = particle.emission;
        ParticleSystem.ShapeModule shape = particle.shape;
        ParticleSystem.TrailModule trail = particle.trails;

        trail.enabled = true;
        trail.mode = ParticleSystemTrailMode.Ribbon;

        main.startLifetime = 0.1f;
        main.startSpeed = 150;
        main.startSize = 0.1f;
        main.startColor = Color.white;

        emission.rateOverTime = 0;
        emission.rateOverDistance = 10;

        shape.angle = 5;
        shape.radius = 0.0001f;
        particle.Play();
    }
    public override void Movement()
    {

    }

    public override void Hit()
    {

    }

}