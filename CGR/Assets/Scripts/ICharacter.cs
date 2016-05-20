using UnityEngine;
using System.Collections;

public enum StateChange { CANNON, CANNON_FIRE, CANNON_COLLISION, PORTAL_IN, PORTAL_OUT, MINION, SWALK_ON, SWALK_OFF, BOX_IN, BOX_OUT, CHECKPOINT, DEATH }

public interface ICharacter{

    /// <summary>
    /// Reactivates the control for a character
    /// </summary>
    /// <param name="state"></param>
    void ReactivateControl(StateChange state);

    /// <summary>
    /// deactivates the control for a character
    /// </summary>
    /// <param name="state"></param>
    void DeactivateControl(StateChange state);
}
