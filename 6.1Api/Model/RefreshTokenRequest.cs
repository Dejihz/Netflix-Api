using System.Xml.Serialization;

namespace project6._1Api.Model
{
    [XmlRoot(ElementName = "RefreshTokenRequest")]
    public class RefreshTokenRequest
    {
        [XmlElement(ElementName = "RefreshToken")]
        public string RefreshToken { get; set; }
    }
}
