using System;

namespace FlowGraph
{
    public class FrameRateCounter
    {
        private long m_lastSecond;

        private int m_calls = 0;

        public FrameRateCounter()
        {

        }

        /// <summary>
        /// Current FPS
        /// </summary>
        public int FPS { get; private set; } = 0;

        /// <summary>
        /// Create a call 
        /// </summary>
        public void Call()
        {
            m_calls++;
            if(Environment.TickCount - m_lastSecond >= 1000)
            {
                FPS = m_calls;
                m_calls = 0;
                m_lastSecond = Environment.TickCount;
            }
        }
    }
}
