/* xsMedia - xsSettings
 * (c)2013 - 2025
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace xsCore.Settings.Data.Filter
{
    [Serializable]
    public class BandData
    {
        public BandData()
        {
            /* Default empty constructor */
        }

        public BandData(double frequency, double amplitude)
        {
            Frequency = frequency;
            Amplitude = amplitude;
        }

        [XmlAttribute("frequency")]
        public double Frequency { get; set; }

        [XmlAttribute("amplitude")]
        public double Amplitude { get; set; }
    }

    [Serializable]
    public class FilterEq
    {
        public FilterEq()
        {
            /* Default constructor */
            Enable = false;
            Preamp = 12;
        }

        public FilterEq(FilterEq eq)
        {
            /* Copy constructor */
            Enable = eq.Enable;
            Preamp = eq.Preamp;
            Band = new List<BandData>(eq.Band.GetRange(0, eq.Band.Count));
        }

        [XmlAttribute("enable")]
        public bool Enable { get; set; }

        [XmlAttribute("preset")]
        public int Preset { get; set; }

        [XmlAttribute("preamp")]
        public double Preamp { get; set; }

        [XmlElement("band")]
        public List<BandData> Band = new List<BandData>();
    }
}
