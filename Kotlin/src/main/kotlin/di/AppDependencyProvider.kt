package di

import toll.TolLCalculator
import toll.TollCalculatorImpl
import toll.TollLogic
import toll.TollLogicImpl

class AppDependencyProvider {

    // provide singleton
    companion object {
        private val logicImpl = TollLogicImpl()
        fun provideTollLogic(): TollLogic = logicImpl

        private val tollCalculatorImpl = TollCalculatorImpl()
        fun provideTollCalculator(): TolLCalculator = tollCalculatorImpl
    }
}
