


export function getTollFeeForTime(date: Date): number {
    const hour = date.getHours();
    const minute = date.getMinutes();

    if (hour === 6 && minute < 30) return 8;
    if (hour === 6) return 13;
    if (hour === 7) return 18;
    if (hour === 8 && minute < 30) return 13;
    if (hour >= 8 && hour <= 14) return 8;
    if (hour === 15 && minute < 30) return 13;
    if (hour === 15 || hour === 16) return 18;
    if (hour === 17) return 13;
    if (hour === 18 && minute < 30) return 8;

    return 0;
}
