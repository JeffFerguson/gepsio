﻿using JeffFerguson.Gepsio.Xml.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Schema;

namespace JeffFerguson.Gepsio.Xml.Implementation.SystemXml
{
    internal class Schema : ISchema
    {
        private XmlSchema thisSchema;
        private List<IQualifiedName> thisNamespaceList;

        internal XmlSchema XmlSchema
        {
            get
            {
                return thisSchema;
            }
        }

        public List<IQualifiedName> Namespaces
        {
            get
            {
                if (thisNamespaceList == null)
                {
                    var newList = new List<IQualifiedName>();
                    var xmlNamespaces = thisSchema.Namespaces.ToArray();
                    newList.Capacity = xmlNamespaces.Length;
                    foreach (var entry in xmlNamespaces)
                    {
                        var newItem = new QualifiedName(entry);
                        newList.Add(newItem);
                    }
                    thisNamespaceList = newList;
                }
                return thisNamespaceList;
            }
        }
        public IEnumerable< ISchemaAppInfo > AppInfo => thisSchema
            .Items.OfType< XmlSchemaAnnotation >( )
            .SelectMany( xmlSchemaAnnotation => xmlSchemaAnnotation
                .Items.OfType< XmlSchemaAppInfo >( )
                .Select( xmlSchemaAppInfo => new SchemaAppInfo( xmlSchemaAppInfo ) )
            );

        /// <summary>
        /// The source URI to the schema.
        /// </summary>
        public string SourceUri => thisSchema.SourceUri;

        public Schema()
        {
            thisSchema = null;
            thisNamespaceList = null;
        }

        internal Schema(XmlSchema schema)
        {
            thisSchema = schema;
            thisNamespaceList = null;
        }

        public bool Read(string path)
        {            
            try
            {
                var schemaReader = XmlTextReader.Create(path);
                thisSchema = XmlSchema.Read(schemaReader, null);
                return true;
            }
            catch(XmlSchemaException)
            {
                return false;
            }
        }

        public bool Read(Stream sourceStream, string sourceUri)
        {
            try
            {
                var schemaReader = XmlTextReader.Create(sourceStream);
                thisSchema = XmlSchema.Read(schemaReader, null);
                thisSchema.SourceUri = sourceUri;
                return true;
            }
            catch (XmlSchemaException)
            {
                return false;
            }
        }
    }
}
