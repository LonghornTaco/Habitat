namespace Sitecore.Foundation.Orm.Model
{
    using System;
    using System.Linq.Expressions;
    using System.Web;
    using Glass.Mapper.Sc;

    public class PropertyBuilder : IPropertyBuilder
    {
        protected readonly IGlassHtml GlassHtml;

        public PropertyBuilder(IGlassHtml glassHtml)
        {
            this.GlassHtml = glassHtml;
        }

        public IHtmlString BuildHtmlString<TModel>(TModel contentItem, Expression<Func<TModel, object>> field) where TModel : IContentBase
        {
            return new HtmlString(this.GlassHtml.Editable(contentItem, field)); ;
        }

        public IHtmlString BuildHtmlString<TModel>(TModel contentItem, Expression<Func<TModel, object>> field, object parameters) where TModel : IContentBase
        {
            return new HtmlString(this.GlassHtml.Editable(contentItem, field, parameters));
        }
    }
}