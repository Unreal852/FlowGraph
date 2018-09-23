using System;

namespace FlowGraph
{
    public partial class Graph
    {
        private int m_debugCalls = 0;
        private int m_debugLastCall;

        private long m_debugBackgroundRenderTime;
        private long m_debugElementsRenderTime;
        private long m_debugTotalRenderTime;

        /// <summary>
        /// Current frames per second
        /// </summary>
        public int FPS { get; private set; }

        /// <summary>
        /// The background render time in milliseconds
        /// </summary>
        public double BackgroundRenderTime { get; private set; }

        /// <summary>
        /// The elements render time ( this apply to all nodes and their child ex: items, connectors etc ) in milliseconds
        /// </summary>
        public double ElementsRenderTime { get; private set; }

        /// <summary>
        /// The total render time in milliseconds
        /// </summary>
        public double TotalRenderTime { get; private set; }

        /// <summary>
        /// Debug call
        /// </summary>
        private void DebugCall()
        {
            m_debugCalls++;
            if (Environment.TickCount - m_debugLastCall >= 1000)
            {
                FPS = m_debugCalls;
                BackgroundRenderTime = TimeSpan.FromTicks(m_debugBackgroundRenderTime).TotalMilliseconds;
                ElementsRenderTime = TimeSpan.FromTicks(m_debugElementsRenderTime).TotalMilliseconds;
                TotalRenderTime = TimeSpan.FromTicks(m_debugTotalRenderTime).TotalMilliseconds;
                m_debugCalls = 0;
                m_debugLastCall = Environment.TickCount;
            }
        }

    }
}
