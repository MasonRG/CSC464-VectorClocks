using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VectorClocks
{
	/// <summary>
	/// The tester for running, evaluating, and printing results of our VectorClock demonstration.
	/// </summary>
	public class VectorClockTester
	{
		const int NUM_NODES = 3;
		readonly static Node[] nodes = new Node[NUM_NODES];
		readonly static Thread[] threads = new Thread[NUM_NODES];

		public static bool simulationStarted = false; //Flag to ensure our threads don't begin until they are all instantiated


		/// <summary>
		/// Relays a message from a sender node to a receiver node.
		/// We intercept and relay the message so we can print the event to console.
		/// </summary>
		/// <param name="from">The nodeId sending the message.</param>
		/// <param name="to">The nodeId receiving the message.</param>
		/// <param name="senderClock">The clock belonging to the sending node.</param>
		public static void RelayMessage(int from, int to, VectorClock senderClock)
		{
			nodes[to].ReceiveMessage(senderClock);
			Console.WriteLine(string.Format("[{0}] {1} -> Message to {2}", Utils.CurrentTimeStamp, Utils.IdToName(from), Utils.IdToName(to)));
		}

		/// <summary>
		/// When called, indicates that a node did work so we can print the event to console.
		/// </summary>
		/// <param name="from">The nodeId that did the work.</param>
		/// <param name="senderClock">The clock of the node that did the work.</param>
		public static void DidWork(int from, VectorClock senderClock)
		{
			Console.WriteLine(string.Format("[{0}] {1} -> Did Work", Utils.CurrentTimeStamp, Utils.IdToName(from)));
		}

		

		static void Main(string[] args)
		{
			//Generating and printing NodeAction lists
			PrintBreak();
			Console.WriteLine("Generating node action lists...\n");
			List<List<NodeAction>> actions = new List<List<NodeAction>>();

			for (int i = 0; i < NUM_NODES; i++)
			{
				var list = Utils.GenerateRandomActionList();
				actions.Add(list);
				Console.WriteLine(Utils.GetFormattedNodeActionList(list, i));
				Thread.Sleep(100); ///for randomness in the generator
			}
			PrintBreak();


			//Running the simulation
			Console.WriteLine("Simulation Start\n");
			for (int i = 0; i < NUM_NODES; i++)
			{
				nodes[i] = new Node(i, NUM_NODES, 400);
				threads[i] = new Thread(new ParameterizedThreadStart(nodes[i].Run));
				threads[i].Start(actions[i]);
			}

			simulationStarted = true;

			foreach (var thread in threads)
			{
				thread.Join();
			}

			Console.WriteLine("\nSimulation Complete");
			PrintBreak();


			//Printing vector clock history for each node
			Console.WriteLine("Vector Clock History\n");

			for(int i = 0; i < NUM_NODES; i++)
			{
				Console.WriteLine(Utils.GetFormattedNodeClockHistory(i, nodes[i]));
			}

			PrintBreak();
			Console.ReadLine();
		}


		/// <summary>
		/// Small helper to format output sections.
		/// </summary>
		private static void PrintBreak()
		{
			Console.WriteLine("\n-----------------------\n");
		}
	}
}
