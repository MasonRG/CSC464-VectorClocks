using System;

namespace VectorClocks
{
	/// <summary>
	/// Vector clock used to track causal relationships between nodes in the simulated
	/// distributed system.
	/// </summary>
	public class VectorClock
	{
		//The timestamp for the clock
		public int[] Vector { get; private set; }

		public VectorClock(int size)
		{
			this.Vector = new int[size];
		}


		/// <summary>
		/// Increment the clock for the given nodeId (the position in the vector).
		/// </summary>
		/// <param name="nodeId">The id of the node to increment in the clock.</param>
		public void Increment(int nodeId)
		{
			Vector[nodeId]++;
		}


		/// <summary>
		/// Merge two clocks as the result of receiving a message. Merge is performed by
		/// 1. for our node: incrementing the value
		/// 2. for other nodes: for each position in the vector, choosing the maximum of our own clock and the other clock 
		/// </summary>
		/// <param name="other">The other clock to merge with ours.</param>
		/// <param name="selfId">The id of the node that our clock belongs to.</param>
		public void Merge(VectorClock other, int selfId)
		{
			for(int i = 0; i < Vector.Length; i++)
			{
				if (i == selfId)
				{
					Increment(selfId);
				}
				else
				{
					Vector[i] = Math.Max(other.Vector[i], Vector[i]);
				}
			}
		}
	}
}
