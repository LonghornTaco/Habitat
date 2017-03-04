namespace Sitecore.Foundation.Orm.Model
{
    using System;
    using System.Linq.Expressions;
    using System.Web;

    public interface IPropertyBuilder
    {
        IHtmlString BuildHtmlString<TModel>(TModel contentItem, Expression<Func<TModel, object>> field) where TModel : IContentBase;
        IHtmlString BuildHtmlString<TModel>(TModel contentItem, Expression<Func<TModel, object>> field, object parameters) where TModel : IContentBase;
    }
}
