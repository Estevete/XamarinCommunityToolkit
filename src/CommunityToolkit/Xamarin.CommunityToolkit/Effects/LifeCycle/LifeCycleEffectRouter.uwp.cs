﻿using System;
using System.Linq;
using Windows.UI.Xaml;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.CommunityToolkit.UWP.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportEffect(typeof(LifeCycleEffectRouter), nameof(LifeCycleEffect))]

namespace Xamarin.CommunityToolkit.UWP.Effects
{
	public class LifeCycleEffectRouter : PlatformEffect
	{
		FrameworkElement nativeView;
		LifeCycleEffect lifeCycleEffect;

		protected override void OnAttached()
		{
			lifeCycleEffect = Element.Effects.OfType<LifeCycleEffect>().FirstOrDefault();

			_ = lifeCycleEffect ?? throw new ArgumentNullException($"The effect {nameof(LifeCycleEffect)} can't be null.");

			nativeView = Control ?? Container;

			nativeView.Loaded += OnNativeViewLoaded;
			nativeView.Unloaded += OnNativeViewUnloaded;
		}

		void OnNativeViewLoaded(object sender, RoutedEventArgs e) => lifeCycleEffect.RaiseLoadedEvent(Element);

		void OnNativeViewUnloaded(object sender, RoutedEventArgs e)
		{
			lifeCycleEffect.RaiseUnloadedEvent(Element);
			nativeView.Unloaded -= OnNativeViewUnloaded;
			lifeCycleEffect = null;
			nativeView = null;
		}

		protected override void OnDetached() => nativeView.Loaded -= OnNativeViewLoaded;
	}
}
