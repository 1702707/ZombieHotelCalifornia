using Controller.Player;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private DamageTriggerZoneComponent _rightArm;
    [SerializeField] private DamageTriggerZoneComponent _leftArm;
    [SerializeField] private KickTriggerZoneComponent _rightLeg;
    [SerializeField] private KickTriggerZoneComponent _leftLeg;

    void Update()
    {
        if (Input.GetButton("LeftPunch"))
        {
            _leftArm.DoDamage();
        }
        
        if (Input.GetButton("RightPunch"))
        {
            _rightArm.DoDamage();
        }
        
        if (Input.GetButton("LeftKick"))
        {
            _leftLeg.DoKick();
        }
        
        if (Input.GetButton("RightKick"))
        {
            _rightLeg.DoKick();
        }
    }
}
