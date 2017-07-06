using UnityEngine;
using UnityEngine.UI;

namespace ANN  {

	public class Layer : MonoBehaviour  {

		public int neuronsCount;
		public Neuron[] neurons;

		public void PropagateForward()  {
			double tempSum;
			for(int i = 0; i < neuronsCount; i++)  {
				tempSum = 0;
				for(int j = 0; j < neurons[i].inputsNumber; j++)  {
					tempSum += neurons[i].inputs[j].value * neurons[i].inputs[j].inNeuron.value;
				}
				neurons[i].value = ANN.tanh(tempSum);
				neurons[i].text.text = System.Math.Round(neurons[i].value, 3).ToString();
				neurons[i].lastTempSum = tempSum;
			}
		}

		public void FindMistakesInNeurons()  {
			double tempSum;
			for(int i = 0; i < neuronsCount; i++)  {
				tempSum = 0;
				for(int j = 0; j < neurons[i].outputsNumber; j++)  {
					tempSum += neurons[i].outputs[j].value * neurons[i].outputs[j].outNeuron.mistake;
				}
				neurons[i].mistake = tempSum;
			}
		}

		public void GradientDescent()  {
			for(int i = 0; i < neuronsCount; i++)  {
				for(int j = 0; j < neurons[i].inputsNumber; j++)  {
					neurons[i].inputs[j].value = 
						neurons[i].inputs[j].value + 
						ANN.step * 
						neurons[i].mistake * 
						neurons[i].inputs[j].inNeuron.value * 
						ANN.dtanh(neurons[i].lastTempSum)
					;
					neurons[i].inputs[j].text.text = System.Math.Round(neurons[i].inputs[j].value, 3).ToString();
				}
			}
		}

	}

}