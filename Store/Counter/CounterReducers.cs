using Fluxor;

namespace BlazorWebAssemblyFluxor.Store.Counter
{
    public class CounterReducers
    {
        [ReducerMethod]
        public static CounterState ReduceCounterActionIncrement(CounterState state, CounterActionIncrement _)
        {
            return state with
            {
                ClickCount = state.ClickCount + 1
            };
        }

        [ReducerMethod]
        public static CounterState ReduceCounterActionReset(CounterState state, CounterActionReset _) => new() { ClickCount = 0 };
    }
}
