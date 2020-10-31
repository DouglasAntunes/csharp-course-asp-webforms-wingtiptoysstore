# C# Course - Getting Started with ASP .NET Web Forms

It's an application named `WingTip Toys`, a simplified storefront website for selling items online with PayPal® Express checkout.

This app was done using [this tutorial](https://docs.microsoft.com/en-us/aspnet/web-forms/overview/getting-started/getting-started-with-aspnet-45-web-forms/), updated to Bootstrap to v4.5 and Code simplified & refactored to the best practices that i have knowledge (C# & HTML5).

And was fully converted to use bootstrap layout. On the tutorial was using table layout and old html.

## Usage

1. Project done using Visual Studio 2019.

2. Clone/Fork this repository.

3. Create the file `AppSettingsSecrets.config` based on the file `AppSettingsSecrets.template.config` located on `src/WingtipToys`.

4. Go to [PayPal® Developer](https://developer.paypal.com) and get your `APIUsername`, `APIPassword` and `APISignature` and change the keys that start with `PayPal_`.

5. [OPTIONAL] Create an Application Project for Google OAuth and get the `ClientId` and `ClientSecret` and change the keys that start with `Google_`.

6. [OPTIONAL] You can add other OAuth providers and change on the `AppSettingsSecrets.config` file.

7. Open Solution.

8. Restore NuGet packages.

9. Run the aplication.
