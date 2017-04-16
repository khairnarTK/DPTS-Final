using System;
using System.Globalization;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;

namespace DPTS.Web.Extensions
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Drop Down Picker
        /// </summary>
        /// <param name="html"></param>
        /// <param name="dayName"></param>
        /// <param name="monthName"></param>
        /// <param name="yearName"></param>
        /// <param name="beginYear"></param>
        /// <param name="endYear"></param>
        /// <param name="selectedDay"></param>
        /// <param name="selectedMonth"></param>
        /// <param name="selectedYear"></param>
        /// <param name="htmlAttributes"></param>
        /// <param name="wrapTags"></param>
        /// <returns></returns>
        public static MvcHtmlString DatePickerDropDowns(this HtmlHelper html,
            string dayName, string monthName, string yearName,
            int? beginYear = null, int? endYear = null,
            int? selectedDay = null, int? selectedMonth = null, int? selectedYear = null,
            object htmlAttributes = null, bool wrapTags = false)
        {
            var daysList = new TagBuilder("select");
            var monthsList = new TagBuilder("select");
            var yearsList = new TagBuilder("select");

            daysList.Attributes.Add("name", dayName);
            monthsList.Attributes.Add("name", monthName);
            yearsList.Attributes.Add("name", yearName);

            var htmlAttributesDictionary = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            daysList.MergeAttributes(htmlAttributesDictionary, true);
            monthsList.MergeAttributes(htmlAttributesDictionary, true);
            yearsList.MergeAttributes(htmlAttributesDictionary, true);

            var days = new StringBuilder();
            var months = new StringBuilder();
            var years = new StringBuilder();

            string dayLocale, monthLocale, yearLocale;

            dayLocale = "Day";
            monthLocale = "Month";
            yearLocale = "Year";


            days.AppendFormat("<option value='{0}'>{1}</option>", "0", dayLocale);
            for (int i = 1; i <= 31; i++)
                days.AppendFormat("<option value='{0}'{1}>{0}</option>", i,
                    (selectedDay.HasValue && selectedDay.Value == i) ? " selected=\"selected\"" : null);


            months.AppendFormat("<option value='{0}'>{1}</option>", "0", monthLocale);
            for (int i = 1; i <= 12; i++)
            {
                months.AppendFormat("<option value='{0}'{1}>{2}</option>",
                    i,
                    (selectedMonth.HasValue && selectedMonth.Value == i) ? " selected=\"selected\"" : null,
                    CultureInfo.CurrentUICulture.DateTimeFormat.GetMonthName(i));
            }


            years.AppendFormat("<option value='{0}'>{1}</option>", "0", yearLocale);

            if (beginYear == null)
                beginYear = DateTime.UtcNow.Year - 100;
            if (endYear == null)
                endYear = DateTime.UtcNow.Year;

            if (endYear > beginYear)
            {
                for (int i = beginYear.Value; i <= endYear.Value; i++)
                    years.AppendFormat("<option value='{0}'{1}>{0}</option>", i,
                        (selectedYear.HasValue && selectedYear.Value == i) ? " selected=\"selected\"" : null);
            }
            else
            {
                for (int i = beginYear.Value; i >= endYear.Value; i--)
                    years.AppendFormat("<option value='{0}'{1}>{0}</option>", i,
                        (selectedYear.HasValue && selectedYear.Value == i) ? " selected=\"selected\"" : null);
            }

            daysList.InnerHtml = days.ToString();
            monthsList.InnerHtml = months.ToString();
            yearsList.InnerHtml = years.ToString();

            if (wrapTags)
            {
                string wrapDaysList = "<span class=\"days-list select-wrapper\">" + daysList + "</span>";
                string wrapMonthsList = "<span class=\"months-list select-wrapper\">" + monthsList + "</span>";
                string wrapYearsList = "<span class=\"years-list select-wrapper\">" + yearsList + "</span>";

                return MvcHtmlString.Create(string.Concat(wrapDaysList, wrapMonthsList, wrapYearsList));
            }
            else
            {
                return MvcHtmlString.Create(string.Concat(daysList, monthsList, yearsList));
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static string FieldNameFor<T, TResult>(this HtmlHelper<T> html, Expression<Func<T, TResult>> expression)
        {
            return html.ViewData.TemplateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(expression));
        }
    }
}