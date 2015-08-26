using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpnpTest
{
    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public class AuthorizationClass
    {
        /// <summary>
        /// The type of Command Sent. typically AuthReq
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// If empty allows for the TV to start pairing. If the pairing code is entered the TV returns a session ID that needs to be used from now on
        /// </summary>
        public uint value { get; set; }
    }
}
