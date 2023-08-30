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

		internal void ConsoleWrite(int level, bool newLine)
		{
			var quote = level > 0 ? "'" : "";
			var output = $"{new String(' ', level)}{Value}({Frequency})";
			if (newLine)
				Console.WriteLine(output);
			else
				Console.Write(output);

			Left?.ConsoleWrite(level + 1, false);
			Right?.ConsoleWrite(level + 1, true);
		}
	}
}
