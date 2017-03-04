namespace Sitecore.Feature.CognitiveServices.Facets
{
    using System;
    using Sitecore.Analytics.Model.Framework;

    [Serializable]
    public class ContactSitecoreHello : Facet, IContactSitecoreHello
    {
        // ReSharper disable once InconsistentNaming
        public const string PERSON_ID = "PersonId";

        public ContactSitecoreHello()
        {
         //this.EnsureAttribute<IContactSitecoreHello>(PERSON_ID);
         this.EnsureAttribute<string>(PERSON_ID);
      }

        public string PersonId
        {
            get
            {
                return this.GetAttribute<string>(PERSON_ID);
            }
            set
            {
                this.SetAttribute(PERSON_ID, value);
            }
        }
    }
}