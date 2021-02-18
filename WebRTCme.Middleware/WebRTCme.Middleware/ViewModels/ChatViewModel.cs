﻿using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using WebRTCme;

namespace WebRTCme.Middleware
{
    public class ChatViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private readonly IMediaStreamService _mediaStreamService;
        private readonly ISignallingServerService _signallingServerService;
        private readonly INavigationService _navigationService;
        private IDisposable _connectionDisposer;



        public Task OnPageAppearing()
        {
            Connect();
            return Task.CompletedTask;
        }

        public Task OnPageDisappearing()
        {
            Disconnect();
            return Task.CompletedTask;
        }

        public ConnectionParameters ConnectionParameters { get; set; }

        public ObservableCollection<DataParameters> Messages { get; set; }

        private string _outgoingText = string.Empty;

        public string OutgoingText
        {
            get => _outgoingText; 
            set
            {
                _outgoingText = value;
                OnPropertyChanged();
            }
        }

        public ICommand SendCommand => new AsyncCommand(async () => 
        { 
        });


        public ConnectionRequestParameters ConnectionRequestParameters { get; set; } = new ConnectionRequestParameters();

        private void Connect()
        {
            ConnectionRequestParameters.DataChannelName = ConnectionRequestParameters.RoomName;
            _connectionDisposer = _signallingServerService.ConnectionRequest(ConnectionRequestParameters).Subscribe(
                onNext: (peerResponseParameters) =>
                {
                    switch (peerResponseParameters.Code)
                    {
                        case PeerResponseCode.PeerJoined:
                            if (peerResponseParameters.DataChannel != null)
                            {
                                var dataChannel = peerResponseParameters.DataChannel;
                                Console.WriteLine($"--------------- DataChannel: {dataChannel.Label} " +
                                    $"state:{dataChannel.ReadyState}");
                            }
                            break;

                        case PeerResponseCode.PeerLeft:
                            System.Diagnostics.Debug.WriteLine($"************* APP PeerLeft");
                            break;

                        case PeerResponseCode.PeerError:
                            System.Diagnostics.Debug.WriteLine($"************* APP PeerError");
                            break;
                    }

                },
                onError: (exception) =>
                {
                    System.Diagnostics.Debug.WriteLine($"************* APP OnError:{exception.Message}");
                },
                onCompleted: () =>
                {
                    System.Diagnostics.Debug.WriteLine($"************* APP OnCompleted");
                });

        }


        private void Disconnect()
        {
            _connectionDisposer.Dispose();
        }
    }
}