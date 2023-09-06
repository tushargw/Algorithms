namespace Huffman
{
	public class Item
	{
		public Item(byte b)
		{
			Value = (char)b;
			Frequency = 1;
		}

		public char Value { get; set; }
		public int Frequency { get; set; }
		public Item? Next { get; set; }
		public Item? Previous { get; set; }
		public Item? Left { get; set; }
		public Item? Right { get; set; }

		internal void ConsoleWrite(int level)
		{
			var output = $"{new String(' ', level)}{Value}({Frequency})";
			Console.WriteLine(output);

			Left?.ConsoleWrite(level + 4);
			Right?.ConsoleWrite(level + 4);
		}
	}
}
