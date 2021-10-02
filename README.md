# MakeGenericAgain
Problem is that nswag client code generation from open api specification or swagger generates classes without generics. This tool can be used afterwards to make classes generic again


To install it on other projects, add this to the csproj:

```
<ItemGroup>
  <PackageReference Include="nksoft.MakeGenericAgain" Version="1.0.0" />
</ItemGroup>
<ItemGroup>
  <DotNetCliToolReference Include="nksoft.MakeGenericAgain" Version="1.0.0" />
</ItemGroup>
```
