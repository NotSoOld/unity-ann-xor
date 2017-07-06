using UnityEngine;
using UnityEngine.UI;

namespace ANN  {

	public class Neuron : MonoBehaviour  {

		public int inputsNumber;
		public int outputsNumber;
		public Axon[] inputs;
		public Axon[] outputs;
		public double value;
		public Text text;
		public double lastTempSum;
		public double mistake;
		public double desiredValue;

		void Reset()  {
			text = GetComponentInChildren<Text>();
		}

	}

}