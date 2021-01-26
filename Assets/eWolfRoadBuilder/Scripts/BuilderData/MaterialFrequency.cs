using System;
using UnityEngine;

namespace eWolfRoadBuilder
{
	[Serializable]
	public class MaterialFrequency
	{
		public enum FrequencyRate
		{
			/// <summary>
			/// The main road will look like this material
			/// </summary>
			MainTexture,

			/// <summary>
			/// This material will only appear once per road
			/// </summary>
			OncePerRoad,

			/// <summary>
			/// This material will about about 50 percent of the time
			/// </summary>
			Randon50Percent,

			/// <summary>
			/// This material will about about 25 percent of the time
			/// </summary>
			Randon25Percent,

			/// <summary>
			/// The material will be in the middle of the road
			/// </summary>
			MiddleOfRoad
		}

		/// <summary>
		/// How often the material should appear
		/// </summary>
		public FrequencyRate Frequency = FrequencyRate.MainTexture;

		/// <summary>
		/// The material to use
		/// </summary>
		public Material Material;
	}
}