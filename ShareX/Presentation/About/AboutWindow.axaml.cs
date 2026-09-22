#region License Information (GPL v3)

/*
    ShareX - A program that allows you to take screenshots and share any file type
    Copyright (c) 2007-2026 ShareX Team

    This program is free software; you can redistribute it and/or
    modify it under the terms of the GNU General Public License
    as published by the Free Software Foundation; either version 2
    of the License, or (at your option) any later version.
*/

#endregion License Information (GPL v3)

#nullable enable

using Avalonia.Controls;
using ShareX.AvaloniaUI.Theming;
using ShareX.HelpersLib;
using ShareX.Localization;
using System;
using System.Collections.Generic;
using System.IO;
using AvaloniaBitmap = Avalonia.Media.Imaging.Bitmap;
using DrawingBitmap = System.Drawing.Bitmap;

namespace ShareX;

public partial class AboutWindow : Window
{
    private readonly AvaloniaBitmap _logoBitmap;
    private readonly ShareXClickerControl _clicker;

    public AboutWindow()
    {
        InitializeComponent();
        RequestedThemeVariant = ThemeManager.GetCurrentTheme();

        using DrawingBitmap logo = ShareXResources.Logo;
        using Stream logoStream = logo.GetStream();
        logoStream.Position = 0;
        _logoBitmap = new AvaloniaBitmap(logoStream);
        LogoImage.Source = _logoBitmap;
        _clicker = new ShareXClickerControl(new ShareXClickerState(),
            LogoImage, ClickerOverlay, ClickerParticleOverlay, SectionsViewer, ClickerHost, AboutPanel.Background,
            [BrandText, ProductNameText, AboutDetailsPanel, CopyrightText]);

        ProductNameText.Text = Program.Title;
        CopyrightText.Text = Strings.AboutWindow_Copyright;
        SectionsControl.ItemsSource = CreateSections();


        Opened += OnOpened;
        Closed += (_, _) =>
        {
            _clicker.Dispose();
            _logoBitmap.Dispose();
        };
    }

    private void OnOpened(object? sender, EventArgs e)
    {
        Activate();
    }

    private void OnLinkClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (sender is HyperlinkButton { DataContext: AboutLinkItem item })
        {
            URLHelpers.OpenURL(item.Uri.AbsoluteUri);
        }
    }

    private static IReadOnlyList<AboutSection> CreateSections()
    {
        return
        [
            new AboutSection(Strings.AboutWindow_Team,
            [
                Link("Jaex", Links.Jaex),
                Link("McoreD", Links.McoreD)
            ]),
            new AboutSection(Strings.AboutWindow_Links,
            [
                Link(Strings.AboutWindow_ProjectPage, Links.GitHub),
                Link("ShareX (upstream)", Links.UpstreamGitHub),
                Link(Strings.AboutWindow_PrivacyPolicy, Links.License)
            ])
        ];
    }

    private static AboutLinkItem Link(string label, string url) => new(label, url, new Uri(url));
}

public sealed record AboutSection(string Title, IReadOnlyList<AboutLinkItem> Items);

public sealed record AboutLinkItem(string Label, string DisplayText, Uri Uri);
