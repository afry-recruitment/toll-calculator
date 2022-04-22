import fetch from "node-fetch";
import appCache from "./appCache.js";
import {holidays,holiMonths} from "../config/holidays.js";
export default class dateServices{
    constructor(){
        this.cache = new appCache();
    }

    isTollFreeDate(date){
        if (this.isWeekend(date)) return true;
        for (const holiMonth of holiMonths) {
            if (this.isSameMonth(new Date(holiMonth), date)) return true;
        }

        for (const holiday of holidays) {
            if (this.isSameDay(new Date(holiday), date)) return true;
        }
    
        return false;
    }


    // 6 = Saturday, 0 = Sunday
    isWeekend(date) {
        return [0,6].includes(date.getDay());
    }

    isSameMonth(a,b){
        return (
            a.getFullYear() === b.getFullYear() &&
            b.getMonth() === b.getMonth()
        );   
    }
    
    isSameDay(a, b) {
        return (
            a.getFullYear() === b.getFullYear() &&
            a.getMonth() === b.getMonth() &&
            a.getDate() === b.getDate()
        );
    }
    



    /**
    * https://date.nager.at/Api
    */
    async getRemoteHolidays(year,country){
        try{
            if (this.cache.has(`${year}${country}`))
                return this.cache.get(`${year}${country}`)
            const url = `https://date.nager.at/api/v3/PublicHolidays/${year}/${country}`;
            const result = await fetch(url);
            const data = result.json();
            this.cache.set(`${year}${country}`,data)
            return data
        }catch(e){
            console.error(e);
            return false;
        }
    
    }
}