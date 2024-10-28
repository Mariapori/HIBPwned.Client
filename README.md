# HIBPwned Client

**HIBPwned Client** is a C# class library that checks whether a password has been compromised using the [Have I Been Pwned](https://haveibeenpwned.com/) "Pwned Passwords" API. It provides a simple and efficient way to verify the safety of passwords by checking them against a database of known breached passwords.

## Installation

Add this class library to your project by including the `HIBPwned.cs` file. Ensure that your project has access to `System.Net.Http`, `System.Linq`, `System.Security.Cryptography`, and `System.Text`.

## Usage

Here's an example of how to use the `IsPasswordPwned` method in your application:

```csharp
using System;
using HIBPwned;

namespace MyApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string password = "YourPassword123";
            bool isPwned = Client.IsPasswordPwned(password);

            if (isPwned)
            {
                Console.WriteLine("Warning: This password has been compromised in a data breach.");
            }
            else
            {
                Console.WriteLine("Safe: This password has not been found in known breaches.");
            }
        }
    }
}
