package di

import toll.TollLogic
import toll.TollLogicImpl

class AppDependencyProvider {
    companion object {
        // provide same instance
        private val logicImpl = TollLogicImpl()
        fun provideTollLogic(): TollLogic = logicImpl
    }
}