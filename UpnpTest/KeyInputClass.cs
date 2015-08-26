using RestSharp.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpnpTest
{
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategory("code")]
    [System.Xml.Serialization.XmlType(AnonymousType = true)]
    [System.Xml.Serialization.XmlRoot(ElementName = "command", Namespace = "", IsNullable = false)]
    [SerializeAs(Name = "command")]
    class KeyInputClass
    {
        /// <summary>
        /// The Session ID (Pairing Code) of the TV
        /// </summary>
        public string session { get; set; }

        /// <summary>
        /// The type of entry sent. Typically HandleKeyInput
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// The Key Code
        /// </summary>
        public int value { get; set; }
    }
}
