# ApiKeyAuth
 A sample of APi Key Authorization from Service 2 Service

Using an API Key that is set in the project which allows another request to it using the same api key in the header.

> NOTE: The key is just used as an example of implementation. It is a `GUID` stored in the `appsettings.json` for **demo purposes only**.

## Request example

`https://localhost:7203/weather` with a header of `x-api-key = 83565FE196334682A1A08665129E7B1D`

![api-request](https://user-images.githubusercontent.com/20805058/225124197-6d75eb84-10e6-4685-a1c3-f8a76ef9236c.png)

## 3 ways to handles this

1. As a Middleware

example

```C# "program.cs"
app.UseMiddleware<ApiKeyAuthMiddleware>();
```

| Pros  | Cons |
| ------------- | ------------- |
| + easy to setup  | - applies to all controllers  |
| + automatically applies to any new code | - applies to all function / requests  |


2. As a Service Filter

example

| Pros  | Cons |
| ------------- | ------------- |
| + fine control  | - have to apply to any new code  |


3. As a Attribute and Service Filter

example

| Pros  | Cons |
| ------------- | ------------- |
| + fine control  | - have to apply to any new code  |
| + ease of use in code  | - harder to unit test / moq  |