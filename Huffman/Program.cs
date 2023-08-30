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

			// Create priority queue
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
						queue.Insert(insertIndex+1, existingNode);
					}
				}
			}
			);

			// Show content
			Pause("Priority Queue", () =>
			{
				queue.ForEach(x =>
				{
					x.ConsoleWrite(0, true);
				}
			);
			});
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