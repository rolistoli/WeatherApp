namespace WeatherApp.Helpers;

public static class PageTransitionAnimations
{
    public static async Task PlayEntranceAsync(VisualElement element)
    {
        element.AbortAnimation(nameof(PlayEntranceAsync));
        element.Opacity = 0;
        element.TranslationY = 18;
        element.Scale = 0.98;

        await Task.WhenAll(
            element.FadeTo(1, 220, Easing.CubicOut),
            element.TranslateTo(0, 0, 260, Easing.CubicOut),
            element.ScaleTo(1, 260, Easing.CubicOut));
    }
}
