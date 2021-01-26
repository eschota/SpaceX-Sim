using UnityEngine;

namespace eWolfRoadBuilderHelpers
{
	/// <summary>
	/// Radian, Pi *2 for a full 360 number range always between 0 and PI*2
	/// </summary>
	public class Radian : NumberWrapper
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public Radian() : base(0, Mathf.PI * 2)
		{
		}

		/// <summary>
		/// Constructor to set the starting value
		/// </summary>
		/// <param name="val">The starting value</param>
		public Radian(float val) : base(0, Mathf.PI * 2)
		{
			Value = val;
		}

		/// <summary>
		/// Get/Sets the value
		/// </summary>
		public float Value
		{
			get { return ValueImp; }
			set { ValueImp = value; }
		}
	}
}