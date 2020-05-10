using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
	[HideInInspector]
	public IMechanic mechanic;
	public enum MechanicType
	{
		Normal,
		ReverseGravity,
		ZeroGravity,
		SizeChange,
		Flying
	};
	public MechanicType mechanicType;
	void Start()
	{
		switch (mechanicType) {
		case MechanicType.Normal:
		{
			mechanic = gameObject.AddComponent("NormalMechanic") as NormalMechanic;
			break;
		}
		case MechanicType.ReverseGravity:
		{
			mechanic = gameObject.AddComponent("ReverseGravityMechanic") as ReverseGravityMechanic;
			break;
		}
		case MechanicType.ZeroGravity:
		{
			mechanic = gameObject.AddComponent("ZeroGravityMechanic") as ZeroGravityMechanic;
			break;
		}
		case MechanicType.SizeChange:
		{
			mechanic = gameObject.AddComponent("SizeChangeMechanic") as SizeChangeMechanic;
			break;
		}
		case MechanicType.Flying:
		{
			mechanic = gameObject.AddComponent("FlyingMechanic") as FlyingMechanic;
			break;
		}
		
		}

		mechanic.Start ();
	}
}
