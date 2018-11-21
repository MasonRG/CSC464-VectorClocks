using System;
using System.Collections.Generic;
using System.Threading;

namespace VectorClocks
{
	/// <summary>
	/// A simulated node in a distributed system. Handles doing work and sending/receiving messages.
	/// Maintains its own vector clock, and records the history of its clock for demonstration purposes.
	/// Logic for performing actions is done on a unique thread for each node.
	/// </summary>
	public class Node
	{
		private readonly int id; //used to identify this node
		private readonly int delayMillis; //how long to wait between actions for this node

		private readonly VectorClock clock; //the vector clock for this node
		private readonly object clockLock = new object(); //lock for modifying our clock (message reception susceptible to race condition between this node and sender node)

		public List<int[]> ClockHistory { get; } = new List<int[]>(); //accessor for our clock history - a convenience property for when we need to print the history to console

		public Node(int id, int numberOfNodes, int delayMillis)
		{
			this.id = id;
			this.delayMillis = delayMillis;
			clock = new VectorClock(numberOfNodes);
		}


		/// <summary>
		/// This method will be run on a separate thread.
		/// </summary>
		/// <param name="parameter">A list of NodeActions for this node to perform. Should not be passed null.</param>
		public void Run(object parameter)
		{
			//wait for the simulation to begin...
			while (!VectorClockTester.simulationStarted) ;

			//initialize the clock history with a vector of all 0's
			UpdateClockHistory(); 

			var actionList = (List<NodeAction>)parameter;
			if (parameter == null)
				return;

			foreach(var action in actionList)
			{
				switch(action)
				{
					case NodeAction.DoWork:
						DoWork();
						break;
					case NodeAction.SendMessage:
						//get receiver 'address' - randomly choose between the other nodes
						var rand = new Random();
						var to = rand.Next(0, clock.Vector.Length);
						if (to == id) to = (to + 1) % clock.Vector.Length;

						SendMessage(to);
						break;
				}

				//sleep to somewhat simulate network latency
				Thread.Sleep(delayMillis);
			}
		}

		/// <summary>
		/// Helper method to update our clock history with a new entry, which is the
		/// current clock state.
		/// </summary>
		private void UpdateClockHistory()
		{
			ClockHistory.Add((int[])clock.Vector.Clone());
		}


		/// <summary>
		/// Simulate doing work.
		/// </summary>
		public void DoWork()
		{
			lock (clockLock)
			{
				clock.Increment(id);
				UpdateClockHistory();
			}
			VectorClockTester.DidWork(id, clock);
		}

		/// <summary>
		/// Simulate sending a message: calling thread will still do the
		/// 'receiving' and will modify the other node/thread's clock. 
		/// </summary>
		/// <param name="toId">The id of the node we are sending the message to.</param>
		public void SendMessage(int toId)
		{
			lock (clockLock)
			{
				clock.Increment(id);
				UpdateClockHistory();
			}
			VectorClockTester.RelayMessage(id, toId, clock);
		}


		/// <summary>
		/// Simulate receiving a message. Sending thread also 'receives' for the 
		/// receiver node.So beware of that race condition on the receiver node's clock.
		/// </summary>
		/// <param name="senderClock">The clock of the node that sent this message.</param>
		public void ReceiveMessage(VectorClock senderClock)
		{
			lock (clockLock)
			{
				clock.Merge(senderClock, id);
				UpdateClockHistory();
			}
		}
	}
}
