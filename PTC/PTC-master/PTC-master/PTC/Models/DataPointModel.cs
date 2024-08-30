using System.Runtime.Serialization;

namespace PTC.Models
{
    [DataContract]
    public class DataPoint
    {
        public DataPoint(string label, double data, string backgroundColor)
        {
            this.label = label;
            this.data = data;
            this.backgroundColor = backgroundColor;
        }

        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "label")]
        public string label = "";

        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "data")]
        public double? data = null;

        [DataMember(Name = "backgroundColor")]
        public string backgroundColor = null;
    }
}
