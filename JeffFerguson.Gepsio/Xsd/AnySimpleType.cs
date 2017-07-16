using JeffFerguson.Gepsio.Xml.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection; // needed for GetTypeInfo() in NETFX_CORE

namespace JeffFerguson.Gepsio.Xsd
{
    /// <summary>
    /// An encapsulation of the XML schema type "anySimpleType" as defined in the http://www.w3.org/2001/XMLSchema namespace. 
    /// </summary>
    /// <remarks>
    /// <para>
    /// This class should be considered deprecated and will most likely be removed in a future version of Gepsio. In early CTPs,
    /// Gepsio implemented its own XML schema parser, and this class was created for the implementation of the XML schema parser
    /// type system. In later CTPs, Gepsio levergaed the XML schema support already available in the .NET Framework, which rendered
    /// Gepsio's XML schema type system obsolete.
    /// </para>
    /// </remarks>
    public abstract class AnySimpleType : AnyType
    {
        private List<FacetDefinition> thisConstrainingFacetDefinitions;
        private List<Facet> thisFacets;

        /// <summary>
        /// Describes whether or not this type is a numeric type. Returns true if this type is a numeric type. Returns false if this
        /// type is not a numeric type.
        /// </summary>
        public override bool NumericType
        {
            get
            {
                Type CurrentType = GetType();
                while (CurrentType != null)
                {
                    if (CurrentType.Equals(typeof(Decimal)) == true)
                        return true;
                    if (CurrentType.Equals(typeof(Double)) == true)
                        return true;
                    if (CurrentType.Equals(typeof(Float)) == true)
                        return true;
#if NETFX_CORE
                    CurrentType = CurrentType.GetTypeInfo().BaseType;
#else
                    CurrentType = CurrentType.BaseType;
#endif
                }
                return false;
            }
        }

        /// <summary>
        /// A collection of <see cref="Facet"/> objects that apply to this type.
        /// </summary>
        public List<Facet> Facets
        {
            get
            {
                return thisFacets;
            }
        }

        internal AnySimpleType()
        {
            thisConstrainingFacetDefinitions = new List<FacetDefinition>();
            thisFacets = new List<Facet>();
            AddConstrainingFacetDefinitions();
        }

        internal AnySimpleType(INode TypeRootNode) : this()
        {
            if (TypeRootNode != null)
            {
                foreach (INode ChildNode in TypeRootNode.ChildNodes)
                    AddFacet(ChildNode);
            }
        }

        //--------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------
        internal override decimal GetValueAfterApplyingPrecisionTruncation(int PrecisionValue)
        {
            if (this.NumericType == false)
                throw new NotSupportedException();
            decimal ValueAsDecimal = Convert.ToDecimal(this.ValueAsString);
            if (PrecisionValue > 0)
            {
                string WholePart;
                string TruncationAsString;

                int DecimalPointIndex = ValueAsString.IndexOf('.');
                if (DecimalPointIndex == -1)
                    WholePart = ValueAsString;
                else
                    WholePart = ValueAsString.Substring(0, DecimalPointIndex);
                if (PrecisionValue < WholePart.Length)
                    TruncationAsString = WholePart;
                else
                {
                    StringBuilder TruncationBuilder = new StringBuilder(WholePart.Substring(0, PrecisionValue));
                    for (int AddZeroCounter = 0; AddZeroCounter < (WholePart.Length - PrecisionValue); AddZeroCounter++)
                        TruncationBuilder.Append('0');
                    TruncationAsString = TruncationBuilder.ToString();
                }
                ValueAsDecimal = Convert.ToDecimal(TruncationAsString);
            }
            return ValueAsDecimal;
        }

        //--------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------
        internal override decimal GetValueAfterApplyingDecimalsTruncation(int DecimalsValue)
        {
            if (this.NumericType == false)
                throw new NotSupportedException();
            decimal ValueAsDecimal = Convert.ToDecimal(this.ValueAsString);
            if (DecimalsValue > 0)
                ValueAsDecimal = Math.Round(ValueAsDecimal, DecimalsValue);
            return ValueAsDecimal;
        }

        //--------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------
        internal List<Facet> GetFacets(Type FacetType)
        {
            List<Facet> NewList;

            NewList = new List<Facet>();
            foreach (Facet CurrentFacet in thisFacets)
            {
                if (CurrentFacet.GetType() == FacetType)
                    NewList.Add(CurrentFacet);
            }
            return NewList;
        }

        private void AddFacet(INode FacetNode)
        {
            ValidateFacet(FacetNode);
        }

        private void ValidateFacet(INode FacetNode)
        {
            foreach (FacetDefinition CurrentFacetDefinition in thisConstrainingFacetDefinitions)
            {
                if (CurrentFacetDefinition.Name.Equals(FacetNode.Name) == true)
                {
                    ProcessFacet(CurrentFacetDefinition, FacetNode);
                    return;
                }
            }
            string MessageFormat = AssemblyResources.GetName("UnsupportedFacet");
            StringBuilder MessageBuilder = new StringBuilder();
            MessageBuilder.AppendFormat(MessageFormat, FacetNode.Name, this.GetType().Name);
            //throw new XbrlException(MessageBuilder.ToString());
        }

        private void ProcessFacet(FacetDefinition CurrentFacetDefinition, INode FacetNode)
        {
            Facet NewFacet;

            NewFacet = Facet.CreateFacet(CurrentFacetDefinition);
            foreach (IAttribute CurrentAttribute in FacetNode.Attributes)
            {
                foreach (FacetPropertyDefinition CurrentPropertyDefinition in CurrentFacetDefinition.PropertyDefinitions)
                {
                    if (CurrentAttribute.Name.Equals(CurrentPropertyDefinition.Name) == true)
                    {
                        NewFacet.AddFacetProperty(CurrentPropertyDefinition, CurrentAttribute.Value);
                        thisFacets.Add(NewFacet);
                        return;
                    }
                }
                string MessageFormat = AssemblyResources.GetName("UnsupportedFacetProperty");
                StringBuilder MessageBuilder = new StringBuilder();
                MessageBuilder.AppendFormat(MessageFormat, CurrentAttribute.Name, CurrentFacetDefinition.Name);
                //throw new XbrlException(MessageBuilder.ToString());
            }
        }

        /// <summary>
        /// Adds a constraining facet definition to this type.
        /// </summary>
        /// <param name="ConstrainingFacet">
        /// The constraining facet to add to this type.
        /// </param>
        protected void AddConstrainingFacetDefinition(FacetDefinition ConstrainingFacet)
        {
            thisConstrainingFacetDefinitions.Add(ConstrainingFacet);
        }

        /// <summary>
        /// Add constraining facet definitions to this type.
        /// </summary>
        /// <remarks>
        /// This method is a virtual method with no implementation for this class. It is expected that derived
        /// classes will override this method with functionality appropriate for the derived class.
        /// </remarks>
        protected virtual void AddConstrainingFacetDefinitions()
        {
        }
    }
}
