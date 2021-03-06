﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : MonoBehaviour
{
    public float     gap = 1f;
    public Vector2   hitBox = new Vector2(1,1);
    public LayerMask attackLayer;
    public GameObject flash;
    public AudioClip attackSound;

    private AudioSource _audioSource;
    private Vector2 gapAttackVector;
    private Vector2 pointA, pointB;
    private Collider2D[] attackColliders = new Collider2D[12];
    private ContactFilter2D attackFilter;
    private GeneratorText generatorTextHit; 

    private void Awake()
    {
        attackFilter.useLayerMask = true;
    }

    private void Start()
    {
        attackFilter.layerMask      = attackLayer;
        generatorTextHit            = GetComponent<GeneratorText>();
        _audioSource                = GetComponent<AudioSource>();
    }

    private void Update()
    {

        Debug.DrawLine(transform.position, (Vector2)transform.position + gapAttackVector, Color.green);
        Debug.DrawLine(pointA, pointB, Color.red);
    }


    public void Attack(Vector2 attackDirection, int danger)
    {
        //Aux para determinar objecto que recibe ataque
        GameObject attackedObject; 

        attackFilter.useLayerMask = true;

        //Direccion de ataque
        ConfigHitBox(attackDirection);

        //Solapar y contar elementos goleados (devuelve la cantidad)
        int attackedElements = Physics2D.OverlapArea(pointA, pointB, attackFilter, attackColliders);
        //Debug.Log(attackedElements);


        //PlaySound
        _audioSource.clip = attackSound;
        _audioSource.Play();

        for (int i = 0; i<attackedElements; i++)
        {
            attackedObject = attackColliders[i].gameObject;

            //Objecto con script Attackable
            if (attackedObject.gameObject.GetComponent<Attackable>())
            {
                attackedObject.gameObject.GetComponent<Attackable>().ReceiveAttack(danger, attackDirection);
                InvokeFlash(attackedObject);

                GenerateTextHit(danger, attackedObject);

            }

        }
    }

    private void GenerateTextHit(int danger, GameObject attackedObject)
    {
        if (generatorTextHit)
        {
            generatorTextHit.CreateTextHit(generatorTextHit.textHit,
                                           danger,
                                           attackedObject.transform,
                                           0.5f,
                                           Color.white,
                                           generatorTextHit.rangeXDefault,
                                           generatorTextHit.rangeYDefault,
                                           2f);
        }
    }

    private void InvokeFlash(GameObject attackedObject)
    {
        if (flash != null)
        {
            Instantiate(flash, attackedObject.transform);
        }
    }

    private void ConfigHitBox(Vector2 attackDirection)
    {
        //Escalar vectores dependiendo del tamaño del GameObject(Player)
        Vector2 scale       = transform.lossyScale;
        Vector2 hitBoxScale = Vector2.Scale(hitBox, scale);

        //Asignar 
        gapAttackVector = Vector2.Scale(attackDirection.normalized * gap, scale);
        pointA          = (Vector2)transform.position + gapAttackVector - hitBoxScale * 0.5f;
        pointB          = pointA + hitBoxScale;
    }
}
