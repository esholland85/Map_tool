using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinSettings : Character
{
    public GameObject rightLegArmor;
    public GameObject leftLegArmor;
    public GameObject rightDagger;
    public GameObject leftDagger;
    //moved stone for simpler handling: supposed to be child of "slingshot_rope_middle" for animations
    public GameObject slingshot;
    public GameObject shield;
    public GameObject sword;
    public GameObject rightBracer;
    public GameObject leftBracer;
    public GameObject shoulderArmor;

    public bool rLArmor = false;
    public bool lLArmor = false;
    public bool rDagger = false;
    public bool lDagger = false;
    public bool lSlingshot = false;
    public bool lShield = false;
    public bool rSword = false;
    public bool rBracer = false;
    public bool lBracer = false;
    public bool rShoulder = false;

    public override void Start()
    {
        base.Start();
        GearUp();
    }

    public void GearUp()
    {
        rightLegArmor.SetActive(rLArmor);
        leftLegArmor.SetActive(lLArmor);
        rightDagger.SetActive(rDagger);
        leftDagger.SetActive(lDagger);
        slingshot.SetActive(lSlingshot);
        shield.SetActive(lShield);
        sword.SetActive(rSword);
        rightBracer.SetActive(rBracer);
        leftBracer.SetActive(lBracer);
        shoulderArmor.SetActive(rShoulder);
    }
}
