using System.Linq;

namespace Huffman
{
	internal class Program
	{
		private static string _sourceFilename = "Source.txt";

		static void Main(string[] args)
		{
			// Load text from file
			var bytes = File.ReadAllBytes(_sourceFilename);

			// Show content
			Pause("Text read from the file", () =>
			{
				bytes.ToList().ForEach(x =>
				{
					if (x > 128)
						return;

					Console.Write((char)x);
				}
			);
			});

			List<Item> queue = CreatePriorityQueue(bytes);

			// Show content
			Pause("Priority Queue", () =>
			{
				queue.ForEach(x =>
				{
					x.ConsoleWrite(0);
				}
			);
			});

			var root = MakeTreeFromQueue(queue);

			// Show content
			Pause("Tree", () =>
			{
				queue.ForEach(x =>
				{
					x.ConsoleWrite(0);
				}
			);
			});

			var lookup = new Dictionary<char, string>();
			MakeLookupFromTree(root, lookup, string.Empty);

			Pause("Code lookup", () =>
			{
				foreach (var key in lookup.Keys)
				{
					Console.WriteLine($"{key} - {lookup[key]}");
				}
			});


		}

		private static void MakeLookupFromTree(Item node, IDictionary<char, string> dict, string code)
		{
			if (node.Value != 0)
			{
				dict[node.Value] = code;
				return;
			}

			if(node.Left != null)
				MakeLookupFromTree(node.Left, dict, code + "0");
			if (node.Right != null)
				MakeLookupFromTree(node.Right, dict, code + "1");
		}

		private static Item MakeTreeFromQueue(List<Item> queue)
		{
			while (queue.Count > 1) {
				Item firsNode = new(0);
				Item secondNode = new(0);

				// Get two nodes with the smallest frequencies
				var minValue = queue.Min(item => item.Frequency);
				var minItems = queue.Where(item => item.Frequency == minValue).ToList();
				if(minItems.Count >= 2) {
					var count = minItems.Count;
					firsNode = minItems[minItems.Count - 1];
					secondNode = minItems[minItems.Count - 2];
				}
				else
				{
					firsNode = minItems[0];
					minValue = queue.Where(item => item.Frequency != firsNode.Frequency).Min(item => item.Frequency);
					secondNode = queue.Where(item => item.Frequency == minValue).Last();
				}

				// Create the parent Node
				var parent = new Item(0) { Frequency = firsNode.Frequency + secondNode.Frequency };
				if(firsNode.Value < secondNode.Value) {
					parent.Left = firsNode;
					parent.Right = secondNode;
				}
				else
				{
					parent.Left = secondNode;
					parent.Right = firsNode;
				}

				// Add parent node to the queue.
				queue.Add(parent);
				queue.Remove(firsNode); 
				queue.Remove(secondNode);
			}

			return queue[0];
		}

		private static List<Item> CreatePriorityQueue(byte[] bytes)
		{
			List<Item> queue = new();
			bytes.ToList().ForEach(b =>
			{
				if (b >= 128)
					return;

				var existingNode = queue.FirstOrDefault(n => n.Value == (char)b);
				if (existingNode == null)
				{
					queue.Add(new Item(b));
				}
				else
				{
					existingNode.Frequency++;
					var index = queue.IndexOf(existingNode);
					var insertIndex = index - 1;
					while (insertIndex >= 0)
					{
						if (queue[insertIndex].Frequency < existingNode.Frequency)
							insertIndex--;
						else
							break;
					}
					if (insertIndex != index)
					{
						queue.RemoveAt(index);
						queue.Insert(insertIndex + 1, existingNode);
					}
				}
			}
			);
			return queue;
		}

		private static void Pause(string message, Action action)
		{
			Console.WriteLine(message + ":");
			Console.WriteLine(new String('-', message.Length + 1));
			if (action != null)
				action();
			Console.WriteLine();
			Console.ReadKey();
			Console.WriteLine();
		}
	}
}