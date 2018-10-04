using System.Linq;
using Xamarin.Forms;
using NUnit.Framework;
using Xamarin.Forms.Internals;

namespace QuickTest
{
    public static class ElementRendering
    {
        public static string Render(this Element element)
        {
            if (!(element as VisualElement)?.IsVisible ?? false)
                return "";

            var result = "· ";// + $"<{element.GetType().Name}> ";

            var navigation = (element as ContentPage)?.Navigation;
            if (navigation != null)
                result += navigation.NavigationStack.FirstOrDefault()?.Title + " " + string.Join(" ", (element as Page).ToolbarItems.Select(t => $"[{t.Text}]"));

            var tabbedPage = element.Parent as TabbedPage;
            if (tabbedPage != null)
                result += "\n|" + string.Join("|", tabbedPage.Children.Select(p => tabbedPage.CurrentPage == p ? $"> {p.Title} <" : $" {p.Title} ")) + "|";

            var automationId = (element as VisualElement)?.AutomationId;
            if (automationId != null)
                result += $"({automationId}) ";

            result += (element as ContentPage)?.Content.Render();

            result += (element as ContentView)?.Content.Render();
            result += (element as ScrollView)?.Content.Render();
            result += string.Join("", (element as Layout<View>)?.Children.Select(c => c.Render()) ?? new[] { "" });
            result += (element as ListView)?.Render();

            result += (element as Label)?.FormattedText?.ToString() ?? (element as Label)?.Text;
            result += (element as Button)?.Text;
            result += (element as Entry)?.Text;
            result += (element as Editor)?.Text;
            result += (element as SearchBar)?.Text;

            result += (element as TextCell)?.Text;
            result += (element as ViewCell)?.View?.Render();

            result = "\n" + result.Replace("\n", "\n  ");

            return result;
        }

        public static string Render(this ListView listView)
        {
            var result = "";
            if (listView.ItemsSource == null)
                return result;

            result += Render(listView.Header);

            if (listView.IsGroupingEnabled)
                foreach (var templatedItems in listView.TemplatedItems.Cast<TemplatedItemsList<ItemsView<Cell>, Cell>>())
                    result += Render(templatedItems);
            else
                result += Render(listView.TemplatedItems);

            result.TrimEnd('\n');
            result += Render(listView.Footer);

            return result;
        }

        static string Render(TemplatedItemsList<ItemsView<Cell>, Cell> tempatedItems)
        {
            string result = "";

            var headerCell = tempatedItems.HeaderContent;
            if (headerCell != null)
                result += Render(headerCell).TrimStart('\n') + "\n";

            foreach (var cell in tempatedItems)
                result += $"- {(cell as TextCell)?.Text + (cell as ViewCell)?.View.Render().Trim()}\n";

            return result;
        }

        static string Render(object stringBindingOrView)
        {
            if (stringBindingOrView is string)
                return stringBindingOrView.ToString() + "\n";
            if (stringBindingOrView is View)
                return (stringBindingOrView as View).Render().TrimStart('\n') + "\n";

            return stringBindingOrView?.ToString() ?? "";
        }
    }
}
