﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Storage;
using VLC_WINRT_APP.Commands.Dlna;
using VLC_WINRT_APP.Commands.RemovableDevices;
using VLC_WINRT_APP.Common;
using VLC_WINRT_APP.ViewModels.Others.VlcExplorer;

namespace VLC_WINRT_APP.ViewModels.NetworkVM
{
    public class DLNAVM : BindableBase, IDisposable
    {
        #region private props
        private FileExplorerViewModel _currentDlnaVm;

        #endregion

        #region private fields

        private ObservableCollection<FileExplorerViewModel> _dlnaVMs = new ObservableCollection<FileExplorerViewModel>();
        private DlnaClickedCommand _dlnaClickedCommand;

        #endregion

        #region public props

        public FileExplorerViewModel CurrentDlnaVm
        {
            get { return _currentDlnaVm; }
            set { SetProperty(ref _currentDlnaVm, value); }
        }

        public DlnaClickedCommand DlnaClickedCommand
        {
            get { return _dlnaClickedCommand; }
            set { _dlnaClickedCommand = value; }
        }

        #endregion

        #region public fields

        public ObservableCollection<FileExplorerViewModel> DLNAVMs
        {
            get { return _dlnaVMs; }
            set { SetProperty(ref _dlnaVMs, value); }
        }
        #endregion
        public DLNAVM()
        {
            DlnaClickedCommand = new DlnaClickedCommand();
            Initialize();
        }

        async Task Initialize()
        {
            if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                var dlnaFolder = await KnownFolders.MediaServerDevices.GetFoldersAsync();
                var tasks = new List<Task>();
                DLNAVMs.Clear();
                foreach (StorageFolder storageFolder in dlnaFolder)
                {
                    StorageFolder newFolder = storageFolder;
                    var videoLib = new FileExplorerViewModel(newFolder);
                    tasks.Add(videoLib.GetFiles());
                    DLNAVMs.Add(videoLib);
                }
                await Task.WhenAll(tasks);
                if (DLNAVMs.Count > 0)
                {
                    CurrentDlnaVm = DLNAVMs[0];
                }
            }
        }
        public void Dispose()
        {

        }
    }
}
