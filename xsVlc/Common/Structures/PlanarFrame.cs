using System;

namespace xsVlc.Common.Structures
{
    public struct PlanarFrame
    {
        public PlanarFrame(IntPtr[] planes, int[] lenghts) : this()
        {
            if (planes.Length != lenghts.Length)
            {
                throw new ArgumentException("Number of planes must be equal to lenghts array");
            }

            Planes = planes;
            Lenghts = lenghts;
        }

        public IntPtr[] Planes { get; set; }
        public int[] Lenghts { get; set; }
    }
}
