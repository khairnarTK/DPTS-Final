using DPTS.Domain.Entities;
using DPTS.Web.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace DPTS.Web.Helpers
{
    public static class HtmlHelperExtensions
    {
        public static AssetsHelper Assets(this HtmlHelper htmlHelper)
        {
            return AssetsHelper.GetInstance(htmlHelper);
        }
        public static MvcHtmlString DeleteConfirmation<T>(this HtmlHelper<T> helper, string buttonsSelector) where T : BaseEntity
        {
            return DeleteConfirmation(helper, "", buttonsSelector);
        }

        public static MvcHtmlString DeleteConfirmation<T>(this HtmlHelper<T> helper, string actionName,
            string buttonsSelector) where T : BaseEntity
        {
            if (String.IsNullOrEmpty(actionName))
                actionName = "Delete";

            var modalId = MvcHtmlString.Create(helper.ViewData.ModelMetadata.ModelType.Name.ToLower() + "-delete-confirmation")
                .ToHtmlString();

            var deleteConfirmationModel = new DeleteConfirmationModel
            {
                Id = helper.ViewData.Model.Id,
                ControllerName = helper.ViewContext.RouteData.GetRequiredString("controller"),
                ActionName = actionName,
                WindowId = modalId
            };

            var window = new StringBuilder();
            window.AppendLine(string.Format("<div id='{0}' class=\"modal fade\"  tabindex=\"-1\" role=\"dialog\" aria-labelledby=\"{0}-title\">", modalId));
            window.AppendLine(helper.Partial("Delete", deleteConfirmationModel).ToHtmlString());
            window.AppendLine("</div>");

            window.AppendLine("<script>");
            window.AppendLine("$(document).ready(function() {");
            window.AppendLine(string.Format("$('#{0}').attr(\"data-toggle\", \"modal\").attr(\"data-target\", \"#{1}\")", buttonsSelector, modalId));
            window.AppendLine("});");
            window.AppendLine("</script>");

            return MvcHtmlString.Create(window.ToString());
        }
    }

    public class AssetsHelper
    {
        public static AssetsHelper GetInstance(HtmlHelper htmlHelper)
        {
            var instanceKey = "48F2764A-6898-450E-99F8-1EF0B912ED03";

            var context = htmlHelper.ViewContext.HttpContext;
            if (context == null) return null;

            var assetsHelper = (AssetsHelper)context.Items[instanceKey];

            if (assetsHelper == null)
                context.Items.Add(instanceKey, assetsHelper = new AssetsHelper());

            return assetsHelper;
        }

        public ItemRegistrar Styles { get; private set; }
        public ItemRegistrar Scripts { get; private set; }
        public ItemRegistrar InlineScripts { get; private set; }
        public ItemRegistrar InlineStyles { get; private set; }

        public AssetsHelper()
        {
            Styles = new ItemRegistrar(ItemRegistrarFormatters.StyleFormat);
            Scripts = new ItemRegistrar(ItemRegistrarFormatters.ScriptFormat);
            InlineScripts = new ItemRegistrar(ItemRegistrarFormatters.InlineScripts);
            InlineStyles = new ItemRegistrar(ItemRegistrarFormatters.InlineStyles);
        }
    }

    public class ItemRegistrar
    {
        private readonly string _format;
        private readonly IList<string> _items;

        public ItemRegistrar(string format)
        {
            _format = format;
            _items = new List<string>();
        }

        public ItemRegistrar Add(string url)
        {
            if (!_items.Contains(url))
                _items.Add(url);

            return this;
        }

        public IHtmlString Render()
        {
            var sb = new StringBuilder();

            foreach (var item in _items)
            {
                var fmt = string.Format(_format, item);
                sb.AppendLine(fmt);
            }

            return new HtmlString(sb.ToString());
        }
    }

    public class ItemRegistrarFormatters
    {
        public const string StyleFormat = "<link href=\"{0}\" rel=\"stylesheet\" type=\"text/css\" />";
        public const string ScriptFormat = "<script src=\"{0}\" type=\"text/javascript\"></script>";
        public const string InlineScripts = "<script type=\"text/javascript\">{0}</script>";
        public const string InlineStyles = "<style type=\"text/css\">{0}</style>";
    }
}