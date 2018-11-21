using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace VectorClocks
{
	/// <summary>
	/// Action that a node can perform in a frame.
	/// </summary>
	public enum NodeAction
	{
		DoWork,
		SendMessage
	}

	public static class Utils
	{
		private static Stopwatch stopwatch = null;

		/// <summary>
		/// First call is always 0.00 -> Returns a string formatted as seconds.milliseconds
		/// </summary>
		public static string CurrentTimeStamp
		{
			get
			{
				if (stopwatch == null) //lazy initialization
				{
					stopwatch = new Stopwatch();
					stopwatch.Start();
				}

				var time = stopwatch.ElapsedMilliseconds;
				var seconds = time / 1000;
				var millis = time % 1000;
				return string.Format("{0:0}.{1:000}", seconds, millis);
			}
		}
		

		/// <summary>
		/// Convert a nodeId to a capital letter as a string. Conversion is 0->A, 1->B, 2->C, etc.
		/// </summary>
		/// <param name="id">The id to be converted.</param>
		public static string IdToName(int id)
		{
			return ((char)(id + 65)).ToString();
		}


		/// <summary>
		/// Generate a list of NodeActions randomly (but not uniformly).
		/// </summary>
		/// <param name="min">The minimum number of actions we must have in the list.</param>
		/// <param name="max">The maximum number of actions we may have in the list.</param>
		/// <returns></returns>
		public static List<NodeAction> GenerateRandomActionList(int min = 2, int max = 5)
		{
			//odds for each action (out of 100) - chance to terminate is the leftover
			const int work_chance = 46;
			const int msg_chance = 46;
			const int end_chance = 100 - work_chance - msg_chance;

			List<NodeAction> nodeActionsList = new List<NodeAction>();
			Random rand = new Random();

			int counter = 0;
			int val = rand.Next(0, 101);
			while (val > end_chance || counter < min)
			{
				if (counter >= max)
					break;

				if (val <= msg_chance) nodeActionsList.Add(NodeAction.SendMessage);
				else nodeActionsList.Add(NodeAction.DoWork);

				val = rand.Next(0, 101);
				counter++;
			}

			return nodeActionsList;
		}


		#region Formatters
		/// <summary>
		/// Takes a list of NodeActions and formats it for printing to the console.
		/// </summary>
		/// <param name="list">The list to print.</param>
		/// <param name="nodeId">The id of the node that this list belongs to.</param>
		public static string GetFormattedNodeActionList(List<NodeAction> list, int nodeId)
		{
			string str = "{ ";
			for (int i = 0; i < list.Count; i++)
			{
				str += list[i].ToString();
				if (i < list.Count - 1)
					str += ", ";
			}
			str += " }";

			return "Node " + IdToName(nodeId) + ": " + str;
		}


		/// <summary>
		/// Formats the vector clock history of the given node as a string for printing.
		/// </summary>
		/// <param name="nodeId">The id of the node being considered.</param>
		/// <param name="node">The node whos clock history we will print.</param>
		public static string GetFormattedNodeClockHistory(int nodeId, Node node)
		{
			string str = "{ ";
			for (int i = 0; i < node.ClockHistory.Count; i++)
			{
				var stamp = node.ClockHistory[i];

				if ((i + 1) % 7 == 0) str += "\n\t  ";
				str += "[";
				for (int j = 0; j < stamp.Length; j++)
				{
					str += string.Format("{0}:{1}", IdToName(j), stamp[j]);
					if (j < stamp.Length - 1) str += "|";
				}
				str += "]";
				if (i < node.ClockHistory.Count - 1) str += ", ";
			}
			str += " }";

			return "Node " + IdToName(nodeId) + ": " + str;
		}
		#endregion
	}
}
