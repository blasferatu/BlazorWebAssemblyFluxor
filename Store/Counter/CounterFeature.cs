using Fluxor;

namespace BlazorWebAssemblyFluxor.Store.Counter
{
    public class CounterFeature : Feature<CounterState>
    {
        public override string GetName() => nameof(CounterState);

        protected override CounterState GetInitialState() => new() { ClickCount = 0 };
    }
}
