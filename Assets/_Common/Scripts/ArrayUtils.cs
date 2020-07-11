using UnityEngine;
using System.Collections.Generic;

namespace Com.Github.Knose1.Common
{
	public static class ArrayUtils
	{

		public static List<T> Clone<T>(this List<T> list) => new List<T>(list);
		public static List<T> Shuffle<T>(this List<T> list)
		{
			list.Sort(ShuffleComparer);
			return list;
		}

		private static int ShuffleComparer<T>(T _, T __)
		{
			return Random.Range(-1, 2);
		}
	}
}
