using UnityEngine;
using UnityEngine.UI;

namespace ANN  {

	public class Axon : MonoBehaviour  {

		public Neuron inNeuron;
		public Neuron outNeuron;
		public double value;
		public Text text;

		void Reset()  {
			text = GetComponentInChildren<Text>();
		}
	
	}
}
