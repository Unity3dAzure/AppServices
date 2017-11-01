﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Azure.AppServices {
  // App Service authentication identity providers: Microsoft Account, Google Plus, Twitter, Facebook, Azure Active Directory
  public enum AuthenticationProvider {
    MicrosoftAccount = 0,
    Google = 1,
    Twitter = 2,
    Facebook = 3,
    AAD = 4
  }
}
