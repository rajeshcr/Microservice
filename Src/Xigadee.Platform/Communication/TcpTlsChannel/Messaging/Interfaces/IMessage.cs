﻿#region Copyright
// Copyright Hitachi Consulting
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//    http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Net.Security;
using System.Security.Authentication;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using System.ComponentModel;

namespace Xigadee
{
    /// <summary>
    /// This interface is used by objects that support the messaging format.
    /// </summary>
    public interface IMessage : ISupportInitialize, IMessageStreamLoad
    {
        bool SupportsInitialization { get; }

        bool CanRead { get; }
        bool CanWrite { get; }

        long Position { get;set; }
        long Length { get; }
        long? BodyLength { get; }

        int Read(byte[] buffer, int offset, int count);
        int ReadByte();

        int Write(byte[] buffer, int offset, int count);
        int WriteByte(byte value);

        /// <summary>
        /// This property indicates the message direction.
        /// </summary>
        MessageDirection Direction { get;}

        /// <summary>
        /// This property indicates whether this section signals the end of the message.
        /// </summary>
        bool IsTerminator { get; }

        string DebugString { get; }

        byte[] ToArray();
        byte[] ToArray(bool copy);
    }
}
