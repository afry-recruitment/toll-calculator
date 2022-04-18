import fetch from "node-fetch";
import appCache from "./appCache.js";

export default class dateServices{
    constructor(){
        this.cache = new appCache();
    }

    isTollFreeDate(date){

    }

    /**
    * https://date.nager.at/Api
    */
    async getHolidays(year,country){
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