1.  First generate PAT (Personal Access Token) in Github\
    [Follow these steps to generate a PAT](https://docs.github.com/en/authentication/keeping-your-account-and-data-secure/creating-a-personal-access-token#creating-a-token)\
    **Very important to select the read:packages options** [![enter image description here](https://i.stack.imgur.com/39b29.png)](https://i.stack.imgur.com/39b29.png)\
    **Github will show you the PAT for ONLY ONE TIME, So make sure to copy it to save place, otherwise, you have to generate it again**

2.  Now with PAT in your hand, Add `Nuget.Config` file to your project\
    [![enter image description here](https://i.stack.imgur.com/dQmXo.png)](https://i.stack.imgur.com/dQmXo.png)\
    The content of the file should be like following

    ```
    <?xml version="1.0" encoding="utf-8"?>
    <configuration>
        <packageSources>
            <clear />
            <add key="github" value="https://nuget.pkg.github.com/OWNER/index.json" />
        </packageSources>
        <packageSourceCredentials>
            <github>
                <add key="Username" value="USERNAME" />
                <add key="ClearTextPassword" value="TOKEN" />
            </github>
        </packageSourceCredentials>
    </configuration>

    ```

3.  You must replace:

    -   USERNAME with the name of your user account on GitHub.

    -   TOKEN with your personal access token **(the token you generated in Step 1)**.

    -   OWNER with the name of the user or organization account that owns the repository containing your project.
4.  You must restart Visual Studio or even Restart the PC **This is important**

After that open the Terminal and copy and paste (maybe with some modifications) the statement that Github give to you to install the package

[![enter image description here](https://i.stack.imgur.com/XA7ZS.png)](https://i.stack.imgur.com/XA7ZS.png)

*Now you are ready to get the Package*.

* * * * *

**UPDATE**

> How to configure github nuget packages for teams?

Configure the `nuget.config` using *environment variables*:

```
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <add key="NuGet official package source" value="https://api.nuget.org/v3/index.json" />
    <add key="Github" value="https://nuget.pkg.github.com/OWNER/index.json" />
  </packageSources>
  <packageSourceCredentials>
    <Github>
      <add key="Username" value="%GITHUB_PACKAGE_USER_NAME%" />
      <add key="ClearTextPassword" value="%GITHUB_PACKAGE_TOKEN%" />
    </Github>
  </packageSourceCredentials>
</configuration>

```

The `GITHUB_PACKAGE_USER_NAME` and `GITHUB_PACKAGE_TOKEN` can be anything you want.

Now, each team member should configure their **user** environment variables:

-   `GITHUB_PACKAGE_USER_NAME`: team member github user name
-   `GITHUB_PACKAGE_TOKEN`: team member Personal Access Token (PAT)

With those configurations, Visual Studio will be able to query and download packages, **assuming** the *team member* has access to the OWNER package repository.

> How to restore github nuget packages in **github actions** workflows?

With the previous `NuGet.config` configured, you need to change your workflow passing the required environment variables, like so:

```
- name: Restore dependencies
  env:
    GITHUB_PACKAGE_USER_NAME: ${{ github.actor }}
    GITHUB_PACKAGE_TOKEN: ${{ secrets.RESTORE_ORGANIZATION_PACKAGES }}
  run: dotnet restore ./src

```

Since you can't pass your PAT, you need to configure a github secret (either for the [repository](https://docs.github.com/en/actions/security-guides/encrypted-secrets#creating-encrypted-secrets-for-a-repository) or the [organization](https://docs.github.com/en/actions/security-guides/encrypted-secrets#creating-encrypted-secrets-for-an-organization)). In the above example, I created a secret named `RESTORE_ORGANIZATION_PACKAGES` with `read:packages` permission at the repository level.
