# BlazorWebAssemblyFluxor

Blazor Web Assembly test project with [Fluxor](https://github.com/mrpmorris/fluxor).

[Fluxor + C#9 - Redux Pattern in Blazor WebAssembly](https://youtu.be/sAyH-O0dFaI).

## Installing and using Fluxor

1. Install the ***Fluxor.Blazor.Web*** NuGet package.
2. **Optional**: Instal the ***Fluxor.Blazor.Web.ReduxDevTools*** NuGet package.
This package is necessary only if [Redux Dev Tools](https://microsoftedge.microsoft.com/addons/search/redux%20dev%20tools) will be used.
3. Add the following line to ***index.html***:

```html
<body>
...
<script src="_content/Fluxor.Blazor.Web/scripts/index.js"></script>
</body>
````

4. Add *<Fluxor.Blazor.Web.StoreInitializer />* to ***App.razor***:

```C#
<Fluxor.Blazor.Web.StoreInitializer />
...
<Router AppAssembly="@typeof(Program).Assembly" PreferExactMatches="@true">
...
</Router>
```
***<Fluxor.Blazor.Web.StoreInitializer />*** initialises the store automatically.

5. Add the following code to ***Main*** in ***Program.cs***:

```C#
...

using Fluxor;

...

public static async Task Main(string[] args)
{
    var builder = WebAssemblyHostBuilder.CreateDefault(args);
    builder.RootComponents.Add<App>("#app");

    ...

    builder.Services.AddFluxor(options => options
            .ScanAssemblies(typeof(Program).Assembly)
            .UseReduxDevTools() // Optional. Needed if Redux Dev Tools will be used. Requires the Fluxor.Blazor.Web.ReduxDevTools NuGet package.
            .UseRouting()
            );

    ...

    await builder.Build().RunAsync();
}
```

6. Create a folder named ***Store***. For every component that needs to manage and/or share state, create a
subfolder under the ***Store*** folder with its name and create the following classes inside
(**note**: for **state** and **action**, a *record* can be used):
- *State*;
- *Feature* (this class describes the state to the store);
- *Action*;
- *Reducers*.

**Example:**

- *Counter****State***:
```C#
namespace BlazorWebAssemblyFluxor.Store.Counter
{
    public record CounterState
    {
        public int ClickCount { get; init; }
    }
}
```
- *Counter****Feature***:
```C#
using Fluxor;

namespace BlazorWebAssemblyFluxor.Store.Counter
{
    public class CounterFeature : Feature<CounterState>
    {
        public override string GetName() => nameof(CounterState);

        protected override CounterState GetInitialState() => new() { ClickCount = 0 };
    }
}
```
- *Counter****ActionIncrement***:

```C#
namespace BlazorWebAssemblyFluxor.Store.Counter
{
    public record CounterActionIncrement { }
}
```
- *Counter****Reducers*** (don't forget to decorate methods with ***[ReducerMethod]***):
```C#
using Fluxor;

namespace BlazorWebAssemblyFluxor.Store.Counter
{
    public class CounterReducers
    {
        [ReducerMethod]
        public static CounterState ReduceCounterActionIncrement(CounterState state, CounterActionIncrement action)
        {
            return state with
            {
                ClickCount = state.ClickCount + 1
            };
        }

        [ReducerMethod]
        public static CounterState ReduceCounterActionReset(CounterState state, CounterActionReset action) => new() { ClickCount = 0 };
    }
}
```

7. Add Fluxor to every component that needs to manage and/or share state. Example:

```C#
@page "/counter"

@inherits Fluxor.Blazor.Web.Components.FluxorComponent

@using Fluxor
@using BlazorWebAssemblyFluxor.Store.Counter

@inject IState<CounterState> CounterState
@inject IDispatcher Dispatcher

<h1>Counter</h1>

<p>Current count: @CounterState.Value.ClickCount</p>

<button class="btn btn-primary" @onclick="IncrementCount">Click me</button>

@code {
    private int currentCount = 0;

    private void IncrementCount()
    {
        var action = new IncrementCounterAction();
        Dispatcher.Dispatch(action);
    }
}
```

**NOTES:**

- The components must:

  - Use ***Fluxor***:
```C#
@using Fluxor
```
- Inherit from ***Fluxor.Blazor.Web.Components.FluxorComponent***:
```C#
@inherits Fluxor.Blazor.Web.Components.FluxorComponent
```
  - Use the store part(s) they need. Example:
```C#
@using BlazorWebAssemblyFluxor.Store.Counter
```
  - Inject ***IDispatcher***:
```C#
@inject IDispatcher Dispatcher
```  
