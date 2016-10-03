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
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xigadee
{
    /// <summary>
    /// This is the resource profile to use to signal resource restrictions.
    /// </summary>
    [DebuggerDisplay("ResourceProfile: {Id}")]
    public class ResourceProfile
    {
        /// <summary>
        /// This is the default constructor for the resource profile.
        /// </summary>
        /// <param name="id">The resource id.</param>
        public ResourceProfile(string id)
        {
            if (id == null)
                throw new ArgumentNullException("ResourceProfile: id cannot be null");
            Id = id;
        }
        /// <summary>
        /// This is the Id of the resource profile.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Implicitly converts a string in to a resource profile.
        /// </summary>
        /// <param name="id">The name of the resource profile.</param>
        public static implicit operator ResourceProfile(string id)
        {
            return new ResourceProfile(id);
        }
    }
}
