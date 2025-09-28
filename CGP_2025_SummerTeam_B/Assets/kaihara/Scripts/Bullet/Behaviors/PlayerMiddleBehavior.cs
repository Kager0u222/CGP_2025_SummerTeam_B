using UnityEngine;

public class PlayerMiddleBehavior : MagicBehavior
{

    public override void First(ParticleSystem particle, ParticleSystemRenderer renderer)
    {
        ParticleSystem.MainModule main = particle.main;
        ParticleSystem.EmissionModule emission = particle.emission;
        ParticleSystem.ShapeModule shape = particle.shape;
        ParticleSystem.TrailModule trail = particle.trails;

        trail.enabled = false;

        main.startLifetime = 1;
        main.startSpeed = 4;
        main.startSize = 2;
        main.startColor = Color.white;

        emission.rateOverTime = 0;
        emission.rateOverDistance = 1;

        shape.angle = 0;
        particle.Play();
    }
    public override void Movement()
    {

    }

    public override void Hit()
    {
        
    }

    
}