using Biblioconecta.Models;

namespace Biblioconecta.Templates
{
    public class TutorialTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ImageFirstTemplate { get; set; } = null!;
        public DataTemplate TextFirstTemplate { get; set; } = null!;

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item.GetType() == typeof(TutorialItem))
            {
                TutorialItem? tutorial = (TutorialItem?)item;
                if (tutorial != null)
                {
                    return tutorial.ImageFirst ? ImageFirstTemplate : TextFirstTemplate;
                }
            }
            return ImageFirstTemplate;
            //throw new NotImplementedException();
        }
    }
}
