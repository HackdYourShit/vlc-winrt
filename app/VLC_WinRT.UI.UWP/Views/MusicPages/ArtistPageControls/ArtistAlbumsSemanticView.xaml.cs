﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using VLC_WinRT.Helpers;
using VLC_WinRT.Model.Video;
using VLC_WinRT.ViewModels;
using VLC_WinRT.ViewModels.MusicVM;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace VLC_WinRT.UI.Legacy.Views.MusicPages.ArtistPageControls
{
    public sealed partial class ArtistAlbumsSemanticView : Grid
    {
        public ArtistAlbumsSemanticView()
        {
            this.InitializeComponent();
            this.Loaded += ArtistAlbumsSemanticView_Loaded;
        }

        private void ArtistAlbumsSemanticView_Loaded(object sender, RoutedEventArgs e)
        {
            if (DeviceTypeHelper.GetDeviceType() == DeviceTypeEnum.Xbox)
                (FindName(nameof(AlbumsArtistsListView)) as FrameworkElement).Visibility = Visibility.Visible;
            else
                (FindName(nameof(AlbumsSemanticZoom)) as FrameworkElement).Visibility = Visibility.Visible;

            if (AlbumsListView != null)
                AlbumsListView.SizeChanged += AlbumsListViewOnSizeChanged;
            Locator.MusicLibraryVM.PropertyChanged += MusicLibraryVM_PropertyChanged;
            ResponsiveTracksListView();
            ResponsiveAlbumsList();
            this.Unloaded += ArtistAlbumsSemanticView_Unloaded;
        }

        private void MusicLibraryVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MusicLibraryVM.CurrentArtist))
            {
                if (AlbumsSemanticZoom != null)
                    AlbumsSemanticZoom.IsZoomedInViewActive = true;
            }
        }

        private void ArtistAlbumsSemanticView_Unloaded(object sender, RoutedEventArgs e)
        {
            if (AlbumsListView != null)
                AlbumsListView.SizeChanged -= AlbumsListViewOnSizeChanged;
            Locator.MusicLibraryVM.PropertyChanged -= MusicLibraryVM_PropertyChanged;
        }

        private void ZoomedOutItemsWrapGrid_Loaded(object sender, RoutedEventArgs e)
        {
            if (AlbumsSemanticZoomZoomedOut != null)
                AlbumsSemanticZoomZoomedOut.SizeChanged += ZoomedOutItemsWrapGrid_SizeChanged;
            if (AlbumsArtistsListView != null)
                AlbumsArtistsListView.SizeChanged += AlbumsArtistsListView_SizeChanged;
            ResponsiveAlbumsWrapGrid();
        }

        private void AlbumsArtistsListView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ResponsiveAlbumsList();
        }

        private void AlbumsListViewOnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
        {
            ResponsiveTracksListView();
        }

        private void ZoomedOutItemsWrapGrid_SizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
        {
            ResponsiveAlbumsWrapGrid();
        }

        void ResponsiveTracksListView()
        {
            if (AlbumsListView == null) return;
            var wrapGrid = AlbumsListView.ItemsPanelRoot as ItemsWrapGrid;
            if (wrapGrid == null) return;
            if (AlbumsSemanticZoom.IsZoomedInViewActive)
                TemplateSizer.ComputeAlbumTracks(ref wrapGrid, AlbumsListView.ActualWidth - wrapGrid.Margin.Left - wrapGrid.Margin.Right);
        }

        void ResponsiveAlbumsWrapGrid()
        {
            if (AlbumsSemanticZoomZoomedOut == null) return;
            var wrapGridZoomedOut = AlbumsSemanticZoomZoomedOut.ItemsPanelRoot as ItemsWrapGrid;
            if (wrapGridZoomedOut == null) return;
            TemplateSizer.ComputeAlbums(wrapGridZoomedOut, AlbumsSemanticZoomZoomedOut.ActualWidth - wrapGridZoomedOut.Margin.Left - wrapGridZoomedOut.Margin.Right);
        }

        void ResponsiveAlbumsList()
        {
            if (AlbumsArtistsListView == null) return;
            var wrapGrid = AlbumsArtistsListView.ItemsPanelRoot as ItemsWrapGrid;
            if (wrapGrid == null) return;
            TemplateSizer.ComputeAlbums(wrapGrid, AlbumsArtistsListView.ActualWidth - wrapGrid.Margin.Left - wrapGrid.Margin.Right);
        }

        private void SemanticZoom_ViewChangeCompleted(object sender, SemanticZoomViewChangedEventArgs e)
        {
            AlbumsSemanticZoomZoomedOut.ItemsSource = GroupAlbums?.View?.CollectionGroups;
        }



        public Visibility FloatingActionButtonVisibility
        {
            get { return (Visibility)GetValue(FloatingActionButtonVisibilityProperty); }
            set { SetValue(FloatingActionButtonVisibilityProperty, value); }
        }

        public static readonly DependencyProperty FloatingActionButtonVisibilityProperty =
            DependencyProperty.Register(nameof(FloatingActionButtonVisibility), typeof(Visibility), typeof(ArtistAlbumsSemanticView), new PropertyMetadata(null, PropertyChangedCallback));


        private static void PropertyChangedCallback(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var that = (ArtistAlbumsSemanticView)obj;
            that.SetFabVisibility();
        }

        public void SetFabVisibility()
        {
            FloatingActionButton.Visibility = FloatingActionButtonVisibility;
        }
    }
}
