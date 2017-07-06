using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

namespace ANN  {

	public class ANN : MonoBehaviour  {

		[Header("Hidden layers: ")]
		public int hiddenLayersCount;
		public Layer[] hiddenLayers;
		[Header("Input layer: ")]
		public Layer inputLayer;
		[Header("Output layer: ")]
		public Layer outputLayer;

		[Header("For teaching:")]
		public double[,] inputs = {{0, 0}, {0, 1}, {1, 0}, {1, 1}};
		public double[] desiredOutput = {0, 1, 1, 0};
		public static double step = 0.05;
		private bool isLearning = false;


		void Start()  {
			for(int i = 0; i < hiddenLayersCount; i++)  {
				for(int j = 0; j < hiddenLayers[i].neuronsCount; j++)  {
					for(int k = 0; k < hiddenLayers[i].neurons[j].inputsNumber; k++)  {
						hiddenLayers[i].neurons[j].inputs[k].value = Random.Range(-0.8f, 0.8f);
						hiddenLayers[i].neurons[j].inputs[k].text.text = System.Math.Round(hiddenLayers[i].neurons[j].inputs[k].value, 3).ToString();
					}
				}
			}
			
			for(int i = 0; i < outputLayer.neuronsCount; i++)  {
				for(int j = 0; j < outputLayer.neurons[i].inputsNumber; j++)  {
					outputLayer.neurons[i].inputs[j].value = Random.Range(-0.8f, 0.8f);
					outputLayer.neurons[i].inputs[j].text.text = System.Math.Round(outputLayer.neurons[i].inputs[j].value, 3).ToString();
				}
			}
		}

		public void OnClick_StartLearning()  {
			isLearning = true;

			if(File.Exists(Application.dataPath+"/ANN/weights.txt"))  {
				FileStream stream = new FileStream(Application.dataPath+"/ANN/weights.txt", FileMode.Open);
				StreamReader reader = new StreamReader(stream);
				string t = reader.ReadLine();
				string[] values = t.Split('#');
				int l = 0;
				for(int i = 0; i < inputLayer.neuronsCount; i++)  {
					for(int j = 0; j < inputLayer.neurons[i].outputsNumber; j++)  {
						inputLayer.neurons[i].outputs[j].value = System.Convert.ToDouble(values[l]);
						l++;
					}
				}
				for(int k = 0; k < hiddenLayersCount; k++)  {
					t = reader.ReadLine();
					values = t.Split('#');
					l = 0;
					for(int i = 0; i < hiddenLayers[k].neuronsCount; i++)  {
						for(int j = 0; j < hiddenLayers[k].neurons[i].outputsNumber; j++)  {
							hiddenLayers[k].neurons[i].outputs[j].value = System.Convert.ToDouble(values[l]);
							l++;
						}
					}
				}
				reader.Close();
			}

			StartCoroutine(Teacher());
		}

		IEnumerator Teacher()  {
			WaitForEndOfFrame delay = new WaitForEndOfFrame();
		//	WaitForSeconds delay = new WaitForSeconds(0.2f);
			while(isLearning)  {
				for(int i = 0; i < 4; i++)  {
					double[] arr = {inputs[i,0], inputs[i,1]};
					PropagateForward(arr, i);
					yield return delay;
					PropagateBack();
					yield return delay;
				}
			}
		}

		public void OnClick_StopLearning()  {
			isLearning = false;

			FileStream stream = new FileStream(Application.dataPath+"/ANN/weights.txt", FileMode.Create);
			StreamWriter writer = new StreamWriter(stream);
			for(int i = 0; i < inputLayer.neuronsCount; i++)  {
				for(int j = 0; j < inputLayer.neurons[i].outputsNumber; j++)  {
					writer.Write(inputLayer.neurons[i].outputs[j].value+"#");
				}
			}
			writer.Write("\n");
			for(int k = 0; k < hiddenLayersCount; k++)  {
				for(int i = 0; i < hiddenLayers[k].neuronsCount; i++)  {
					for(int j = 0; j < hiddenLayers[k].neurons[i].outputsNumber; j++)  {
						writer.Write(hiddenLayers[k].neurons[i].outputs[j].value+"#");
					}
				}
				writer.Write("\n");
			}
			writer.Close();
			stream.Close();
		}

		public void OnValueChanged_SetStep(string newStep)  {
			step = System.Convert.ToDouble(newStep);
		}

		public void PropagateForward(double[] inputSet, int iter)  {
			for(int i = 0; i < inputLayer.neuronsCount; i++)  {
				inputLayer.neurons[i].value = inputSet[i];
				inputLayer.neurons[i].text.text = System.Math.Round(inputLayer.neurons[i].value, 3).ToString();
			}

			outputLayer.neurons[0].desiredValue = desiredOutput[iter];

			for(int i = 0; i < hiddenLayersCount; i++)  {
				hiddenLayers[i].PropagateForward();
			}
			outputLayer.PropagateForward();
		}

		public void PropagateBack()  {
			outputLayer.neurons[0].mistake = outputLayer.neurons[0].desiredValue - outputLayer.neurons[0].value;
			for(int i = 0; i < hiddenLayersCount; i++)  {
				hiddenLayers[i].FindMistakesInNeurons();
			}

			for(int i = 0; i < hiddenLayersCount; i++)  {
				hiddenLayers[i].GradientDescent();
			}
			outputLayer.GradientDescent();
		}

		public static double sig(double x)  {
			return 1 / (1 + System.Math.Exp(-x));
		}

		public static double dsig(double x)  {
			double s = sig(x);
			return s * (1 - s);
		}

		public static double tanh(double x)  {
			double t = System.Math.Exp(2*x);
			return (t - 1) / (t + 1);
		}

		public static double dtanh(double x)  {
			double t = tanh(x);
			return (1 + t) * (1 - t);
		}

	}

}