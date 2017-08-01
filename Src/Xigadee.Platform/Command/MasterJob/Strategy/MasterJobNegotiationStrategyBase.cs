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

#region using
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Threading;
using Xigadee;
#endregion
namespace Xigadee
{
    /// <summary>
    /// The base functionality for the master job negotiation strategy.
    /// </summary>
    public abstract class MasterJobNegotiationStrategyBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MasterJobNegotiationStrategyBase"/> class.
        /// </summary>
        /// <param name="name">The strategy name.</param>
        protected MasterJobNegotiationStrategyBase(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Gets the strategy name.
        /// </summary>
        public string Name { get; }
    }
}