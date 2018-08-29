using System.Collections.Generic;
using UnityEngine;

namespace Controls
{
    public class Tower
    {
        private readonly Stack<PieControl> stack = new Stack<PieControl>();

        public int Height => stack.Count;
        public Vector3 Position { get; set; } = Vector3.zero;
        public Vector3 ApexPosition => new Vector3(Position.x, Position.y + Height, Position.z);

        public PieControl Pop()
        {
            return stack.Count > 0 ? stack.Pop() : null;
        }

        public PieControl Peek()
        {
            return stack.Count > 0 ? stack.Peek() : null;
        }

        public void Push(PieControl pie)
        {
            stack.Push(pie);
        }
    }
}
