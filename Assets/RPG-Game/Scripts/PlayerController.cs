﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//[RequireComponent(typeof(Attributes))]
public class PlayerController : MonoBehaviour
{
    // Descontinuada 
    //[SerializeField]
    //private float _velocity;

    //Fisicas
    private InputPlayer inputPlayer;
    private Rigidbody2D rgb2D;
    private Transform _transform;

    //Movimiento
    private Animator _anim;
    private bool _isBack;
    private SpriteRenderer _sprite;
    int RunHashCode;

    //Atributos
    public Attributes attributePlayer;
    private Attacker attacker;
    private Salud salud;

    //Nivel de Exp
    private ExperienceLevel experienceLevel;

    //Mascara
    public LayerMask layerMaskInteration;


    // Start is called before the first frame update
    void Start()
    {
        inputPlayer = GetComponent<InputPlayer>();
        rgb2D       = GetComponent<Rigidbody2D>();

        _transform = GetComponent<Transform>();
        _anim      = GetComponent<Animator>();
        _sprite    = GetComponent<SpriteRenderer>();

        RunHashCode     = Animator.StringToHash("IsRun");
        attacker        = GetComponent<Attacker>();

        salud = GetComponent<Salud>();

        experienceLevel = GetComponent<ExperienceLevel>();

        //Inicializar texto de paneles
        PanelAttribute.Instance.UpdateTextAtributte(attributePlayer, salud, experienceLevel);
    }


    //Fisicas siempre con el Fixed
    void FixedUpdate()
    {
        //Mover al jugador
        if (_anim.GetBool("Attacking"))
        {
            rgb2D.velocity = Vector2.zero;
        }
        else
        {
            Vector2 _VelocityVector = new Vector2(inputPlayer._horizontal, inputPlayer._vertical) * attributePlayer.velocity;
            rgb2D.velocity = _VelocityVector;
        }
    }

    void Update()
    {

        //Animacion Vertical y horizontal
        MoveLookAt();

        //Input de Ataque 
        if (inputPlayer._attack)
        {
            _anim.SetBool("Attacking", true);
        }

        if (inputPlayer._inventory)
        {
            MenuPanel.instance.OpenCloseInventory();
        }

    }

    private void MoveLookAt()
    {
        if (inputPlayer._horizontal != 0 || inputPlayer._vertical != 0)
        {
            SetXYAnimator();
            _anim.SetBool(RunHashCode, true);
        }
        else
        {
            _anim.SetBool(RunHashCode, false);
        }
    }

    private void SetXYAnimator()
    {
        _anim.SetFloat("X", inputPlayer._horizontal);
        _anim.SetFloat("Y", inputPlayer._vertical);
    }



    private void AttackController()
    {
        attacker.Attack(inputPlayer.LookDirection, attributePlayer.attack);
        _anim.SetBool("Attacking", false);


    }

    public RaycastHit2D[] Interact()
    {
        RaycastHit2D[] circleCast = Physics2D.CircleCastAll(transform.position, 
                                                            GetComponent<CapsuleCollider2D>().size.x, 
                                                            inputPlayer.LookDirection.normalized,
                                                            2f,
                                                            layerMaskInteration);
        if (circleCast != null)
        {
            return circleCast;
        }
        else
        {
            return null;
        }
    }


    //private void SplitSprite()
    //{
    //    // Flip del sprite
    //    if (inputPlayer._horizontal > 0 && Mathf.Abs(inputPlayer._vertical) <= Mathf.Abs(inputPlayer._horizontal))
    //    {
    //        _sprite.flipX = true;
    //    }
    //    else if (inputPlayer._horizontal != 0)
    //    {
    //        _sprite.flipX = false;
    //    }
    //}


}
