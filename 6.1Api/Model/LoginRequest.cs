﻿using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace project6._1Api.Model
{
    [XmlRoot(ElementName = "LoginRequest")]
    public class LoginRequest
    {
        [Required]
        [XmlElement(ElementName = "email")]
        public string Email { get; set; }
        [Required]
        [XmlElement(ElementName = "password")]
        public string Password { get; set; }
    }
}
