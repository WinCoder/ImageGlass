﻿/*
ImageGlass Project - Image viewer for Windows
Copyright (C) 2010 - 2022 DUONG DIEU PHAP
Project homepage: https://imageglass.org

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <https://www.gnu.org/licenses/>.

---------------------
This source code is based on Christopher Morgan's NamedPipes project:
Url: https://www.codeproject.com/Articles/810030/IPC-with-Named-Pipes
License: CPOL, http://www.codeproject.com/info/cpol10.aspx
---------------------
*/
namespace ImageGlass.Base.NamedPipes;

using System;
using System.IO.Pipes;
using System.Text;
using System.Threading;


/// <summary>
/// A simple named-pipe server.
/// </summary>
public class PipeServer : IDisposable
{
    private CancellationTokenSource _cancellationTokenSource;


    #region IDisposable Disposing

    public bool IsDisposed { get; private set; } = false;

    protected virtual void Dispose(bool disposing)
    {
        if (IsDisposed)
            return;

        if (disposing)
        {
            // Free any other managed objects here.
            Stop();

            _cancellationTokenSource.Dispose();
            ServerStream.Dispose();
        }

        // Free any unmanaged objects here.
        IsDisposed = true;
    }

    public virtual void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~PipeServer()
    {
        Dispose(false);
    }

    #endregion


    #region Properties & Events

    /// <summary>
    /// Gets the named-pipe server stream.
    /// </summary>
    public NamedPipeServerStream ServerStream { get; private set; }


    /// <summary>
    /// Gets the name of the <see cref="PipeServer"/>.
    /// </summary>
    public string PipeName { get; init; }


    /// <summary>
    /// Gets the tag number of the <see cref="PipeServer"/>. Default value is <c>0</c>.
    /// </summary>
    public int TagNumber { get; set; } = 0;


    /// <summary>
    /// Occurs when a message is received from the named pipe.
    /// </summary>
    public event EventHandler<MessageReceivedEventArgs> MessageReceived;


    /// <summary>
    /// Occurs when the client disconnected.
    /// </summary>
    public event EventHandler<DisconnectedEventArgs> ClientDisconnected;

    #endregion


    /// <summary>
    /// Initializes a new instance of the <see cref="PipeServer"/> class.
    /// </summary>
    /// <param name="pipeName">The name of the pipe.</param>
    /// <param name="pipeDirection">Determines the direction of the pipe.</param>
    public PipeServer(string pipeName, PipeDirection pipeDirection, int tagNumber = 0)
    {
        PipeName = pipeName;
        TagNumber = tagNumber;

        ServerStream = new NamedPipeServerStream(
            PipeName,
            pipeDirection,
            1,
            PipeTransmissionMode.Message,
            PipeOptions.Asynchronous);

        _cancellationTokenSource = new CancellationTokenSource();

    }



    #region Methods

    /// <summary>
    /// Start the pipe server.
    /// </summary>
    /// <exception cref="ObjectDisposedException"></exception>
    public void Start()
    {
        Start(_cancellationTokenSource.Token);
    }


    /// <summary>
    /// Start the pipe server.
    /// </summary>
    public void Start(CancellationToken token = default)
    {
        if (IsDisposed) return;

        var state = new PipeServerState(ServerStream, token);
        ServerStream.BeginWaitForConnection(ConnectionCallback, state);
    }


    /// <summary>
    /// The connection callback.
    /// </summary>
    private void ConnectionCallback(IAsyncResult result)
    {
        if (result.AsyncState is not PipeServerState pipeServer) return;

        pipeServer.PipeServer.EndWaitForConnection(result);
        pipeServer.PipeServer.BeginRead(pipeServer.Buffer, 0, 255, ReadCallback, pipeServer);
    }


    /// <summary>
    /// The read callback.
    /// </summary>
    private void ReadCallback(IAsyncResult result)
    {
        if (result.AsyncState is not PipeServerState pipeState) return;

        var received = pipeState.PipeServer.EndRead(result);

        // disconnected
        if (received == 0 || !pipeState.PipeServer.IsConnected)
        {
            ClientDisconnected?.Invoke(this, new DisconnectedEventArgs(PipeName));
            return;
        }

        var stringData = Encoding.UTF8.GetString(pipeState.Buffer, 0, received);
        pipeState.Message.Append(stringData);

        if (pipeState.PipeServer.IsMessageComplete)
        {
            MessageReceived?.Invoke(this, new MessageReceivedEventArgs(PipeName, stringData));
            pipeState.Message.Clear();
        }

        if (!(_cancellationTokenSource.IsCancellationRequested
            || pipeState.ExternalCancellationToken.IsCancellationRequested))
        {
            if (pipeState.PipeServer.IsConnected)
            {
                pipeState.PipeServer.BeginRead(pipeState.Buffer, 0, 255, ReadCallback, pipeState);
            }
            else
            {
                pipeState.PipeServer.BeginWaitForConnection(ConnectionCallback, pipeState);
            }
        }
    }


    /// <summary>
    /// Stops the pipe server.
    /// </summary>
    public void Stop()
    {
        if (IsDisposed) return;

        _cancellationTokenSource.Cancel();

        if (ServerStream.IsConnected)
        {
            ServerStream.Disconnect();
        }
    }


    /// <summary>
    /// Waits for a client to connect.
    /// </summary>
    public async Task WaitForConnectionAsync(CancellationToken token = default)
    {
        if (IsDisposed) return;

        await ServerStream.WaitForConnectionAsync(token);
    }


    /// <summary>
    /// Sends a string to the client.
    /// </summary>
    /// <param name="value">The string to send to the server.</param>
    public async Task SendAsync(string value)
    {
        if (IsDisposed) return;

        var buffer = Encoding.UTF8.GetBytes(value);

        await ServerStream.WriteAsync(buffer);
    }

    #endregion
}