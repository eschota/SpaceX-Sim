using UnityEngine;

namespace eWolfRoadBuilder
{
    public class OverridableMaterialFrequency : MonoBehaviour, IMaterialFrequency
    {
        #region Public Fields
        /// <summary>
        /// How the road will look
        /// </summary>
        public MaterialFrequency[] Details;

        /// <summary>
        /// Get the materials details
        /// </summary>
        public MaterialFrequency[] GetDetails
        {
            get
            {
                return Details;
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Populated the materials
        /// </summary>
        /// <param name="rnl">The current road network layout</param>
        public void PopulateDefaultMaterials(RoadNetworkLayout rnl)
        {
            if (rnl == null)
                return;

            RoadBuilder rb = rnl.GetComponent<RoadBuilder>();
            if (rb == null)
                return;

            Copy(rb.GetComponent<IMaterialFrequency>());
        }

        /// <summary>
        /// Copy the material frequency detials
        /// </summary>
        /// <param name="materialFrequency">The master material frequency</param>
        public void Copy(IMaterialFrequency materialFrequency)
        {
            if (materialFrequency == null)
                return;

            Details = new MaterialFrequency[materialFrequency.GetDetails.Length];
            for (int i = 0; i < materialFrequency.GetDetails.Length; i++)
            {

                Details[i] = new MaterialFrequency();
                Details[i].Frequency = materialFrequency.GetDetails[i].Frequency;
                Details[i].Material = new Material(materialFrequency.GetDetails[i].Material);
            }
        }
        #endregion
    }
}
