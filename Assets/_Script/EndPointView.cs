using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPointView : CatchableView
{
    [SerializeField] private Sprite free;
    public Sprite catched;
    [SerializeField] private ParticleSystem catchParticle;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private void Start()
    {
        spriteRenderer.SetSprite(free);
    }
    protected override void OnCatch(Candy player)
    {
        catchParticle.Play();
        spriteRenderer.SetSprite(catched);
        player.GetComponent<SpriteRenderer>().SetActive(false);
    }
}
