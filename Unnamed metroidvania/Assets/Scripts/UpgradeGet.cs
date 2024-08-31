using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]

public class UpgradeGet : MonoBehaviour
{
    public UpgradeType upgradeType;
    public SpellData spellData;

    /*/ Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }*/

    public enum UpgradeType
    {
        None,
        Dash,
        WallJump,
        DoubleJump,
        Spell,
        Paraglider,
        Grapple
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag != "Player")
        {
            return;
        }
        switch (upgradeType)
        {
            default:
            case UpgradeType.None: break;
            case UpgradeType.Dash:
                PlayerController.canDash = true;
                break;
            case UpgradeType.WallJump:
                PlayerController.canWallJump = true;
                break;
            case UpgradeType.DoubleJump:
                PlayerController.jumpMax = 2;
                break;
            case UpgradeType.Spell:
                PlayerController.eruditeSpells.Add(spellData);
                break;
            case UpgradeType.Paraglider:
                PlayerController.canParaglide = true;
                break;
            case UpgradeType.Grapple:
                PlayerController.canGrapple = true;
                break;
        }

        Destroy(gameObject);
        
    }
}
