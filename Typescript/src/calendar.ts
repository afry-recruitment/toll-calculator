import Holidays from "date-holidays";

const holidays = new Holidays("SE");

export function isTollFreeDate(date: Date): boolean {
    return isWeekend(date) || isHoliday(date);
}

function isWeekend(date: Date): boolean {
    const day = date.getDay();
    return day === 0 || day === 6; 
}

function isHoliday(date: Date): boolean {
    const holiday = holidays.isHoliday(date);

    if (!holiday) {
        return false;
    }

    return holiday.some(day => day.type === "public");
}