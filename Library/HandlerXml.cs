using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;

namespace Library
{
    public class HandlerXml
    {
        public HandlerXml(string xmlFilePath, string xsdFilePath)
        {
            XmlFilePath = xmlFilePath;
            XsdFilePath = xsdFilePath;
        }

        public HandlerXml(string xmlFilePath)
        {
            XmlFilePath = xmlFilePath;
        }

        public string XmlFilePath { get; set; }
        public string XsdFilePath { get; set; }
        public string ValidationMessage { get; set; }
        private bool isValid;

        public bool ValidateXml()
        {
            isValid = true;
            ValidationMessage = "Documento válido.";

            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(XmlFilePath);
                ValidationEventHandler evento = new ValidationEventHandler(trataEvento);
                doc.Schemas.Add(null, XsdFilePath);
                doc.Validate(evento);
            }
            catch (Exception e)
            {
                isValid = false;
                ValidationMessage = "Documento inválido." + e.Message;
            }

            return isValid;
        }

        private void trataEvento(object sender, ValidationEventArgs e)
        {
            isValid = false;

            switch (e.Severity)
            {
                case XmlSeverityType.Error:
                    ValidationMessage = "Documento inválido. Error. " + e.Message;
                    break;
                case XmlSeverityType.Warning:
                    ValidationMessage = "Documento inválido. Warning.  " + e.Message;
                    break;
                default:
                    break;
            }
        }
    }
}
