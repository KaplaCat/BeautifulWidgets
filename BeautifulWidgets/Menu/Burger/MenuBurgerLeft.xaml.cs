using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BeautifulWidgets.Menu.Burger
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuBurgerLeft : ContentView
    {
        const float FlyoutCornerRadius = 10f;

        bool _isFlyoutOpen = false;
        double _scale;
        uint _flyoutSpeed = 200;
        double _pagePositionX;
        double _flyoutTranslationX;

        public static readonly BindableProperty MenuContentProperty = BindableProperty.Create(
                nameof(MenuContent), 
                typeof(ContentView),
                typeof(MenuBurgerLeft), 
                default(ContentView),
                BindingMode.TwoWay);
        public static readonly BindableProperty PageContentProperty = BindableProperty.Create(
                nameof(PageContent), 
                typeof(ContentView), 
                typeof(MenuBurgerLeft),
                default(ContentView),
                BindingMode.TwoWay);

        public ContentView MenuContent
        {
            get { return (ContentView)GetValue(MenuContentProperty); }
            set { SetValue(MenuContentProperty, value); }
        }
        public ContentView PageContent
        {
            get { return (ContentView)GetValue(PageContentProperty); }
            set { SetValue(PageContentProperty, value); }
        }

        public MenuBurgerLeft()
        {
            InitializeComponent();
            _scale = MainContent.Scale;
            _pagePositionX = MainContent.TranslationX;

            MainContent.SizeChanged += OnMainContentSizeChanged;
            Flyout.Opacity = 0;
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName.Equals(MenuContentProperty.PropertyName))
                menuContent.Content = MenuContent;
            else if (propertyName.Equals(PageContentProperty.PropertyName))
                pageContent.Content = PageContent;
        }

        private void MenuContent_ChildAdded(object sender, ElementEventArgs e)
        {
            throw new NotImplementedException();
        }

        void OnMainContentSizeChanged(object sender, EventArgs e)
        {
            MainContent.SizeChanged -= OnMainContentSizeChanged;
            _flyoutTranslationX = MainContent.Width * .75;

            if (Flyout.Children.Count == 1 && Flyout.Children[0] is Layout menuPage)
            {
                var flyoutPadding = Flyout.Width - (Flyout.Width * .8);
                (Flyout.Children[0] as Layout).Padding = new Thickness(0, 0, flyoutPadding, 0);
            }
        }

        void OnToggleMenu(object sender, EventArgs e)
        {
            ToggleFlyout();
        }

        void FlyoutClose(object sender, SwipedEventArgs e)
        {
            if (_isFlyoutOpen)
                ToggleFlyout();
        }

        void FlyoutOpen(object sender, SwipedEventArgs e)
        {
            if (!_isFlyoutOpen)
                ToggleFlyout();
        }

        void ToggleFlyout()
        {
            if (_isFlyoutOpen)
            {
                MainContent.ScaleTo(_scale, _flyoutSpeed);
                MainContent.TranslateTo(_pagePositionX, Flyout.TranslationY, _flyoutSpeed);
                Flyout.FadeTo(0, _flyoutSpeed);
                MainContent.CornerRadius = 0;
            }
            else
            {
                MainContent.ScaleTo(_scale * .9, _flyoutSpeed);
                MainContent.TranslateTo(Flyout.TranslationX + _flyoutTranslationX, Flyout.TranslationY, _flyoutSpeed);
                Flyout.FadeTo(1, _flyoutSpeed);
                MainContent.CornerRadius = FlyoutCornerRadius;
            }

            _isFlyoutOpen = !_isFlyoutOpen;
        }

       
    }
}